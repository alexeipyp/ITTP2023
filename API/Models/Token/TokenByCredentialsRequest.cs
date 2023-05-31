namespace API.Models.Token
{
    public class TokenByCredentialsRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
