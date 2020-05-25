using StarIndiaHoliday.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace StarIndiaHoliday.Controllers
{
    public class StarIndiaHolidayController : Controller
    {
        // GET: StarIndiaHoliday
        public ActionResult Index()
        {
            ViewBag.active = "index";
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "setashomepage"));
                DataTable dt = DataLayer.getdataTable("SPtblTour", list.ToArray());
                var listdt1 = (from DataRow dr in dt.Rows
                               select new Tour()
                               {
                                   id = Convert.ToInt32(dr["id"]),
                                   categoryName = dr["categoryName"].ToString(),
                                   destination = dr["destination"].ToString(),
                                   price = Convert.ToInt32(dr["price"]),
                                   imageName = dr["imageName"].ToString(),
                                   tourName = dr["tourName"].ToString(),
                                   shortDescription = dr["shortDescription"].ToString(),
                                   facility = dr["facility"].ToString(),
                               }).ToList();
                return View(listdt1);

            }
            catch(Exception ex)
            {

            }
            return View();
        }

        public PartialViewResult _PartialTourPackageMenu()
        {
            try
            {
                TourPackageMenu obj = new TourPackageMenu();
                DataSet ds = DataLayer.getdset("SPbindTourMenu");
                var listdt1 = (from DataRow dr in ds.Tables[0].Rows
                               select new Destination()
                               {
                                   id = Convert.ToInt32(dr["id"]),
                                   categoryName = dr["categoryName"].ToString(),
                                   destination = dr["destination"].ToString()
                               }).ToList();
                obj.objdestination = listdt1;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public PartialViewResult _PartialTouristFeedback()
        {
            List<touristFeedback> obj = new List<touristFeedback>();
            try
            { 
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "select"));
                DataTable dt = DataLayer.getdataTable("SPtblTouristFeedback", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows 
                              select new touristFeedback()
                              {
                                  id = Convert.ToInt32(dr["id"]),
                                  name = dr["name"].ToString(),
                                  mobileNumber = dr["mobileNumber"].ToString(),
                                  shortDescription = dr["shortDescription"].ToString(),
                                  designatin = dr["designatin"].ToString(),
                                  imageName = dr["imageName"].ToString(),
                                  status = Convert.ToBoolean(dr["status"]) 
                              }).Where(x=>x.status==true).ToList();
                obj = listdt;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public ActionResult tour(string tourtype, string tourname) 
        {
            ViewBag.active = "tour";
            try
            {
                if (tourtype == "")
                {
                    return View();
                }
                ViewBag.tourType = tourtype.Replace("-", " ").Split(' ')[0];
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@tourtype", tourtype.Replace("-", " ").Split(' ')[0]));
                if (!String.IsNullOrEmpty(tourname))
                {
                    list.Add(new SqlParameter("@tourname", tourname.Replace("-", " ")));
                }
                DataTable dt = DataLayer.getdataTable("SPbindTour", list.ToArray());
                var listdt1 = (from DataRow dr in dt.Rows 
                               select new Tour()
                               {
                                   id = Convert.ToInt32(dr["id"]),
                                   categoryName = dr["categoryName"].ToString(),
                                   destination = dr["destination"].ToString(),
                                   price = Convert.ToInt32(dr["price"]),
                                   imageName = dr["imageName"].ToString(),
                                   tourName = dr["tourName"].ToString(),
                                   shortDescription = dr["shortDescription"].ToString(),
                                   facility = dr["facility"].ToString(),
                               }).ToList();
                return View(listdt1);
            

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public PartialViewResult _PartialLeftMenu()
        {
            try
            {
                TourPackageMenu obj = new TourPackageMenu();
                DataSet ds = DataLayer.getdset("SPbindTourMenu");
                var listdt1 = (from DataRow dr in ds.Tables[0].Rows
                               select new Destination()
                               {
                                   id = Convert.ToInt32(dr["id"]),
                                   categoryName = dr["categoryName"].ToString(),
                                   destination = dr["destination"].ToString()
                               }).ToList();
                obj.objdestination = listdt1;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public ActionResult aboutus()
        {
            ViewBag.active = "aboutus";
            return View();
        }

        public ActionResult contactus(string tourtype)
        {
            int temp = 0;
            if (!String.IsNullOrEmpty(tourtype) && int.TryParse(tourtype, out temp))
            {
                var listedit = new List<SqlParameter>();
                listedit.Add(new SqlParameter("@id", tourtype));
                listedit.Add(new SqlParameter("@state", "getdetailforenquiry"));
                DataTable dt = DataLayer.getdataTable("sptblTour", listedit.ToArray());
                ViewBag.tourDetails = dt.Rows[0]["categoryName"].ToString() + " Tour" + " / " + dt.Rows[0]["destination"].ToString() + " / " + dt.Rows[0]["tourName"].ToString();
            }
            ViewBag.active = "contactus";
            return View();
        }

        [HttpPost]
        public ActionResult contactus(contactus obj)
        {
            ViewBag.active = "contactus";
            if (obj.captcha == HttpContext.Session["captchacontact"].ToString())
            {
                try
                {
                    //Insert record in DB Start

                    var listedit = new List<SqlParameter>();
                    listedit.Add(new SqlParameter("@Name", obj.name));
                    listedit.Add(new SqlParameter("@Email", obj.email));
                    listedit.Add(new SqlParameter("@Mobile", obj.mobile));
                    listedit.Add(new SqlParameter("@Message", obj.message));
                    if (!String.IsNullOrEmpty("obj.enquiryforTour"))
                    {
                        listedit.Add(new SqlParameter("@EnquiryFor", obj.enquiryforTour));
                    }
                    listedit.Add(new SqlParameter("@state", "contactus"));
                    int status = DataLayer.executenonquery("SPEnquiry", listedit.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Your enquiry has been sent,We will come back to you soon.";

                    }
                    //Email send message using goDAddy Start

                    //Create the msg object to be sent
                    MailMessage msg = new MailMessage();
                    //Add your email address to the recipients
                    msg.To.Add("starindiaholiday123@gmail.com");
                    //Configure the address we are sending the mail from
                    MailAddress address = new MailAddress("starindiaholiday123@gmail.com");
                    msg.From = address;
                    msg.Subject = "Enquiry";
                    msg.Body = "Name : " + obj.name + "<br/>" + "Email : " + obj.email + "<br/>" + "Mobile : " + obj.mobile + "<br/>" + "Subject : " + "Enquiry" + "<br/>" + "Enquiry For : " + obj.enquiryforTour + "<br/>" + "Message : " + obj.message + "<br/>";
                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = false;
                    client.Host = "relay-hosting.secureserver.net";
                    client.Port = 25;
                    NetworkCredential credentials = new NetworkCredential("starindiaholiday123@gmail.com", "starindia123");
                    client.UseDefaultCredentials = true;
                    client.Credentials = credentials;
                    client.Send(msg);
                    ViewBag.Success = "Your enquiry has been sent,We will come back to you soon.";
                    //ModelState.Clear();

                    //Email send message using goDAddy End
                }
                catch (Exception ex)
                {
                    MailSend("Exception Mail:" + ex.Message + ex.StackTrace + ex.InnerException);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(obj.enquiryforTour))
                {
                ViewBag.tourDetails = obj.enquiryforTour;
                }
                ViewBag.Error = "Captcha code is wrong,try again.";
            }
            ModelState.Clear();
            return View();
        }

        public CaptchaResult getcaptchaContactUs()
        {
            string captchaText = Captcha.GenerateRandomCode();
            HttpContext.Session.Add("captchacontact", captchaText);
            return new CaptchaResult(captchaText);

        }

        public PartialViewResult _PartialIndexEnquiry()
        {
            return PartialView();
        }
          
        public CaptchaResult getcaptindexEnquiry()
        {
            string captchaText = Captcha.GenerateRandomCode();
            HttpContext.Session.Add("captchaindex", captchaText);
            return new CaptchaResult(captchaText);
        }

        public ActionResult MailToAdmin(string name, string email, string mobile, string captcha, string message)
        {
            if (captcha == HttpContext.Session["captchaindex"].ToString())
            {
                var result = "";
                try
                {

                    var listedit = new List<SqlParameter>();
                    listedit.Add(new SqlParameter("@Name", name));
                    listedit.Add(new SqlParameter("@Email", email));
                    listedit.Add(new SqlParameter("@Mobile", mobile));
                    listedit.Add(new SqlParameter("@Message", message));
                    listedit.Add(new SqlParameter("@state", "index"));
                    int status = DataLayer.executenonquery("SPEnquiry", listedit.ToArray());
                    if (status == 1)
                    {
                        result = "Yes";

                    }
                    else
                    {

                    }
                    MailMessage msg = new MailMessage();
                    //Add your email address to the recipients
                    msg.To.Add("starindiaholiday123@gmail.com");
                    //Configure the address we are sending the mail from
                    MailAddress address = new MailAddress("starindiaholiday123@gmail.com");
                    msg.From = address;
                    msg.Subject = "Enquiry";
                    msg.Body = "Name : " + name + "<br/>" + "Email : " + email + "<br/>" + "Mobile : " + mobile + "<br/>" + "Subject : " + "Enquiry" + "<br/>" + "Message : " + message + "<br/>";
                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = false;
                    client.Host = "relay-hosting.secureserver.net";
                    client.Port = 25;
                    NetworkCredential credentials = new NetworkCredential("starindiaholiday123@gmail.com", "starindia123");
                    client.UseDefaultCredentials = true;
                    client.Credentials = credentials;
                    client.Send(msg);
                    result = "Yes";
                }
                catch(Exception ex)
                {
                    MailSend("Exception Mail:Message:   " + ex.Message + ex.StackTrace + ex.InnerException);
                }

                
                return Json(result);

            }
            else
            {
                var result = "No";
                return Json(result);
            }


        }
         
        public void MailSend(string body="")
        {
            try
            {
                //Fetching Settings from WEB.CONFIG file.  
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string emailFromAddress = "chandani@AKSindia.com"; //Sender Email Address  
                string password = "Aks@4356"; //Sender Password  
                string emailToAddress = "chandani@AKSindia.com"; //Receiver Email Address  
                string subject = "Exception Mail";
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                ////Fetching Email Body Text from EmailTemplate File.
                //string FilePath = "";
                //if (!String.IsNullOrEmpty(tourName))
                //{
                //    FilePath = Server.MapPath("~/emailTemplates/enquiryfortour.html");
                //}
                //else
                //{
                //    FilePath = Server.MapPath("~/emailTemplates/enquiry.html");
                //}
                //StreamReader str = new StreamReader(FilePath);
                //string MailText = str.ReadToEnd();
                //str.Close();
                //if (!String.IsNullOrEmpty(tourName))
                //{
                //    MailText = MailText.Replace("[Tour]", tourName);
                //}
                ////Repalce [Name] = firstname lastname   
                //MailText = MailText.Replace("[Name]", name);
                ////Repalce [Email] = email
                //MailText = MailText.Replace("[Email]", email);

                //MailText = MailText.Replace("[Mobile]", mobile);

                //MailText = MailText.Replace("[Message]", message);

                //string subject = "Enquiry from StarIndiaHoliday";

                ////Base class for sending email  
                //MailMessage _mailmsg = new MailMessage();

                ////Make TRUE because our body text is html  
                //_mailmsg.IsBodyHtml = true;

                ////Set From Email ID  
                //_mailmsg.From = new MailAddress(emailSender);

                ////Set To Email ID  
                //_mailmsg.To.Add("chandani@aksindia.com");  //jise mail send karna ho uski mail id yaha lagega, agar ek se jyada logo ko send karna ho to , laga kar sende kar skte h 

                ////Set Subject  
                //_mailmsg.Subject = subject;

                ////Set Body Text of Email   
                //_mailmsg.Body = MailText;


                ////Now set your SMTP   
                //SmtpClient _smtp = new SmtpClient();

                ////Set HOST server SMTP detail  
                //_smtp.Host = emailSenderHost;

                ////Set PORT number of SMTP  
                //_smtp.Port = emailSenderPort;
                //_smtp.Timeout = 10000;
                //_smtp.UseDefaultCredentials = false;
                ////Set SSL --> True / False  
                //_smtp.EnableSsl = emailIsSSL;

                ////Set Sender UserEmailID, Password  
                //NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
                //_smtp.Credentials = _network;

                ////Send Method will send your MailMessage create above.  
                //_smtp.Send(_mailmsg);
            }
            catch (Exception ex)
            {
                //https://myaccount.google.com/lesssecureapps
            }
        }
          
        public PartialViewResult _PartialHotPackageMenu()
        {
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "bindmenu"));
                DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows
                              select new Hotpackage()
                              {
                                  id = Convert.ToInt32(dr["id"]),
                                  PackageName = dr["PackageName"].ToString()

                              }).ToList();
                return PartialView(listdt);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
          
        public ActionResult Hotpackage(string tourtype)
        {
            ViewBag.active = "hotPackage";
            try
            {
                var list = new List<SqlParameter>();

                int temp;
                if (!String.IsNullOrEmpty(tourtype) && int.TryParse(tourtype, out temp))
                {
                    list.Add(new SqlParameter("@id", tourtype));
                    list.Add(new SqlParameter("@state", "edit"));
                    DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                    Hotpackage obj = new Hotpackage();
                    obj.PackageName = Convert.ToString(dt.Rows[0]["PackageName"]);
                    obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                    obj.Duration = Convert.ToString(dt.Rows[0]["Duration"]);
                    obj.PackageCost = Convert.ToString(dt.Rows[0]["PackageCost"]);
                    obj.Details = Convert.ToString(dt.Rows[0]["Details"]);
                    obj.ShortDescription = Convert.ToString(dt.Rows[0]["ShortDescription"]);
                    obj.photo1 = Convert.ToString(dt.Rows[0]["photo1"]);
                    obj.photo2 = Convert.ToString(dt.Rows[0]["photo2"]);
                    obj.TermsConditions = Convert.ToString(dt.Rows[0]["TermsConditions"]);
                    return View(obj);
                }
            

            }
            catch (Exception ex)
            {

            }
            return View();
        }
         
        public PartialViewResult _PartialHotPackageForIndex()
        {
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "bindmenu"));
                DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows
                              select new Hotpackage()
                              {
                                  id = Convert.ToInt32(dr["id"]),
                                  PackageName = dr["PackageName"].ToString(),
                                  photo1 = dr["photo1"].ToString(),
                                  Duration = dr["Duration"].ToString(),
                                  PackageCost = dr["PackageCost"].ToString()

                              }).ToList();
                return PartialView(listdt);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
 
        private void mailSend()
        {
            try
            {
               
                //Display some feedback to the user to let them know it was sent
                
            }
            catch (Exception ex)
            {
               
            }

        }

        public ActionResult TermsCondtion()
        {
            return View();
        }

        public ActionResult Disclaimer()
        {
            return View();
        }


        public ActionResult Test ()
        {
            List<Destination> obj = new List<Destination>();
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@state", "select"));
           
            DataTable dt = DataLayer.getdataTable("sptblDesignation", list.ToArray());
            var listdt = (from DataRow dr in dt.Rows
                          select new Destination()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              categoryName = dr["categoryName"].ToString(),
                              destination = dr["destination"].ToString(),
                              status = Convert.ToBoolean(dr["status"])

                          }).ToList();
            obj = listdt;
            return View(obj);
        }

        public ActionResult TestSelfPractice()
        {
            return View();
        }        
        

    }
}