using System.ComponentModel.DataAnnotations;

namespace ItransitionProject.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
