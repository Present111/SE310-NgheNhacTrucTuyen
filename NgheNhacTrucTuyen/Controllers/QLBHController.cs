using System;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NgheNhacTrucTuyen.Models;


namespace NgheNhacTrucTuyen.Controllers
{
    public class QLBHController : Controller
    {
        


        [HttpGet]
        public ActionResult DSBaiHat()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var baihat = context.Nhacs.OrderBy(x => x.TenBH).ToList();
            ViewBag.baihats = baihat;
            ViewBag.Casis = context.CaSis.ToList();
            ViewBag.theloais = context.TheLoais.ToList();
            ViewBag.chudes = context.ChuDes.ToList();
            return View();
        }

       
        [HttpGet]
        public ActionResult Create()
        {
            DBcontextDataContext context = new DBcontextDataContext();
            ViewBag.Casis = new SelectList(context.CaSis, "MaCS", "TenCS");
            ViewBag.Theloais = new SelectList(context.TheLoais, "MaTL", "TenTL");
            ViewBag.Chudes = new SelectList(context.ChuDes, "MaCD", "TenCD");
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase filenhac, HttpPostedFileBase fileanh)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Request.Form.Count > 0)
            {
                Nhac p = new Nhac();
                p.TenBH = Request.Form["TenBH"];
                p.NgayPH = DateTime.ParseExact(Request.Form["NgayPH"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                p.MaCS = int.Parse(Request.Form["MaCS"]);
                p.MaTL = int.Parse(Request.Form["MaTL"]);
                p.MaCD = int.Parse(Request.Form["MaCD"]);

                if (filenhac != null && filenhac.FileName != "")
                {
                    string _FileName = Path.GetFileName(filenhac.FileName);
                    string _path = Path.Combine(Server.MapPath("/Sound"), _FileName);
                    filenhac.SaveAs(_path);
                    p.Files = filenhac.FileName;
                }
                if (fileanh != null && fileanh.FileName != "")
                {
                    string _FileName1 = Path.GetFileName(fileanh.FileName);
                    string _path1 = Path.Combine(Server.MapPath("/image"), _FileName1);
                    fileanh.SaveAs(_path1);
                    p.image = fileanh.FileName;
                }
                context.Nhacs.InsertOnSubmit(p);
                context.SubmitChanges();
                return RedirectToAction("DSBaiHat", "QLBH");

            }

            return View();
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            Nhac n = context.Nhacs.FirstOrDefault(x => x.MaBH == id);

            if (Request.Form.Count == 0)
            {
                ViewBag.nhac = n;
                ViewBag.Casis = new SelectList(context.CaSis, "MaCS", "TenCS");
                ViewBag.Theloais = new SelectList(context.TheLoais, "MaTL", "TenTL");
                ViewBag.Chudes = new SelectList(context.ChuDes, "MaCD", "TenCD");
                return View(n);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase filenhac, HttpPostedFileBase fileanh)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            Nhac n = context.Nhacs.FirstOrDefault(x => x.MaBH == id);

            if (Request.Form.Count == 0)
            {
                return View(n);
            }
            n.TenBH = Request.Form["TenBH"];
            if (!string.IsNullOrEmpty(Request.Form["NgayPH"]))
            {
                n.NgayPH = DateTime.ParseExact(Request.Form["NgayPH"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            n.MaCS = int.Parse(Request.Form["MaCS"]);
            n.MaTL = int.Parse(Request.Form["MaTL"]);
            n.MaCD = int.Parse(Request.Form["MaCD"]);

            if (filenhac != null && filenhac.FileName != "")
            {
                string _FileName = Path.GetFileName(filenhac.FileName);
                string _path = Path.Combine(Server.MapPath("/Sound"), _FileName);
                filenhac.SaveAs(_path);
                n.Files = filenhac.FileName;
            }
            if (fileanh != null && fileanh.FileName != "")
            {
                string _FileName1 = Path.GetFileName(fileanh.FileName);
                string _path1 = Path.Combine(Server.MapPath("/image"), _FileName1);
                fileanh.SaveAs(_path1);
                n.image = fileanh.FileName;
            }

            context.SubmitChanges();
            return RedirectToAction("DSBaiHat", "QLBH");
        }

        public ActionResult Delete(int id)
        {

            DBcontextDataContext context = new DBcontextDataContext();
            var data = context.Nhacs.FirstOrDefault(x => x.MaBH == id);
            if (data != null)
            {
                context.Nhacs.DeleteOnSubmit(data);
                context.SubmitChanges();
                return RedirectToAction("DSBaiHat");
            }

            return View("DSBaiHat");
        }


        



        public ActionResult Detail(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            Nhac a = context.Nhacs.FirstOrDefault(x => x.MaBH == id);
            ViewBag.detail = a;
            return View();
        }



        [HttpPost]
        public ActionResult ThemvaoPL(int id, string TenPL)
        {
            if (Session["Email"] != null)
            {
                DBcontextDataContext context = new DBcontextDataContext();
                account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
                bool check = context.PlayLists.Any(x => x.TenPL == TenPL && x.Matk == a.MaTK);
                if (check)
                {
                    bool kiemtra = context.PlayLists.Any(x => x.TenPL == TenPL && x.Matk == a.MaTK && x.MaBH == id);
                    if (kiemtra == false)
                    {
                        PlayList p = new PlayList();
                        p.Matk = a.MaTK;
                        p.MaBH = id;
                        p.TenPL = TenPL;
                        context.PlayLists.InsertOnSubmit(p);
                        context.SubmitChanges();
                        ViewBag.ok1 = "Thêm thành công";
                    }
                }
            }
            return RedirectToAction("Baihat", "Home", new { id = id });
        }

        [HttpPost]
        public ActionResult XoakhoiPL(int id, string TenPL)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Session["Email"] == null)
            {
                return RedirectToAction("Playlist", "Home", new { id = TenPL });
            }
            account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
            List<PlayList> playlistItems = context.PlayLists.Where(x => x.Matk == a.MaTK && x.MaBH == id && x.TenPL == TenPL).ToList();
            if (playlistItems.Any())
            {
                context.PlayLists.DeleteAllOnSubmit(playlistItems);
                context.SubmitChanges();
                return RedirectToAction("Playlist", "Home", new { tenPL = TenPL });
            }
            else
            {
                return RedirectToAction("Playlist", "Home", new { tenPL = TenPL });
            }
        }

        [HttpPost]
        public ActionResult XoakhoiYT(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Session["Email"] == null)
            {
                return RedirectToAction("BaiHatYeuThich", "Home");
            }
            account a = context.accounts.FirstOrDefault(x => x.Email == Session["Email"].ToString());
            List<PlayList> playlistItems = context.PlayLists.Where(x => x.Matk == a.MaTK && x.MaBH == id && x.TenPL == "").ToList();
            if (playlistItems.Any())
            {
                context.PlayLists.DeleteAllOnSubmit(playlistItems);
                context.SubmitChanges();
                return RedirectToAction("BaiHatYeuThich", "Home");
            }
            else
            {
                return RedirectToAction("BaiHatYeuThich", "Home");
            }
        }

    }
}