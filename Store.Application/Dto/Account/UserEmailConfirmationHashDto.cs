namespace Store.Application.Dto.Account
{
    public class UserEmailConfirmationHashDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ConfirmationHash { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
