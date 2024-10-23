using NgheNhacTrucTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NgheNhacTrucTuyen.Controllers
{
    public class loginController : Controller
    {
        // GET: login

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
    }
}