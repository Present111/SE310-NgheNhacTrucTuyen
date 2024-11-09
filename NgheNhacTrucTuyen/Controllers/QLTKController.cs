using NgheNhacTrucTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class QLTKController : Controller
    {
        public ActionResult DSTaiKhoan()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var a = context.accounts.ToList();
            ViewBag.taikhoan = a;
            return PartialView("DSTaiKhoan");
        }

        public ActionResult EditTK(int id)

        {
            DBcontextDataContext context = new DBcontextDataContext();
            account a = context.accounts.FirstOrDefault(x => x.MaTK == id);
            if (Request.Form.Count == 0)
            {
                return View(a);
            }
            a.Email = Request.Form["Email"];
            a.PassWord = Request.Form["PassWord"];
            a.Role = int.Parse(Request.Form["Role"]);
            a.Ten = Request.Form["Ten"];
            context.SubmitChanges();
            ViewBag.ok = "Chỉnh sửa tài khoản thành công!";
            return View();

        }
        public ActionResult CreateTK()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Request.Form.Count > 0)
            {
                account a = new account();
                a.Email = Request.Form["Email"];
                a.PassWord = Request.Form["PassWord"];
                a.Role = int.Parse(Request.Form["Role"]);
                a.Ten = Request.Form["Ten"];

                bool check = context.accounts.Any(x => x.Email == a.Email);
                if (check == false)
                {
                    context.accounts.InsertOnSubmit(a);
                    context.SubmitChanges();
                    ViewBag.ok = "Thêm tài khoản thành công!";
                    return View();
                }
                else
                {
                    ViewBag.sucess = "Email đã tồn tại";
                    return View();
                }
            }

            return View();
        }
        public ActionResult DeleteTK(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var data = context.accounts.FirstOrDefault(x => x.MaTK == id);
            if (data != null)
            {
                context.accounts.DeleteOnSubmit(data);
                context.SubmitChanges();
                return RedirectToAction("DSTaiKhoan", "QLTK");
            }
            return RedirectToAction("DSTaiKhoan", "QLTK");
        }

    }
}