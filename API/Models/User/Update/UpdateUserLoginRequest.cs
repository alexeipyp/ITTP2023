using System.ComponentModel.DataAnnotations;

namespace API.Models.User.Update
{
    public class UpdateUserLoginRequest : UpdateUserRequest
    {
        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string Login { get; set; } = null!;
    }
}
