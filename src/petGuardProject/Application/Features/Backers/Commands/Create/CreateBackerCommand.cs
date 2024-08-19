using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Rules;
using Application.Features.Backers.Constants;
using Application.Features.Backers.Rules;
using Application.Services.AuthService;
using Application.Services.MernisService;
using Application.Services.OperationClaimsService;
using Application.Services.Repositories;
using Application.Services.UserOperationClaimsService;
using Application.Services.UsersService;
using Application.Services.UtilitiesService;
using AutoMapper;
using Core.Mailing;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Application.Features.Backers.Commands.Create;

public class CreateBackerCommand : IRequest<CreatedBackerResponse>
{

    public BackerForRegisterDto BackerForRegisterDto { get; set; }
    public string IpAddress { get; set; }


    public class CreateBackerCommandHandler : IRequestHandler<CreateBackerCommand, CreatedBackerResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IBackerRepository _backerRepository;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly MernisServiceBase _mernisServiceBase;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly BackerBusinessRules _backerBusinessRules;

        private IConnection _connection;
        private IModel _channel;
        private const string _exchangeName = "emailExchange";
        private const string _queueName = "emailQueue";

        public CreateBackerCommandHandler(IConfiguration configuration,IBackerRepository backerRepository, IAuthService authService, IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, IMailService mailService, IMapper mapper, MernisServiceBase mernisServiceBase, AuthBusinessRules authBusinessRules, BackerBusinessRules backerBusinessRules)
        {
            _configuration = configuration;
            _backerRepository = backerRepository;
            _authService = authService;
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _mailService = mailService;
            _mapper = mapper;
            _mernisServiceBase = mernisServiceBase;
            _authBusinessRules = authBusinessRules;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<CreatedBackerResponse> Handle(CreateBackerCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.BackerForRegisterDto.Email);

            await _mernisServiceBase.CheckIfRealPerson(nationalityNumber: Convert.ToInt64(request.BackerForRegisterDto.NationalityNumber),
                                                       firstName: request.BackerForRegisterDto.FirstName,
                                                       lastName: request.BackerForRegisterDto.LastName,
                                                       yearOfBirth: request.BackerForRegisterDto.DateOfBirth.Year);

            string password = UtilityManager.GeneratePassword(length: 6,
                                                              includeLowercase: true,
                                                              includeUppercase: true,
                                                              includeNumbers: true,
                                                              includeSpecialChars: false);


            User createdUser = await CreateUser(request, password);
            Backer createdBacker = await CreateBacker(request, createdUser);

            await AddBackerOperationClaims(createdUser);

            Mail mail = await CreateEmailTemplate(request.BackerForRegisterDto, password);

            SendEmail(mail);

            CreatedBackerResponse respone = _mapper.Map<CreatedBackerResponse>(createdBacker);
            return respone;

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

        private async Task<User> CreateUser(CreateBackerCommand request, string password)
        {
            HashingHelper.CreatePasswordHash(
                password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            User newUser =
                new()
                {
                    Email = request.BackerForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };
            User createdUser = await _userService.AddAsync(newUser);
            return createdUser;
        }

        private async Task<Backer> CreateBacker(CreateBackerCommand request, User user)
        {
            Backer backer = _mapper.Map<Backer>(request.BackerForRegisterDto);
            backer.UserId = user.Id;
            backer.ImageUrl = null;

            await _backerRepository.AddAsync(backer);
            return backer;
        }

        private async Task AddBackerOperationClaims(User createdUser)
        {
            OperationClaim? operationClaim = await _operationClaimService.GetAsync(x => x.Name == BackersOperationClaims.Admin);
            UserOperationClaim userOperationClaim = new UserOperationClaim();

            userOperationClaim.OperationClaimId = operationClaim!.Id;
            userOperationClaim.UserId = createdUser.Id;

            await _userOperationClaimService.AddAsync(userOperationClaim);
        }


        private async Task<Mail> CreateEmailTemplate(BackerForRegisterDto backerForRegisterDto, string password)
        {
            Mail mail = new Mail();
            mail.ToList = new List<MailboxAddress>
            {
                new MailboxAddress($"{backerForRegisterDto.FirstName} ${backerForRegisterDto.LastName}",address:backerForRegisterDto.Email)
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
                                    .password-container {{
                                        display: flex;
                                        justify-content: center;
                                        gap: 10px;
                                        margin-bottom: 30px;
                                    }}
                                    .password-box {{
                                        width: 50px;
                                        height: 50px;
                                        background-color: #023047;
                                        color: #ffffff;
                                        font-size: 24px;
                                        text-align: center;
                                        line-height: 50px;
                                        border-radius: 5px;
                                    }}
                                    .change-password-text {{
                                        font-size: 16px;
                                        color: #777777;
                                        text-align: center;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <p class='welcome-text'>Welcome to PETGUARD</p>
                                    <p class='info-text'>Dear {backerForRegisterDto.FirstName} ${backerForRegisterDto.LastName}, thank you for joining us. Below is your password:</p>
                                    <div class='password-container'>
                                        {string.Join("", password.Select(c => $"<div class='password-box'>{c}</div>"))}
                                    </div>
                                    <p class='change-password-text'>Please remember to change your password after your first login.</p>
                                </div>
                            </body>
                            </html>";

            return mail;

        }

        private async Task CreateTokens(CreateBackerCommand request, User createdUser)
        {
            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
        }



    }
}
