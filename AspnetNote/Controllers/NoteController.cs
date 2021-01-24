using AspnetNote.DataContext;
using AspnetNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetNote.Views.Shared
{
    public class NoteController : Controller
    {
        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            using (var db = new AspnetNoteDbContext())
            {
                // 전체
                var list = db.Notes.ToList(); 
                return View(list);
            }
        }

        // 게시물 상세
        public IActionResult Detail(int noteNo)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            using (var db = new AspnetNoteDbContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                return View(note);
            }
        }
        // 게시글 추가
        public IActionResult Add()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Add(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            model.UserNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());
            if (ModelState.IsValid)
            {
                using (var db = new AspnetNoteDbContext())
                {
                    db.Notes.Add(model);
                    
                    if(db.SaveChanges() > 0)
                    {
                        // 동일한 컨트롤러 Index로 넘기는 것
                        return Redirect("Index"); 
                    }
                    //db.SaveChanges(); //COMMIT
                }
                ModelState.AddModelError(string.Empty, "저장 실패");
            }
            return View(model);
        }
        // 게시글 수정
        public IActionResult Edit(int noteNo)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            using (var db = new AspnetNoteDbContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                return View(note);
            }
        }

        public IActionResult EditCommit(Note model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AspnetNoteDbContext())
                {
                    var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(model.NoteNo));
                    note.NoteTitle = model.NoteTitle;
                    note.NoteContents = model.NoteContents;
                    if (db.SaveChanges() > 0)
                    {
                        // 동일한 컨트롤러 Index로 넘기는 것
                        return Redirect("Index");
                    }
                    //db.SaveChanges(); //COMMIT
                }
                ModelState.AddModelError(string.Empty, "저장 실패");
            }
            return View(model);
        }
        
        // 게시글 삭제
        public IActionResult Delete(int noteNo)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 비로그인
                return RedirectToAction("Login", "Account");
            }
            using (var db = new AspnetNoteDbContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                db.Notes.Remove(note);  // 삭제
                if (db.SaveChanges() > 0)
                {
                    // 동일한 컨트롤러 Index로 넘기는 것
                    return Redirect("Index");
                }
            }
            return View();
        }
    }
}
