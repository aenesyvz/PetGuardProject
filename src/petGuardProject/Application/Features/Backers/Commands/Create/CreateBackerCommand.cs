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
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Backers.Commands.Create;

public class CreateBackerCommand : IRequest<CreatedBackerRespone>
{
    public string Email { get; set; }
    //public string Password { get; set; }
    //public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public string IpAddress { get; set; }


    public class CreateBackerCommandHandler : IRequestHandler<CreateBackerCommand, CreatedBackerRespone>
    {
        private readonly IBackerRepository _backerRepository;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IMapper _mapper;
        private readonly MernisServiceBase _mernisServiceBase;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly BackerBusinessRules _backerBusinessRules;

        public CreateBackerCommandHandler(IBackerRepository backerRepository, IAuthService authService, IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, IMapper mapper, MernisServiceBase mernisServiceBase,AuthBusinessRules authBusinessRules, BackerBusinessRules backerBusinessRules)
        {
            _backerRepository = backerRepository;
            _authService = authService;
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _mapper = mapper;
            _mernisServiceBase = mernisServiceBase;
            _authBusinessRules = authBusinessRules;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<CreatedBackerRespone> Handle(CreateBackerCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.Email);

            await _mernisServiceBase.CheckIfRealPerson(nationalityNumber: Convert.ToInt64(request.NationalityNumber), 
                                                       firstName: request.FirstName, 
                                                       lastName: request.LastName, 
                                                       yearOfBirth: request.DateOfBirth.Year);

            string password = UtilityManager.GeneratePassword(length: 6, 
                                                              includeLowercase: true, 
                                                              includeUppercase: true, 
                                                              includeNumbers: true, 
                                                              includeSpecialChars: false);


            User createdUser = await CreateUser(request,password);
            Backer createdBacker = await CreateBacker(request, createdUser);

            await AddBackerOperationClaims(createdUser);

            CreatedBackerRespone respone = _mapper.Map<CreatedBackerRespone>(createdBacker);
            return respone;
            
        }

        private async Task<User> CreateUser(CreateBackerCommand request,string password)
        {
           HashingHelper.CreatePasswordHash(
               password,
               passwordHash: out byte[] passwordHash,
               passwordSalt: out byte[] passwordSalt
           );

            User newUser =
                new()
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    LockoutEnabled=true,
                    AccessFailedCount=0
                };
            User createdUser = await _userService.AddAsync(newUser);
            return createdUser;
        }

        private async Task<Backer> CreateBacker(CreateBackerCommand request, User user)
        {
            Backer backer = _mapper.Map<Backer>(request);
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

        private async Task CreateTokens(CreateBackerCommand request, User createdUser)
        {
            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
        }
    }
}
