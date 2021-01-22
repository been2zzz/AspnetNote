using System.ComponentModel.DataAnnotations;

namespace AspnetNote.ViewModel
{
    public class LoginViewModel
    {   
        [Required(ErrorMessage = "ID를 입력하세요")]
        public string UserId { get; set; }
        
        [Required(ErrorMessage = "비밀번호를 입력하세요")]
        public string UserPassword { get; set; }
    }
}
