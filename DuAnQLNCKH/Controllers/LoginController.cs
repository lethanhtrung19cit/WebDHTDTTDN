using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;
namespace DuAnQLNCKH.Controllers
{
    public class LoginController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        // GET: Login
        public ActionResult Index()
        {
            Session["Acess"] = "null";
            Session["UserName"] = "null";
            List<Account> acesslist = qLNCKHDHTDTD.Accounts.ToList();
            ViewBag.listacess = new SelectList(acesslist, "Access", "Access");
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login1(Account account)
        {
            if (ModelState.IsValid)
            {
                using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
                {
                    var obj = db.Accounts.Where(a => a.UserName.Equals(account.UserName) && a.PassWord.Equals(account.PassWord) && a.Access.Equals(account.Access)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Acess"] = obj.Access.ToString();
                        Session["UserName"] =  obj.UserName.ToString();
                        return RedirectToAction("Index","TopicOfLecture");
                    }
                }
            }
            return View();
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}