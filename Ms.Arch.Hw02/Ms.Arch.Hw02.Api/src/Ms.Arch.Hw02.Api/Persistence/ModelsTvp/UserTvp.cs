using Ms.Arch.Hw02.Api.Application.Models;
using NpgsqlTypes;

namespace Ms.Arch.Hw02.Api.Persistence.ModelsTvp
{
    public class UserTvp
    {
        [PgName("id")]
        public long Id { get; set; }
        [PgName("firstname")]
        public string FirstName { get; set; }
        [PgName("lastname")]
        public string LastName { get; set; }
        [PgName("email")]
        public string Email { get; set; }
        [PgName("phone")]
        public string Phone { get; set; }

        public UserModel ConvertToUserModel()
        {
            return new UserModel(Id, FirstName, LastName, Email, Phone);
        }
    }
}