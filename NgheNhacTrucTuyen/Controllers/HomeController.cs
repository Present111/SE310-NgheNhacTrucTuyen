using NgheNhacTrucTuyen.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
using System.Linq;
using System.Text;
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

                var normalizedSearchString = RemoveDiacritics(SearchString.ToLower());

                link = link.Where(s => RemoveDiacritics(s.TenBH.ToLower()).Contains(normalizedSearchString));
            }

            return View(link);
        }


        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

           
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }



        [HttpGet]
        public ActionResult Library()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Session["Email"] != null)
            {
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                ViewBag.Thuvien = context.PlayLists.Where(x => x.Matk == a.MaTK && x.TenPL != "").ToList().GroupBy(x => x.TenPL).Select(group => group.First());
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

        [HttpPost]
        public ActionResult ThemYeuThich(int id)
        {
            try
            {             
                DBcontextDataContext context = new DBcontextDataContext();
                if (Session["Email"] == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại." });
                }
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());             
                PlayList p = new PlayList
                {
                    Matk = a.MaTK,
                    MaBH = id,
                    TenPL = ""
                };
                context.PlayLists.InsertOnSubmit(p);
                context.SubmitChanges(); 
                return Json(new { success = true, message = "Thêm bài hát yêu thích thành công!" });
            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult XoaYeuThich(int id)
        {
            try
            {           
                DBcontextDataContext context = new DBcontextDataContext();
                if (Session["Email"] == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại." });
                }
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                List<PlayList> playlistItems = context.PlayLists.Where(x => x.Matk == a.MaTK && x.MaBH == id && x.TenPL == "").ToList(); 

                if (playlistItems.Any()) 
                {
                    context.PlayLists.DeleteAllOnSubmit(playlistItems); 
                    context.SubmitChanges(); 
                    return Json(new { success = true, message = "Xóa bài hát yêu thích thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy bài hát yêu thích để xóa." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpGet]
        public ActionResult BaiHat(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            
            if (Session["Email"] == null)
            {
                ViewBag.IsFavorited = false;
                ViewBag.playlists = null;
            }
            else
            {
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                ViewBag.playlists = context.PlayLists.Where(x => x.Matk == a.MaTK && x.TenPL != "").ToList().GroupBy(x => x.TenPL).Select(group => group.First());
                bool check = context.PlayLists.Any(x => x.MaBH == id && x.TenPL == "" && x.Matk == a.MaTK);
                ViewBag.IsFavorited = check;
            }
            Nhac n = context.Nhacs.FirstOrDefault(x => x.MaBH == id);
            ViewBag.baihat = n;
            var nhac = context.Nhacs.Where(x => x.MaCS == n.MaCS).ToList();
            ViewBag.list = nhac;
            return View();
        }

        [HttpGet]
        public ActionResult BaiHatYeuThich()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
            List<PlayList> PL = context.PlayLists.Where(x => x.Matk == a.MaTK && x.TenPL == "").ToList();
            ViewBag.playlist = PL;
            ViewBag.s = PL.Count();
            ViewBag.tk = a;
            return PartialView("BaiHatYeuThich");
        }

        [HttpGet]
        public ActionResult ThemPL()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var baihat = context.Nhacs.ToList();
            ViewBag.baihats = baihat;
            TempData["success"] = null;
            return PartialView("ThemPL");
        }

        [HttpPost]
        public ActionResult ThemPL(string tenPL, string selectedSongsList)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var baihat = context.Nhacs.ToList();
            ViewBag.baihats = baihat;
            account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());

            bool check = context.PlayLists.Where(x=>x.Matk==a.MaTK).Any(x => x.TenPL == tenPL);

            if (check == false)
            {
               
                PlayList p = new PlayList();
                p.Matk = a.MaTK;
                p.TenPL = tenPL;
                context.PlayLists.InsertOnSubmit(p);
                context.SubmitChanges();

                var selectedSongIds = new List<int>();
                if (string.IsNullOrEmpty(selectedSongsList)==false)
                {
                    selectedSongIds = selectedSongsList.Split(',').Select(int.Parse).ToList();
                }
               
                foreach (var songId in selectedSongIds)
                {
                    PlayList pl = new PlayList();
                    pl.Matk = a.MaTK;
                    pl.MaBH = songId;
                    pl.TenPL = tenPL;
                    context.PlayLists.InsertOnSubmit(pl);
                    context.SubmitChanges();
                }
                TempData["success"] = "Thêm PL thành công !";
            }
            else
            {
                TempData["success"] = "PL đã có trong danh sách";
            }

            return PartialView("ThemPL");
        }
    }
}