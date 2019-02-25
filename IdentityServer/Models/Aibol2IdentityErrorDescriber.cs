using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class Aibol2IdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError {Code = nameof(DefaultError), Description = $"发生了未知错误，Code: 0x000001"};
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "Optimistic concurrency failure, object has been modified."
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError {Code = nameof(PasswordMismatch), Description = "Incorrect password."};
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError {Code = nameof(InvalidToken), Description = "Invalid token."};
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "A user with this login already exists."
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"User name '{userName}' is invalid, can only contain letters or digits."
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError {Code = nameof(InvalidEmail), Description = $"Email '{email}' is invalid."};
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"用户名/邮箱 '{userName}' 已经被占用"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"邮箱 '{email}' 已经被占用"
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError {Code = nameof(InvalidRoleName), Description = $"Role name '{role}' is invalid."};
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"Role name '{role}' is already taken."
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "User already has a password set."
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = "Lockout is not enabled for this user."
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = $"User already in role '{role}'."
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError {Code = nameof(UserNotInRole), Description = $"User is not in role '{role}'."};
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"密码至少需要 {length} 位"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "密码至少需要一个字符"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "密码至少需要一个 0至9 中的数字"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "密码中需要包含至少一个小写字母"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "密码中需要包含至少一个大写字母"
            };
        }
    }
}