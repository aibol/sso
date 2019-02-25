using System;
using System.Collections.Generic;

namespace ImportBackstageUsers.BackstageModels
{
    public partial class AibolLogin
    {
        public Guid Id { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordEncrypted { get; set; }
        public DateTime? LastLogonWhen { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
