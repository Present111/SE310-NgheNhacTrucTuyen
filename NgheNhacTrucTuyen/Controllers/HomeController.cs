using NgheNhacTrucTuyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgheNhacTrucTuyen.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var theloai = context.TheLoais.ToList();
            ViewBag.test = theloai;
            var nhac = context.Nhacs.ToList();
            ViewBag.jointable = nhac;
            return View();
        }
        public ActionResult Index1()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var theloai = context.TheLoais.ToList();
            ViewBag.test = theloai;
            var nhac = context.Nhacs.ToList();
            ViewBag.jointable = nhac;
            return PartialView("Index1");
        }
    }
}