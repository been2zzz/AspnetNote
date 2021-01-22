using AspnetNote.DataContext;
using AspnetNote.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspnetNote.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid) // 입력값 입력했는지 아는 함수(User required로 앎)
            {
                using (var db = new AspnetNoteDbContext())
                {
                    db.Users.Add(model); //memory에 올림
                    db.SaveChanges(); //실제 db 저장. COMMIT!
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
