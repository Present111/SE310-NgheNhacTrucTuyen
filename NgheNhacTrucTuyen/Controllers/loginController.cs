using NgheNhacTrucTuyen.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace NgheNhacTrucTuyen.Controllers
{
    public class loginController : Controller
    {
       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string emai, string password)
        {
            DBcontextDataContext db = new DBcontextDataContext();
            var adminUser = db.accounts.FirstOrDefault(x => x.Email == emai && x.PassWord == password && x.Role == 0);
            if (adminUser != null)
            {
                Session["islogin"] = true;
                Session["adm"] = false;
                Session["Ten"] = adminUser.Ten;
                Session["Email"] = adminUser.Email;
                return RedirectToAction("Index", "Home");


            }

          
            var regularUser = db.accounts.FirstOrDefault(x => x.Email == emai && x.PassWord == password && x.Role == 1);
            if (regularUser != null)
            {
                Session["islogin"] = true;
                Session["adm"] = true;
                Session["Ten"] = regularUser.Ten;
                Session["Email"] = regularUser.Email;
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
            DBcontextDataContext context = new DBcontextDataContext();
            var check = context.accounts.FirstOrDefault(s => s.Email == email);
            if (check == null)
            {
                account newAccount = new account
                {
                    Email = email,
                    PassWord = psw,
                    Ten = ten,
                    Role = 0 
                };
                context.accounts.InsertOnSubmit(newAccount);
                context.SubmitChanges();
                ViewBag.Success = "Đăng ký thành công";
            }
            else
            {
                ViewBag.erro = "Email đã tồn tại";
            }
            return View();
        }


        public ActionResult Logout()
        {    
            FormsAuthentication.SignOut();
            Session["islogin"] = false;
            Session["adm"] = false;
            Session["Ten"] = null;
            Session["Email"] = null;
            return RedirectToAction("Login");
        }


    }
}
