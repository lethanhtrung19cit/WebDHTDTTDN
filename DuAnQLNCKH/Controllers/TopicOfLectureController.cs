using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;
using Type = DuAnQLNCKH.Models.Type;

namespace DuAnQLNCKH.Controllers
{
    public class TopicOfLectureController : Controller
    {
        // GET: DeTaiGV
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfLectureModel dtgv = new TopicOfLectureModel();
        List<TopicOfStudent> studentList = new List<TopicOfStudent>();
        public ActionResult ListType()
        {
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
        public ActionResult getTypeList(string IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdTy == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            
            ViewBag.TopicOfStudents = dtgv.listStudentAll();
            var model = dtgv.listAll();
            return View(model);

        }

        public ActionResult myTopicLecture()
        {
            Session["UserName"] = "lecture";
            Session["Acess"] = "2";

            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<Information> information = db.Information.ToList();

                var topicextension = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      where i.UserName == Session["UserName"].ToString() 
                                      where t.Status=="chưa duyệt"                                     
                                      select new TopicOfLectureView
                                      {

                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicExtension = topicextension;
                var topic = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      where i.UserName == Session["UserName"].ToString()
                                      where t.Status == "đã duyệt"
                                      select new TopicOfLectureView
                                      {

                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicProgress = topic;
                return View();
            }
        }
        //To Handle connection related activities
        [HttpPost]
        public void ShowIdP(string id)
        {
            ViewBag.idp = id;
            
        }
        public ActionResult CreateTopicOfLecture(TopicOfLecture topicOfLecture)
        {
            
            if (ModelState.IsValid)
            {
                string id = dtgv.IdTp();

                TopicOfLectureModel topic = new TopicOfLectureModel();
                if (topic.AddTopicLecture(topicOfLecture, id))
                    ViewBag.Message = "Employee details added successfully";
                
            }
            ModelState.Clear();
            
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View("ViewCreateTopicOfLecture");
           
        }
       
        public ActionResult ViewCreateTopicOfLecture()
        {
            List<Type> typelist = qLNCKHDHTDTD.Types.ToList();
            ViewBag.listtype = new SelectList(typelist, "IdTy", "Name");
            return View();
        }
       

        [HttpPost]   
        public JsonResult Delete(string IdTp)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from TopicOfLecture where IdTp='" + IdTp + "'")>0;

            return Json(new {
                IdTp = IdTp,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult chuaduyet()
        {
            ViewBag.TopicOfStudentchuaduyet = dtgv.listchuaduyetsv();
                       
            var model = dtgv.listchuaduyet();
            return View(model);
         
        }
        public ActionResult registerExtension(Extension extension)
        {
            TopicOfLectureModel topicModel = new TopicOfLectureModel();
            topicModel.AddRegisterExtension(extension);
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<Information> information = db.Information.ToList();

                var topicextension = (from t in topicOfLectures
                                      join i in information on t.IdLe equals i.IdLe
                                      where i.UserName == Session["UserName"].ToString()
                                      where t.Status == "chưa duyệt"
                                      select new TopicOfLectureView
                                      {

                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicExtension = topicextension;
                var topic = (from t in topicOfLectures
                             join i in information on t.IdLe equals i.IdLe
                             where i.UserName == Session["UserName"].ToString()
                             where t.Status == "đã duyệt"
                             select new TopicOfLectureView
                             {

                                 topicOfLecture = t,
                                 information = i
                             }).ToList();
                ViewBag.topicProgress = topic;
                
            }
            return View("myTopicLecture");
        }
        public ActionResult viewRegisterExtension()
        {     
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                List<Information> information = db.Information.ToList();

                var topicextension = (from t in topicOfLectures 
                                     join i in information on t.IdLe equals i.IdLe
                                     where i.UserName== Session["UserName"].ToString()
                                     where t.Status=="đã duyệt"
                                      select new TopicOfLectureView
                                     {
                                         
                                         topicOfLecture = t,
                                         information=i
                                     }).ToList();
                return View(topicextension);
            }
        }
        public ActionResult listextension()
        {
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {
                List<Extension> extension = db.Extensions.ToList();
                
                List<TopicOfLecture> topicOfLectures = db.TopicOfLectures.ToList();

                var topicextension = from e in extension
                                     join t in topicOfLectures on e.IdTp equals t.IdTp into table
                                    
                                     from t in table.ToList()
                                     select new TopicOfLectureView
                                     {
                                         extension = e,
                                         topicOfLecture = t,
                                         
                                     };
                return View(topicextension);
            }
          

        }
        
        public ActionResult chuaduyetsv()
        {            
            
             var model = dtgv.listchuaduyetsv();
            return View(model);
        }
        [HttpPost]
        [Route("/TopicOfLecture/extension")]
        public void extension(int Times,string IdTp, int IdEx)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                
                entities.Database.ExecuteSqlCommand("update TopicOfLecture set Times="+Times+" where IdTp='"+IdTp+"' delete from extension where IdEx="+IdEx);
                entities.SaveChanges();
            }

        }
        [HttpPost]       
        public void xetduyet2(TopicOfLecture topicOfLecture)
        {
           

                using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
                {
                     TopicOfLecture topic = (from c in entities.TopicOfLectures
                                                where c.IdTp == topicOfLecture.IdTp
                                                select c).FirstOrDefault();
                     entities.Database.ExecuteSqlCommand("update TopicOfLecture set Status=N'đã duyệt', Progress=N'chờ báo cáo lần 1' where IdTp='" + topic.IdTp + "'");
                    entities.SaveChanges();
                }
 
        }
        [HttpPost]
        
        public void xetduyetsv(TopicOfStudent topicOfStudent)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                TopicOfStudent topic = (from c in entities.TopicOfStudents
                                        where c.IdTp == topicOfStudent.IdTp
                                        select c).FirstOrDefault();
                entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt' where IdTp='" + topic.IdTp + "'");
                entities.SaveChanges();
            }
        }
    }
}