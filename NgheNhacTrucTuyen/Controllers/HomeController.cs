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
        

        [HttpGet]
        public ActionResult Index()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var theloai = context.TheLoais.ToList();
            ViewBag.test = theloai;
            var nhac = context.Nhacs.ToList();
            ViewBag.jointable = nhac;
            return View();
        }

        
        [HttpGet]
        public ActionResult Index1()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var theloai = context.TheLoais.ToList();
            ViewBag.test = theloai;
            var nhac = context.Nhacs.ToList();
            ViewBag.jointable = nhac;
            return PartialView("Index1");
        }



      
        [HttpGet]
        public ActionResult Search()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            ViewBag.Chude = context.ChuDes.ToList();

            return PartialView("Search");
        }


      
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


      
        [HttpGet]
        public ActionResult Library()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Session["Email"] != null)
            {
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                ViewBag.Thuvien = context.PlayLists.Where(x => x.Matk == a.MaTK).ToList().GroupBy(x => x.TenPL).Select(group => group.First());
                return PartialView("Library");
                
            }
            ViewBag.Thuvien = new List<PlayList>(); 
            ViewBag.ErrorMessage = "Bạn phải đăng nhập để xem thư viện.";
            return View();
        }

        [HttpGet]
        public ActionResult Playlist(string tenPL)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Session["Email"] != null)
            {
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                List<PlayList> PL = context.PlayLists.Where(x => x.Matk == a.MaTK && x.TenPL == tenPL).ToList();
                ViewBag.playlist = PL;
                ViewBag.s = PL.Count() - 1;
                ViewBag.tk = a;
                ViewBag.tkten = a.Ten;
                ViewBag.ten = tenPL;
                return View();

            }
            ViewBag.playlist=new List<PlayList>();
            ViewBag.ErrorMessage = "Bạn phải đăng nhập để xem thư viện.";
            return View();
        }


    }
}