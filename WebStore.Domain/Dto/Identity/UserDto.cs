using System;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Dto.Identity
{
    public abstract class UserDto
    {
        public User User { get; set; }
    }
    
    public class AddLoginDto : UserDto
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDto : UserDto
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDto : UserDto
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}