using AspnetNote.DataContext;
using AspnetNote.Models;
using AspnetNote.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AspnetNoteDbContext())
                {
                    // Linq - 메서드 체이닝
                    // => : A GO TO B
                    //var user = db.Users
                    //    .FirstOrDefault(u => u.UserId == model.UserId && u.UserPassword == model.UserPassword);
                    var user = db.Users
                            .FirstOrDefault(u => u.UserId.Equals(model.UserId) 
                            && u.UserPassword.Equals(model.UserPassword));
                    if (user != null)
                    {
                        // 로그인 성공
                        //HttpContext.Session.SetInt32(key, value);
                        HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                        return RedirectToAction("LoginSuccess", "Home");
                    }
                }
                // 로그인 실패
                ModelState.AddModelError(string.Empty, "ID 혹은 비밀번호가 올바르지 않습니다.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER_LOGIN_KEY");

            // 전체 Session CLEAR!
            // HttpContext.Session.Clear();
            
            // 다른 컨트롤러로 넘길 때 ToAction
            return RedirectToAction("Index", "Home");
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
