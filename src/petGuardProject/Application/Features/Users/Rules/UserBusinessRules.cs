using Application.Features.Auth.Constants;
using Application.Features.Users.Contants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Hashing;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using NArchitecture.Core.Localization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly ILocalizationService _localizationService;
    private LockoutSettings _lockoutSettings;

    public UserBusinessRules(IUserRepository userRepository, ILocalizationService localizationService)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;

    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, UsersMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
            await throwBusinessException(UsersMessages.UserDontExists);
    }

    public async Task UserIdShouldBeExistsWhenSelected(Guid id)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: u => u.Id == id);
        if (doesExist)
            await throwBusinessException(UsersMessages.UserDontExists);
    }

    public async Task UserPasswordShouldBeMatched(User user, string password)
    {
    

        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            await throwBusinessException(UsersMessages.PasswordDontMatch);
        }

      
    }

    

    public async Task UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Email == email);
        if (doesExists)
            await throwBusinessException(UsersMessages.UserMailAlreadyExists);
    }

    public async Task UserEmailShouldNotExistsWhenUpdate(Guid id, string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Id != id && u.Email == email);
        if (doesExists)
            await throwBusinessException(UsersMessages.UserMailAlreadyExists);
    }

}
