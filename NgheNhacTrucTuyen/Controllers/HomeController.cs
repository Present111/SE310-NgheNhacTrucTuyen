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
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            ViewBag.Chude = context.ChuDes.ToList();

            return PartialView("Search");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult TimKiem(string SearchString)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            List<Nhac> a = context.Nhacs.ToList();
            var link = from l in a select l;
            if (!string.IsNullOrEmpty(SearchString))
            {
                link = link.Where(s => s.TenBH.Contains(SearchString));
            }
            return View(link);
        }
    }
}