namespace Ms.Arch.Hw02.Api.Application.Models
{
    public class UserModel
    {
        public UserModel(
            long id, 
            string firstName, 
            string lastName, 
            string email, 
            string phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        public long Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Phone { get; }
    }
}