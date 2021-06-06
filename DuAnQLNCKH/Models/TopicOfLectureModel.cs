﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Models
{
    public class TopicOfLectureModel
    {

        //string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        private SqlConnection con;
        public virtual string IdTy { get; set; }
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        public string IdTp()
        {
            connection();
            con.Open();
            string sql = string.Format("declare cur_IdTp cursor for select count(IdTp) from TopicOfLecture open cur_IdTp declare @count int fetch next from cur_IdTp into @count if @count=0 begin insert into TopicOfLecture(IdTp) values ('1') select IdTp='DTGV01' from TopicOfLecture ;end; else begin select IdTp='DTGV0'+CAST(@count+1 as varchar(10)) from TopicOfLecture ;fetch next from cur_IdTp into @count ;end; close cur_IdTp deallocate cur_IdTp");
            SqlCommand a = new SqlCommand(sql, con);
            String a1 = (String)a.ExecuteScalar();
            con.Close();
            return a1;
        }
      
        public SelectList getType1()
        {
            IEnumerable<SelectListItem> typeList = (from m in qLNCKHDHTDTD.Types select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.Name, Value = m.IdTy.ToString() });
            return new SelectList(typeList, "Value", "Text", IdTy);

        }
        public SelectList getDetailType()
        {
            IEnumerable<SelectListItem> detailtypeList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(IdTy))
            {

                detailtypeList = (from m in qLNCKHDHTDTD.PointTables where m.IdTy == IdTy select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.NameP, Value = m.IdP.ToString() });
            }
            return new SelectList(detailtypeList, "Value", "Text", IdTy);

        }
        public bool AddTopicLecture(TopicOfLecture topicOfLecture, string IdTp)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewTopicOfLecture", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdTp", IdTp);
            com.Parameters.AddWithValue("@Name", topicOfLecture.Name);
            com.Parameters.AddWithValue("@IdLe", topicOfLecture.IdLe);
            com.Parameters.AddWithValue("@IdP", topicOfLecture.IdP);
            com.Parameters.AddWithValue("@DateSt", topicOfLecture.DateSt);
            com.Parameters.AddWithValue("@Times", topicOfLecture.Times);
            com.Parameters.AddWithValue("@Expense", topicOfLecture.Expense);
            com.Parameters.AddWithValue("@Status", "chưa duyệt");
            com.Parameters.AddWithValue("@Progress", "chờ duyệt");
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }
        }
        public bool AddRegisterExtension(Extension extension)
        {

            connection();
            SqlCommand com = new SqlCommand("AddRegisterExtension", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdTp", extension.IdTp);
            com.Parameters.AddWithValue("@Times", extension.Times);
            
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }
        }
        private DHTDTTDNEntities1 qLNCKHDHTDTD = null;
        public TopicOfLectureModel()
        {
            qLNCKHDHTDTD = new DHTDTTDNEntities1();
        }
       
        public List<TopicOfLecture> listchuaduyet()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfLecture>("select * from TopicOfLecture where Status like N'chưa duyệt'").ToList();
            return list;
        }
        public List<Type> listType()
        {
            return qLNCKHDHTDTD.Types.ToList();
            //var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfLecture>("select * from TopicOfLecture").ToList();
            //return list;
        }
        
       
        public List<TopicOfStudent> listchuaduyetsv()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfStudent>("select * from TopicOfStudent where Status like N'chưa duyệt'").ToList();
            return list;
        }
        public List<TopicOfStudent> listStudentAll()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfStudent>("select * from TopicOfStudent where Status like N'đã duyệt'").ToList();
            return list;
        }
        public List<TopicOfLecture> listAll()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfLecture>("select * from TopicOfLecture where Status like N'đã duyệt'").ToList();
            return list;
        }
        //public DeTaiGV GetByMaDT(string maDT)
        //{
        //    qLNCKHDHTDTD.DeTaiGVs.SingleOrDefault.
        //}
        public TopicOfLecture ViewDetail(string IdTp)
        {
            return qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
        }
        public bool xoa(string IdTp)
        {
            try
            { 
                var topic = qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
                qLNCKHDHTDTD.TopicOfLectures.Remove(topic);

                qLNCKHDHTDTD.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
          
        }
        public bool xetduyet1(string IdTp)
        {
            try
            {


                //detai.TrangThai = dtgv1.TrangThai;                 
                qLNCKHDHTDTD.Database.ExecuteSqlCommand("update TopicOfLecture set Status like N'đã duyệt' where IdTp=@IdTp",
                     new SqlParameter("@IdTp", IdTp)
                    );

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
       
        //public int xetduyet(int ID)
        //{
        //    int i;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("DeleteEmployee", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@Id", ID);
        //        i = com.ExecuteNonQuery();
        //    }
        //    return i;
        //}
    }
}