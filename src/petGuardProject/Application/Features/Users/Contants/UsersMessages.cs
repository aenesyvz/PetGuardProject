using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Contants;
public static class UsersMessages
{
    public const string SectionName = "Users";

    public const string UserDontExists = "UserDontExists";
    public const string PasswordDontMatch = "PasswordDontMatch";
    public const string UserMailAlreadyExists = "UserMailAlreadyExists";
    public const string UserAccountLockout = "UserAccountLockout";
}
