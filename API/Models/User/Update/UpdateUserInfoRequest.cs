using System.ComponentModel.DataAnnotations;

namespace API.Models.User.Update
{
    public class UpdateUserInfoRequest : UpdateUserRequest
    {
        [RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")]
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int Gender { get; set; } = (int)Common.Enums.Gender.Unknown;
    }
}
