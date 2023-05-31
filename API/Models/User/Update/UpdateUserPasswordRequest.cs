using System.ComponentModel.DataAnnotations;

namespace API.Models.User.Update
{
    public class UpdateUserPasswordRequest : UpdateUserRequest
    {
        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string Password { get; set; } = null!;
    }
}
