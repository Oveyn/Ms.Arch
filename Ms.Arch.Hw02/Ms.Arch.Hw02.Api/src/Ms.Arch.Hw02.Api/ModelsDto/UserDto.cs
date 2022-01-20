using System.ComponentModel.DataAnnotations;
using Ms.Arch.Hw02.Api.Application.Models;

namespace Ms.Arch.Hw02.Api.ModelsDto
{
    public class UserDto
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public UserDto()
        {
        }

        public UserDto(UserModel userModel)
        {
            Id = userModel.Id;
            FirstName = userModel.FirstName;
            LastName = userModel.LastName;
            Phone = userModel.Phone;
            Email = userModel.Email;
        }

        public UserModel ConvertToUserModel()
        {
            return new UserModel
            (
                Id,
                FirstName,
                LastName,
                Phone,
                Email
            );
        }
    }
}