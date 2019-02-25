using System;
using System.Collections.Generic;

namespace ImportBackstageUsers.BackstageModels
{
    public partial class AibolUser
    {
        public Guid Id { get; set; }
        public int Serial { get; set; }
        public Guid? TenantId { get; set; }
        public string TenantBvalue { get; set; }
        public string TenantTypeName { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string JobTitle { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Avatar { get; set; }
        public byte Gender { get; set; }
        public string SpellChar { get; set; }
        public DateTime? Birthday { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Notification { get; set; }
        public int Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedWhen { get; set; }
        public string JobPosition { get; set; }
        public string IdentityCard { get; set; }
        public int? Age { get; set; }
        public Guid? PostId { get; set; }
    }
}
