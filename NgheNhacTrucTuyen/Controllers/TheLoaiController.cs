using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NgheNhacTrucTuyen.Models;

namespace WebApplication1.Controllers
{   
    public class TheLoaiController : Controller
    {
        // GET: Theloai
        public ActionResult CreateTL()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Request.Form.Count > 0)
            {
                TheLoai tl = new TheLoai();
                tl.TenTL = Request.Form["TenTL"];
                bool check = context.TheLoais.Any(x => x.TenTL == tl.TenTL);
                if (check == false)
                {
                    context.TheLoais.InsertOnSubmit(tl);
                    context.SubmitChanges();
                    ViewBag.sucess = "Thêm thể loại thành công !";
                    return View();
                }
                else
                {
                    ViewBag.sucess = "Thể loại đã có trong danh sách";
                    return View();
                }
            }

            return View();
        }

    
        public ActionResult EditTL(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            TheLoai tl = context.TheLoais.FirstOrDefault(x => x.MaTL == id);
            if (Request.Form.Count == 0)
            {
                return View(tl);
            }
            tl.MaTL = int.Parse(Request.Form["MaTL"]);
            tl.TenTL = Request.Form["TenTL"];
            context.SubmitChanges();
            ViewBag.ok = "Chỉnh sửa thể loại thành công!";
            return View();
        }
        public ActionResult DeleteTL(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
           var data = context.TheLoais.FirstOrDefault(x => x.MaTL == id);
            if (data != null)
            {
                context.TheLoais.DeleteOnSubmit(data);
                context.SubmitChanges();
                return RedirectToAction("DSBaiHat", "QLBH");
            }
            return RedirectToAction("DSBaiHat", "QLBH");
        }
    }
}