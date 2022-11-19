using ContactApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactApp.Controllers
{
    public class ContactController : Controller
    {
        private static string connection = ConfigurationManager.ConnectionStrings["string"].ToString();

        private static List<Contact> olist = new List<Contact>();

        // GET: Contact
        public ActionResult Home()
        {
            olist = new List<Contact>();

            using (SqlConnection oconnection = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACT", oconnection);
                cmd.CommandType = CommandType.Text;
                oconnection.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contact newContact = new Contact();

                        newContact.IdContact = Convert.ToInt32(dr["IdContact"]);    
                        newContact.Name = dr["Name"].ToString();
                        newContact.LastName = dr["LastName"].ToString();
                        newContact.Phone = dr["Phone"].ToString();
                        newContact.Email = dr["Email"].ToString();

                        olist.Add(newContact);
                    }
                }
            }
            return View(olist);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Contact ocontact)
        {
            using (SqlConnection oconnection = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_Register", oconnection);
                cmd.Parameters.AddWithValue("Name", ocontact.Name);
                cmd.Parameters.AddWithValue("LastName", ocontact.LastName);
                cmd.Parameters.AddWithValue("Phone", ocontact.Phone);
                cmd.Parameters.AddWithValue("Email", ocontact.Email);
                cmd.CommandType = CommandType.StoredProcedure;
                oconnection.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Home", "Contact");
        }

        [HttpGet]
        public ActionResult Edit(int? idcontact)
        {
            if(idcontact == null)
                return RedirectToAction("Home", "Contact");
            
            Contact ocontact = olist.Where(c => c.IdContact == idcontact).FirstOrDefault();
            
            return View(ocontact);
        }

        [HttpPost]
        public ActionResult Edit(Contact ocontact)
        {
            using (SqlConnection oconnection = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_Edit", oconnection);
                cmd.Parameters.AddWithValue("IdContact", ocontact.IdContact);
                cmd.Parameters.AddWithValue("Name", ocontact.Name);
                cmd.Parameters.AddWithValue("LastName", ocontact.LastName);
                cmd.Parameters.AddWithValue("Phone", ocontact.Phone);
                cmd.Parameters.AddWithValue("Email", ocontact.Email);
                cmd.CommandType = CommandType.StoredProcedure;
                oconnection.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Home", "Contact");
        }

        [HttpGet]
        public ActionResult Delete(int? idcontact)
        {
            if (idcontact == null)
                return RedirectToAction("Home", "Contact");

            Contact ocontact = olist.Where(c => c.IdContact == idcontact).FirstOrDefault();

            return View(ocontact);
        }

        [HttpPost]
        public ActionResult Delete(string IdContact)
        {
            using (SqlConnection oconnection = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_Delete", oconnection);
                cmd.Parameters.AddWithValue("IdContact", IdContact);
                cmd.CommandType = CommandType.StoredProcedure;
                oconnection.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Home", "Contact");
        }
    }
}