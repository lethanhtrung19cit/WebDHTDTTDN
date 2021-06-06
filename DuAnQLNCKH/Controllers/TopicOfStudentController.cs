using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;

namespace DuAnQLNCKH.Controllers
{
    public class TopicOfStudentController : Controller
    {
        // GET: DeTaiGV
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfStudentModel dtsv = new TopicOfStudentModel();
        public ActionResult Index()
        {

            var model = qLNCKHDHTDTD.TopicOfStudents.Where(s => s.Status.ToString().Contains("đã duyệt")).ToList();
            return View(model);

        }
        public ActionResult Register(TopicOfStudent topicOfStudent)
        {
            if (ModelState.IsValid)
            {
                string id = dtsv.IdTp();

                TopicOfStudentModel topic = new TopicOfStudentModel();
                if (topic.AddTopicStudent(topicOfStudent, id))
                    ViewBag.Message = "Employee details added successfully";

            }
            ModelState.Clear();

            List<Models.Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View("viewRegister");
        }
      
        public ActionResult getTypeList(string IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult viewRegister()
        {
            List<Models.Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
       
        [HttpPost]
        public JsonResult Delete(string IdTp)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from TopicOfStudent where IdTp='" + IdTp + "'") > 0;

            return Json(new
            {
                IdTp = IdTp,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult chuaduyet()
        {
            var detai = new TopicOfStudentModel();
            var model = detai.listchuaduyet();
            return View(model);
        }


        [HttpPost]
        [Route("/TopicOfStudent/xetduyet2")]
        public void xetduyet2(TopicOfStudent TopicOfStudent)
        {


            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                TopicOfStudent topic = (from c in entities.TopicOfStudents
                                        where c.IdTp == TopicOfStudent.IdTp
                                        select c).FirstOrDefault();
                entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt' where IdTp='" + topic.IdTp + "'");
                entities.SaveChanges();
            }

        }       
    }
}