using NgheNhacTrucTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NgheNhacTrucTuyen.Models;

namespace NgheNhacTrucTuyen.Controllers
{
    public class loginController : Controller
    {
        public static object Enconding { get; private set; }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(string email, string password)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            bool data = context.accounts.Any(x => x.Email == email && x.PassWord == password);
            account a = context.accounts.FirstOrDefault(x => x.Email == email && x.PassWord == password);
            if (data)
            {
                FormsAuthentication.SetAuthCookie(a.Ten, false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string email, string psw, string ten)
        {

            if (ModelState.IsValid)
            {
                DBcontextDataContext context = new DBcontextDataContext();
                account a = new account();
                a.Email = Request.Form["email"];
                a.PassWord = Request.Form["psw"];
                a.Ten = Request.Form["ten"];
                a.Role = int.Parse("0");
                var check = context.accounts.FirstOrDefault(s => s.Email == email);
                if (check == null)
                {
                    context.accounts.InsertOnSubmit(a);
                    context.SubmitChanges();
                    ViewBag.Sucess = "Đăng ký thành công";

                }
                else
                {
                    ViewBag.erro = "Email đã tồn tại";
                    return View();
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("Login");
        }

    }
}