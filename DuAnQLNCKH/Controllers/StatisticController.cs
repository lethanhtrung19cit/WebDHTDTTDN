using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuAnQLNCKH.Models;


namespace DuAnQLNCKH.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        DHTDTTDNEntities1 dHTDTTDNEntities1 = new DHTDTTDNEntities1();
        public ActionResult Index()
        {
            Session["UserName"] = "pnckh";
            Session["Acess"] = "1";
            viewbag();
           
            return View();
        }
        public void viewbag()
        {
            List<TopicOfLecture> listTopicOfLecture = dHTDTTDNEntities1.TopicOfLectures.ToList();
            List<TopicOfStudent> listTopicOfStudent = dHTDTTDNEntities1.TopicOfStudents.ToList();
            ViewBag.listTopicOfStudent = listTopicOfStudent;
            ViewBag.listTopicOfLecture = listTopicOfLecture;
            List<Models.Type> typelist = dHTDTTDNEntities1.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
        }
        public ActionResult getTypeList(string IdTy)
        {
            dHTDTTDNEntities1.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = dHTDTTDNEntities1.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchLecture(string name)
        {
            List<TopicOfLecture> listTopicOfLecture = dHTDTTDNEntities1.TopicOfLectures.Where(x=>x.Name.Contains(name)).ToList();
            List<TopicOfStudent> listTopicOfStudent = dHTDTTDNEntities1.TopicOfStudents.ToList();
            ViewBag.listTopicOfStudent = listTopicOfStudent;
            ViewBag.listTopicOfLecture = listTopicOfLecture;
            return View("Index");
        }
        [HttpGet]
        public JsonResult LoadData(string name)
        {
            IQueryable<TopicOfLecture> model = dHTDTTDNEntities1.TopicOfLectures;

            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.Name.Contains(name));
 
            int totalRow = model.Count();
            return Json(new
            {
                data = model,
                total = totalRow
                
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Journal()
        {
            List<Models.Type> typelist = dHTDTTDNEntities1.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
        private SqlConnection con;
        public string IdP1;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        [HttpPost]
        public ActionResult ExportExcel(string IdPa, DateTime dateEnd, DateTime dateSt)
        {
            var gv = new GridView();
            //ViewBag.IdP = "22";
            //string dateEnd1 = dateEnd.ToString("dd/MM/yyyy");
            //string dateSt1 = dateSt.ToString("dd/MM/yyyy");
            
            string daSt = dateSt.Date.ToString();
            
            string daEn = dateEnd.Date.ToString();
            //DateTime dateSt1 = new DateTime(dateSt);
            int a1 = int.Parse(IdPa);

            gv.DataSource = dHTDTTDNEntities1.TopicOfLectures.Where(a => a.IdP == a1 && a.DateSt >= dateSt && a.DateSt<= dateEnd).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            viewbag();
            return View("Index");
        }
        
        [HttpPost]
        
        public JsonResult ExportToExcel(string IdPa, DateTime dateEnd, DateTime dateSt)
        {
            ViewBag.IdP = IdPa;
            IdP1 = IdPa;
            viewbag();
            return Json("Index");
        }
        public string idp()
        {

            return ViewBag.IdP;
        }
    }
}