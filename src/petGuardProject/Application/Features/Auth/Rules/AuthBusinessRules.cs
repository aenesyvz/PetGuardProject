using Application.Features.Auth.Constants;
using Application.Features.UserOperationClaims.Constants;
using Application.Features.Users.Contants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Enums;
using Core.Security.Hashing;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using NArchitecture.Core.Localization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Rules;


public class AuthBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly ILocalizationService _localizationService;
    private readonly LockoutSettings _lockoutSettings;

    public AuthBusinessRules(IUserRepository userRepository, ILocalizationService localizationService,IConfiguration configuration)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;
        _lockoutSettings = configuration.GetSection("LockoutSettings").Get<LockoutSettings>()!;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, AuthMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task EmailAuthenticatorShouldBeExists(EmailAuthenticator? emailAuthenticator)
    {
        if (emailAuthenticator is null)
            await throwBusinessException(AuthMessages.EmailAuthenticatorDontExists);
    }

    public async Task OtpAuthenticatorShouldBeExists(OtpAuthenticator? otpAuthenticator)
    {
        if (otpAuthenticator is null)
            await throwBusinessException(AuthMessages.OtpAuthenticatorDontExists);
    }

    public async Task OtpAuthenticatorThatVerifiedShouldNotBeExists(OtpAuthenticator? otpAuthenticator)
    {
        if (otpAuthenticator is not null && otpAuthenticator.IsVerified)
            await throwBusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists);
    }

    public async Task EmailAuthenticatorActivationKeyShouldBeExists(EmailAuthenticator emailAuthenticator)
    {
        if (emailAuthenticator.ActivationKey is null)
            await throwBusinessException(AuthMessages.EmailActivationKeyDontExists);
    }

    public async Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
            await throwBusinessException(AuthMessages.UserDontExists);
    }

    public async Task UserShouldNotBeHaveAuthenticator(User user)
    {
        if (user.AuthenticatorType != AuthenticatorType.None)
            await throwBusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);
    }

    public async Task RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
            await throwBusinessException(AuthMessages.RefreshDontExists);
    }

    public async Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.RevokedDate != null && DateTime.UtcNow >= refreshToken.ExpirationDate)
            await throwBusinessException(AuthMessages.InvalidRefreshToken);
    }

    public async Task UserEmailShouldBeNotExists(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Email == email);
        if (doesExists)
            await throwBusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserPasswordShouldBeMatch(User user, string password)
    {
        await CheckIfUserIsLockedOut(user);

        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            await HandleFailedAccessAttempt(user);
            await throwBusinessException(UsersMessages.PasswordDontMatch);
        }

        await HandleSuccessAccessAttempt(user);
    }



    private async Task CheckIfUserIsLockedOut(User user)
    {
        if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
        {
            await throwBusinessException(UsersMessages.UserAccountLockout);
        }
    }

    private async Task HandleFailedAccessAttempt(User user)
    {
        if (!user.LockoutEnabled) return;

        user.AccessFailedCount++;

        if (user.AccessFailedCount >= _lockoutSettings.MaxFailedAccessAttempts)
        {
            user.LockoutEnd = DateTime.UtcNow.AddMinutes(_lockoutSettings.LockoutDurationInMinutes);
        }

        await _userRepository.UpdateAsync(user);
    }

    private async Task HandleSuccessAccessAttempt(User user)
    {
        if (!user.LockoutEnabled) return;

        user.AccessFailedCount = 0;
        user.LockoutEnd = null;

        await _userRepository.UpdateAsync(user);
    }
}