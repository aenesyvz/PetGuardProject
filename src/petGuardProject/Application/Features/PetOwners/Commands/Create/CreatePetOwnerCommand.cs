using Application.Features.Auth.Rules;
using Application.Features.Backers.Commands.Create;
using Application.Features.PetOwners.Constants;
using Application.Features.PetOwners.Rules;
using Application.Services.AuthService;
using Application.Services.MernisService;
using Application.Services.OperationClaimsService;
using Application.Services.Repositories;
using Application.Services.UserOperationClaimsService;
using Application.Services.UsersService;
using AutoMapper;
using Core.Mailing;
using Core.Security.Hashing;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace Application.Features.PetOwners.Commands.Create;

public class CreatePetOwnerCommand : IRequest<CreatedPetOwnerResponse>
{
    public PetOwnerForRegisterDto PetOwnerForRegisterDto { get; set; }
    public string IpAddress { get; set; }


    public class CreatePetOwnerCommandHandler : IRequestHandler<CreatePetOwnerCommand, CreatedPetOwnerResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly MernisServiceBase _mernisServiceBase;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        private IConnection _connection;
        private IModel _channel;
        private const string _exchangeName = "emailExchange";
        private const string _queueName = "emailQueue";


        public CreatePetOwnerCommandHandler(IConfiguration configuration,IPetOwnerRepository petOwnerRepository, IMapper mapper, IAuthService authService, IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, MernisServiceBase mernisServiceBase,AuthBusinessRules authBusinessRules, PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _configuration = configuration;
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
            _authService = authService;
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _mernisServiceBase = mernisServiceBase;
            _authBusinessRules = authBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<CreatedPetOwnerResponse> Handle(CreatePetOwnerCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.PetOwnerForRegisterDto.Email);

            await _mernisServiceBase.CheckIfRealPerson(
                                                        nationalityNumber: Convert.ToInt64(request.PetOwnerForRegisterDto.NationalityNumber),
                                                        firstName: request.PetOwnerForRegisterDto.FirstName,
                                                        lastName: request.PetOwnerForRegisterDto.LastName,
                                                        yearOfBirth: request.PetOwnerForRegisterDto.DateOfBirth.Year);

            User createdUser =  await CreateUser(request);
            PetOwner createdPetOwner = await CreatePetOwner(request, createdUser);

            await AddPetOwnerOperationClaims(createdUser);

            Mail mail = CreateEmailTemplate(request.PetOwnerForRegisterDto);

            SendEmail(mail);


            CreatedPetOwnerResponse response = _mapper.Map<CreatedPetOwnerResponse>(createdPetOwner);
            return response;

        }


        private async Task<User> CreateUser(CreatePetOwnerCommand request)
        {
            HashingHelper.CreatePasswordHash(
                request.PetOwnerForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            User newUser =
                new()
                {
                    Email = request.PetOwnerForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };
            User createdUser = await _userService.AddAsync(newUser);
            return createdUser;
        }

        private async Task<PetOwner> CreatePetOwner(CreatePetOwnerCommand request, User user)
        {
            PetOwner petOwner = _mapper.Map<PetOwner>(request.PetOwnerForRegisterDto);
            petOwner.UserId = user.Id;
            petOwner.ImageUrl = null;

            await _petOwnerRepository.AddAsync(petOwner);
            return petOwner;
        }

        private async Task AddPetOwnerOperationClaims(User createdUser)
        {
            OperationClaim? operationClaim = await _operationClaimService.GetAsync(x => x.Name == PetOwnersOperationClaims.Admin);
            UserOperationClaim userOperationClaim = new UserOperationClaim();

            userOperationClaim.OperationClaimId = operationClaim!.Id;
            userOperationClaim.UserId = createdUser.Id;

            await _userOperationClaimService.AddAsync(userOperationClaim);
        }


        private Mail CreateEmailTemplate(PetOwnerForRegisterDto petOwnerForRegisterDto)
        {
            Mail mail = new Mail();
            mail.ToList = new List<MailboxAddress>
            {
                new MailboxAddress($"{petOwnerForRegisterDto.FirstName} {petOwnerForRegisterDto.LastName}",address:petOwnerForRegisterDto.Email)
            };
            mail.Subject = "Verify Your Email - PETGUARD";
            mail.HtmlBody = $@"
                            <!DOCTYPE html>
                            <html lang='en'>
                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                <title>Welcome to PETGUARD</title>
                                <style>
                                    body {{
                                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                        background-color: #f0f0f0;
                                        margin: 0;
                                        padding: 20px;
                                        display: flex;
                                        justify-content: center;
                                        align-items: center;
                                        min-height: 100vh;
                                    }}
                                    .container {{
                                        background-color: #ffffff;
                                        padding: 30px;
                                        border-radius: 15px;
                                        box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
                                        max-width: 600px;
                                        width: 100%;
                                    }}
                                    .welcome-text {{
                                        font-size: 24px;
                                        font-weight: bold;
                                        color: #3498db;
                                        margin-bottom: 20px;
                                        text-align: center;
                                    }}
                                    .info-text {{
                                        font-size: 18px;
                                        color: #333333;
                                        margin-bottom: 30px;
                                        text-align: center;
                                    }}
                                  
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <p class='welcome-text'>Welcome to PETGUARD</p>
                                    <p class='info-text'>Dear {petOwnerForRegisterDto.FirstName} {petOwnerForRegisterDto.LastName}, thank you for joining us. Below is your password:</p>
         
                                    
                                </div>
                            </body>
                            </html>
            ";
            return mail;

        }


        private void SendEmail(Mail mail)
        {
            var uri = _configuration.GetValue<string>("RabbitMQConfiguration:Uri");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri!),
                UserName = _configuration.GetValue<string>("RabbitMQConfiguration:Username"),
                Password = _configuration.GetValue<string>("RabbitMQConfiguration:Password"),
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout, true, false);

            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _channel.QueueBind(queue: _queueName,
                              exchange: _exchangeName,
                              routingKey: "");


            string mailSerialize = JsonConvert.SerializeObject(mail);
            byte[] mailByte = Encoding.UTF8.GetBytes(mailSerialize);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: _exchangeName, "", basicProperties: properties, body: mailByte);
        }

    }
}
