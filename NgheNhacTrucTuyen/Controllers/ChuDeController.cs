using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NgheNhacTrucTuyen.Models;

namespace NgheNhacTrucTuyen.Controllers
{   
    public class ChuDeController : Controller
    {
        // GET: Chude
        public ActionResult CreateCD(HttpPostedFileBase fileanh)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            if (Request.Form.Count > 0)
            {
                ChuDe cd = new ChuDe();
                cd.TenCD = Request.Form["TenCD"];
                cd.Color = Request.Form["Color"];
                if (fileanh != null && fileanh.FileName != "")
                {
                    string _FileName1 = Path.GetFileName(fileanh.FileName);
                    string _path1 = Path.Combine(Server.MapPath("/image"), _FileName1);
                    fileanh.SaveAs(_path1);
                    cd.Picture = fileanh.FileName;
                }
                bool check = context.ChuDes.Any(x => x.TenCD == cd.TenCD);
                if (check == false)
                {
                    context.ChuDes.InsertOnSubmit(cd);
                    context.SubmitChanges();
                    ViewBag.ok = "Thêm chủ đề thành công!";
                    return View();
                }
                else
                {
                    ViewBag.sucess = "Chủ đề đã có trong danh sách";
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult EditCD(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            ChuDe cd = context.ChuDes.FirstOrDefault(x => x.MaCD == id);
            ViewBag.chude = cd;
            ViewBag.anhchude = cd.Picture;
            if (Request.Form.Count == 0)
            {
                return View(cd);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditCD(int id, HttpPostedFileBase fileanh)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            ChuDe cd = context.ChuDes.FirstOrDefault(x => x.MaCD == id);
            if (Request.Form.Count == 0)
            {
                return View(cd);
            }
            cd.MaCD = int.Parse(Request.Form["MaCD"]);
            cd.TenCD = Request.Form["TenCD"];
            cd.Color = Request.Form["Color"];
            if (fileanh != null && fileanh.FileName != "")
            {
                string _FileName1 = Path.GetFileName(fileanh.FileName);
                string _path1 = Path.Combine(Server.MapPath("/image"), _FileName1);
                fileanh.SaveAs(_path1);
                cd.Picture = fileanh.FileName;
            }
            context.SubmitChanges();
            ViewBag.ok = "Chỉnh sửa chủ đề thành công!";
            return View();
            
        }


        public ActionResult DeleteCD(int id)
        {
            DBcontextDataContext context = new DBcontextDataContext();
            var data = context.ChuDes.FirstOrDefault(x => x.MaCD == id);
            if (data != null)
            {
                context.ChuDes.DeleteOnSubmit(data);
                context.SubmitChanges();
                return RedirectToAction("DSBaiHat", "QLBH");
            }
            return RedirectToAction("DSBaiHat", "QLBH");
        }
    }
}