using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class NotificationController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        NotificationModel notify = new NotificationModel();
        public ActionResult Index(FileNotifiModel model)
        {
            Session["Acess"] = "null";
            Session["UserName"] = "null";
            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName=c.FileName
                         }).ToList();
            //var list1 = demo.Database.SqlQuery<tblFileDetail>("select * from tblFileDetails").ToList();
           
            model.ListFile = list1;
            ViewBag.listNoti = list1;
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).ToList();


            return View();
        }

        [HttpPost]
        public ActionResult CreateNotifi(HttpPostedFileBase files, FileNotifiModel model)
        {

            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();

            model.ListFile = list1;
            ViewBag.FileList = list1;

            if (files != null)
            {
                var Extension = Path.GetExtension(files.FileName);
                var fileName = "filenotifi-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                string path = Path.Combine(Server.MapPath("~/FileNotification"), fileName);
                model.FileName = Url.Content(Path.Combine("~/FileNotification/", fileName));
                NotificationModel notifi = new NotificationModel();

                if (notifi.AddNoTification(model, Session["UserName"].ToString()))
                {
                    files.SaveAs(path);
                    ViewBag.Message = "Employee details added successfully";
                }
                else
                {
                    ModelState.AddModelError("", "Error In Add File. Please Try Again !!!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Choose Correct File Type !!");

            }


         
            return RedirectToAction("Index", "Notification");


        }

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult DetailNotification(string IdNo)
        {

            var model = notify.detailNotification(IdNo);
            return View(model);

        }
        // hien thi thong bao va chi tiet thong bao
   
        public ActionResult DetailNo(long id)
        {
            // return connect.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Detail = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList(); 
            return View();
        }
      
    }
}