using System;
using UserService.Infrastructure.Interfaces;

namespace UserService.Infrastructure.Models
{
    public class User : IValidation
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public bool IsValid()
        {
            return  !string.IsNullOrEmpty(FirstName) && FirstName.Length <= 50 &&
                !string.IsNullOrEmpty(LastName) && LastName.Length <= 50 &&
                Age < 120 && Age > 0 &&
                !string.IsNullOrEmpty(Email) && Email.Length <= 50;
        }
    }
}