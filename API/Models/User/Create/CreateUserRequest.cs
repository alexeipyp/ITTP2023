using System.ComponentModel.DataAnnotations;

namespace API.Models.User.Create
{
    public class CreateUserRequest
    {
        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string Login { get; set; } = null!;

        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string Password { get; set; } = null!;

        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string Name { get; set; } = null!;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
    }
}
