using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UtilitiesService;


public static class UtilityManager
{
   

    public static string GeneratePassword(int length, bool includeLowercase, bool includeUppercase, bool includeNumbers, bool includeSpecialChars)
    {
         const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
         const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
         const string NumberChars = "0123456789";
         const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        if (length <= 0)
        {
            throw new ArgumentException("Password length must be greater than zero.", nameof(length));
        }

        if (!includeLowercase && !includeUppercase && !includeNumbers && !includeSpecialChars)
        {
            throw new ArgumentException("At least one character type must be selected.");
        }

        StringBuilder validChars = new StringBuilder();
        if (includeLowercase) validChars.Append(LowercaseChars);
        if (includeUppercase) validChars.Append(UppercaseChars);
        if (includeNumbers) validChars.Append(NumberChars);
        if (includeSpecialChars) validChars.Append(SpecialChars);

        Random rnd = new Random();
        StringBuilder passwordBuilder = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            char randomChar = validChars[rnd.Next(validChars.Length)];
            passwordBuilder.Append(randomChar);
        }

        return passwordBuilder.ToString();
    }

}