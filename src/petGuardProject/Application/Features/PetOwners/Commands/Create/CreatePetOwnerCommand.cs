using Application.Features.Auth.Rules;
using Application.Features.PetOwners.Constants;
using Application.Features.PetOwners.Rules;
using Application.Services.AuthService;
using Application.Services.OperationClaimsService;
using Application.Services.Repositories;
using Application.Services.UserOperationClaimsService;
using Application.Services.UsersService;
using AutoMapper;
using Core.Security.Hashing;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.PetOwners.Commands.Create;

public class CreatePetOwnerCommand : IRequest<CreatedPetOwnerResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public string PhoneNumber { get; set; }


    public class CreatePetOwnerCommandHandler : IRequestHandler<CreatePetOwnerCommand, CreatedPetOwnerResponse>
    {
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        public CreatePetOwnerCommandHandler(IPetOwnerRepository petOwnerRepository, IMapper mapper, IAuthService authService, IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, AuthBusinessRules authBusinessRules, PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
            _authService = authService;
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _authBusinessRules = authBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<CreatedPetOwnerResponse> Handle(CreatePetOwnerCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.Email);

            User createdUser =  await CreateUser(request);
            PetOwner createdPetOwner = await CreatePetOwner(request, createdUser);

            await AddPetOwnerOperationClaims(createdUser);

            CreatedPetOwnerResponse response = _mapper.Map<CreatedPetOwnerResponse>(createdPetOwner);
            return response;

        }


        private async Task<User> CreateUser(CreatePetOwnerCommand request)
        {
            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            User newUser =
                new()
                {
                    Email = request.Email,
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
            PetOwner petOwner = _mapper.Map<PetOwner>(request);
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


    }
}
