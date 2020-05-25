using StarIndiaHoliday.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;

namespace StarIndiaHoliday.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult dashboard()
        {
            ViewBag.active = "dashboard";
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.active = "Login";
            return View();
        }
        [HttpPost]

        public ActionResult Login(Login obj)
        {
            try
            {
                if (ModelState.IsValid) //Check here server side validation
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@Username", obj.UserName));
                    list.Add(new SqlParameter("@Password", obj.Password));
                    string status = DataLayer.executescalar("SpLogin", list.ToArray());
                    if (status == "1")
                    {
                        Session["UserNameAdmin"] = obj.UserName;
                        return RedirectToAction("dashboard", "Admin");
                    }
                    else
                    {
                        ViewBag.error = "Wrong UserName or Password";
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            ViewBag.active = "ChangePassword";
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword obj)
        {
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@currentPassword", obj.Password));
                list.Add(new SqlParameter("@newPassword", obj.NewPassword));
                int status = DataLayer.executenonquery("spChangePassword", list.ToArray());
                if (status >= 1)
                {
                    ViewBag.Success = "Now Password Is Change";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Error = "Your Old Password Is Wrong";
                }
                
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public ActionResult destination(int? id)
        {
            ViewBag.active = "destination";
            try
            {
                string cmd = "";

                if (id != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["command"]))
                    {
                        cmd = Request.QueryString["command"].ToString();
                        if (cmd == "del")
                        {
                            var list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@state", "delete"));
                            int status = DataLayer.executenonquery("sptblDesignation", list.ToArray());
                            if (status >= 1)
                            {

                                TempData["msg"] = "Deleted Successfuly";

                                return RedirectToAction("destination", "Admin");


                            }
                        }
                        else if (cmd == "ed")
                        {

                            var listedit = new List<SqlParameter>();
                            listedit.Add(new SqlParameter("@id", id));
                            listedit.Add(new SqlParameter("@state", "edit"));
                            DataTable dt = DataLayer.getdataTable("sptblDesignation", listedit.ToArray());
                            Destination obj = new Destination();
                            obj.categoryName = dt.Rows[0]["categoryName"].ToString();
                            obj.destination = dt.Rows[0]["destination"].ToString();
                            obj.status = Convert.ToBoolean(dt.Rows[0]["status"]);
                            ViewBag.btn = "Update";
                            return View(obj);

                        }

                    }

                }



            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult destination(Destination obj, string cmd, int? id)
        {
            ViewBag.active = "destination";
            try
            {
                if (cmd == "Submit")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@categoryName", obj.categoryName.Trim()));
                    list.Add(new SqlParameter("@destination", obj.destination.Trim()));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@state", "insert"));
                    int status = DataLayer.executenonquery("sptblDesignation", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Inserted";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This destination name is allready exist in the Selected Category.";
                        return View(obj);
                    }

                }
                else
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@categoryName", obj.categoryName.Trim()));
                    list.Add(new SqlParameter("@id", id));
                    list.Add(new SqlParameter("@destination", obj.destination.Trim()));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@state", "update"));
                    int status = DataLayer.executenonquery("sptblDesignation", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Updated";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This Destination name is allready exist.";
                        ViewBag.btn = "Update";
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public PartialViewResult _Partialdestination(string categoryName = "", string state = "")
        {
            List<Destination> obj = new List<Destination>();
            try
            {
                if (state == "")
                {
                    categoryName = "";
                }
                obj = binddestination(categoryName);
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<Destination> binddestination(string categoryName = "")
        {

            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@state", "select"));
            if (categoryName != "")
            {
                list.Add(new SqlParameter("@categoryName", categoryName));
            }
            DataTable dt = DataLayer.getdataTable("sptblDesignation", list.ToArray());
            var listdt = (from DataRow dr in dt.Rows
                          select new Destination()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              categoryName = dr["categoryName"].ToString(),
                              destination = dr["destination"].ToString(),
                              status = Convert.ToBoolean(dr["status"])

                          }).ToList();
            return listdt;

        }


        public ActionResult tour(int? id)
        {
            ViewBag.active = "destination";
            try
            {
                string cmd = "";

                if (id != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["command"]))
                    {
                        cmd = Request.QueryString["command"].ToString();
                        if (cmd == "del")
                        {
                            //Find imageName for delete start
                            string image1 = "";
                            //get image names from table from deleted to folder start
                            var listImage = new List<SqlParameter>();
                            listImage.Add(new SqlParameter("@id", id));
                            listImage.Add(new SqlParameter("@state", "getimagename"));
                            DataTable dt = DataLayer.getdataTable("sptblTour", listImage.ToArray());//For calling SP here for getting imageName and later delete it from folder
                            image1 = Convert.ToString(dt.Rows[0]["imageName"]);
                            DeleteImage(image1);
                            //Find imageName for delete end



                            var list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@state", "delete"));
                            int status = DataLayer.executenonquery("sptblTour", list.ToArray());
                            if (status >= 1)
                            {

                                TempData["msg"] = "Deleted Successfuly";

                                return RedirectToAction("tour", "Admin");


                            }
                        }
                        else if (cmd == "ed")
                        {

                            var listedit = new List<SqlParameter>();
                            listedit.Add(new SqlParameter("@id", id));
                            listedit.Add(new SqlParameter("@state", "edit"));
                            DataTable dt = DataLayer.getdataTable("sptblTour", listedit.ToArray());
                            Tour obj1 = new Tour();
                            obj1.categoryName = dt.Rows[0]["categoryName"].ToString();
                            ViewBag.destinationName = bindDestinationFordropdown(dt.Rows[0]["categoryName"].ToString());
                            obj1.destinationid = Convert.ToInt32(dt.Rows[0]["destinationid"]);
                            obj1.price = Convert.ToInt32(dt.Rows[0]["price"]);
                            obj1.tourName = dt.Rows[0]["tourName"].ToString();
                            obj1.shortDescription = dt.Rows[0]["shortDescription"].ToString();
                            obj1.facility = dt.Rows[0]["facility"].ToString();
                            string[] facilities = dt.Rows[0]["facility"].ToString().Split(',');
                            obj1.objCheck = bindCheckBox();
                            for (int i = 0; i < obj1.objCheck.Count; i++)
                            {
                                foreach (var result in facilities)
                                {
                                    if (result == obj1.objCheck[i].Name)
                                    {
                                        obj1.objCheck[i].Checked = true;
                                    }
                                }
                            }
                            obj1.status = Convert.ToBoolean(dt.Rows[0]["status"]);
                            ViewBag.imageName = dt.Rows[0]["imageName"].ToString();
                            obj1.imageName = Convert.ToString(dt.Rows[0]["imageName"]);
                            obj1.setAsHomePage = Convert.ToBoolean(dt.Rows[0]["setAsHomePage"]);
                            ViewBag.btn = "Update";
                            return View(obj1);

                        }

                    }

                }


            }
            catch (Exception ex)
            {

            }
            Tour obj = new Tour();
            obj.objCheck = bindCheckBox();
            obj.status = true;
            return View(obj);

        }

        public List<CheckModel> bindCheckBox()
        {
            var list = new List<CheckModel>
            {
                 new CheckModel{Id = 1, Name = "Hotels", Checked = false},
                 new CheckModel{Id = 2, Name = "Transport", Checked = false},
                 new CheckModel{Id = 3, Name = "Meals", Checked = false},
                 new CheckModel{Id = 4, Name = "Sightseeing", Checked = false},
                 new CheckModel{Id = 5, Name = "Houseboat", Checked = false},
                 new CheckModel{Id = 6, Name = "Visa", Checked = false},
                 new CheckModel{Id = 7, Name = "Flight", Checked = false},
                 new CheckModel{Id = 8, Name = "Guide", Checked = false},
                 new CheckModel{Id = 9, Name = "Drinks", Checked = false}

            };
            return list;
        }

        [HttpPost]
        public ActionResult tour(Tour obj, IEnumerable<HttpPostedFileBase> files, string cmd, int? id)
        {
            ViewBag.active = "destination";
            try
            {
                if (cmd == "Submit")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@categoryName", obj.categoryName.Trim()));
                    list.Add(new SqlParameter("@destinationid", obj.destinationid));
                    list.Add(new SqlParameter("@tourName", obj.tourName.Trim()));
                    list.Add(new SqlParameter("@shortDescription", obj.shortDescription.Trim()));
                    list.Add(new SqlParameter("@price", obj.price));
                    list.Add(new SqlParameter("@facility", findFacility(obj.objCheck)));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@state", "insert"));
                    list.Add(new SqlParameter("@imageName", uploadImage(obj.file1, null))); //For image upload

                    list.Add(new SqlParameter("@setAsHomePage", Convert.ToByte(obj.setAsHomePage))); //For setAsHomePage
                    int status = DataLayer.executenonquery("sptblTour", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Inserted";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This Tour name is allready exist in the Selected Category and destination.";
                        return View(obj);
                    }

                }
                else
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@categoryName", obj.categoryName.Trim()));
                    list.Add(new SqlParameter("@id", id));
                    list.Add(new SqlParameter("@destinationid", obj.destinationid));
                    list.Add(new SqlParameter("@tourName", obj.tourName.Trim()));
                    list.Add(new SqlParameter("@shortDescription", obj.shortDescription.Trim()));
                    list.Add(new SqlParameter("@price", obj.price));
                    list.Add(new SqlParameter("@facility", findFacility(obj.objCheck)));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@state", "update"));
                    //For image upload start
                    foreach (var file in files)
                    {

                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                string s = uploadImage(file, id);
                                obj.imageName = s;
                            }
                        }
                    }
                    list.Add(new SqlParameter("@imageName", obj.imageName));
                    //For image upload end
                    list.Add(new SqlParameter("@setAsHomePage", Convert.ToByte(obj.setAsHomePage))); //For setAsHomePage
                    int status = DataLayer.executenonquery("sptblTour", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Updated";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This Tour name is allready exist in the Selected Category and destination.";
                        ViewBag.btn = "Update";
                    }

                }

            }
            catch (Exception ex)
            {

            }
            Tour obj1 = new Tour();
            obj1.objCheck = bindCheckBox();
            obj1.status = true;
            return View(obj1);
        }



        //For uploading Image start
        public string uploadImage(HttpPostedFileBase file, int? id, string pageName = "",int ctr=0)
        {
            string fileName = "";
            string trfile = "";
            int MaxContentLength = 1024 * 1024 * 10; //10 MB
            string[] AllowedFileExtensions = new string[] { ".jpg", ".JPG", ".gif", ".GIF", ".png", "PNG", ".jpeg", ".JPEG" };
            if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
            }
            else
            {
                //TO:DO
                //MyDBContext objDbContext = new MyDBContext();
                if (id != null)
                {
                    string image1 = "", image2 = "";
                    //get image names from table from deleted to folder start
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@id", id));
                    list.Add(new SqlParameter("@state", "getimagename"));
                    if (pageName == "Hotpackage")
                    {
                        DataTable dt1 = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                        image1 = Convert.ToString(dt1.Rows[0]["photo1"]);
                        image2 = Convert.ToString(dt1.Rows[0]["photo2"]);
                        if(ctr==1)
                            DeleteImage(image1);
                        else if (ctr == 2)
                            DeleteImage(image2);
                        dt1.Dispose();
                    }
                    else if (pageName == "touristFeedback")
                    {
                        var listImage = new List<SqlParameter>();
                        listImage.Add(new SqlParameter("@id", id));
                        listImage.Add(new SqlParameter("@state", "getimagename"));
                        DataTable dt = DataLayer.getdataTable("SPtblTouristFeedback", listImage.ToArray());//For calling SP here for getting imageName and later delete it from folder
                        image1 = Convert.ToString(dt.Rows[0]["imageName"]);
                        DeleteImage(image1);
                        dt.Dispose();
                    }
                    else
                    {
                        DataTable dt = DataLayer.getdataTable("sptblTour", list.ToArray());//For calling SP here for getting imageName and later delete it from folder
                        image1 = Convert.ToString(dt.Rows[0]["imageName"]);
                        DeleteImage(image1);
                        dt.Dispose();
                    }




                }
                fileName = Path.GetFileName(file.FileName);
                Random RandomClass = new Random();
                int randmno = RandomClass.Next(345678);
                if (fileName.Contains(" ") || fileName.Contains("*") || fileName.Contains("+") || fileName.Contains("@") || fileName.Contains("=") || fileName.Contains("%") || fileName.Contains("  ") || fileName.Contains("&") || fileName.Contains("&&") || fileName.Contains("#") || fileName.Contains("'") || fileName.Contains("`") || fileName.Contains("~") || fileName.Contains("?") || fileName.Contains(","))
                {
                    fileName = fileName.Replace(" ", "_");
                    fileName = fileName.Replace("  ", "_");
                    fileName = fileName.Replace("*", "_");
                    fileName = fileName.Replace("+", "_");
                    fileName = fileName.Replace("%", "_");
                    fileName = fileName.Replace("@", "_");
                    fileName = fileName.Replace("=", "_");
                    fileName = fileName.Replace("&", "_");
                    fileName = fileName.Replace("&&", "_");
                    fileName = fileName.Replace("#", "_");
                    fileName = fileName.Replace("'", "_");
                    fileName = fileName.Replace("`", "_");
                    fileName = fileName.Replace("~", "_");
                    fileName = fileName.Replace("?", "_");
                    fileName = fileName.Replace(",", "_");

                    trfile = randmno.ToString() + fileName.ToString();
                }
                else
                {
                    trfile = randmno.ToString() + fileName.ToString();
                }
                var path = Path.Combine("");
                if (pageName == "Hotpackage")
                {
                    path = Path.Combine(Server.MapPath("~/Content/uploadImage/largeImage"), trfile);

                    ImageUpload.CreateThumbnails(file, path, Convert.ToInt32(533));   // here we send image width: 533px for cutting the image
                }
                else
                {
                    if (pageName == "touristFeedback")
                    {
                        path = Path.Combine(Server.MapPath("~/Content/uploadImage/smallImage"), trfile);

                        ImageUpload.CreateThumbnails(file, path, Convert.ToInt32(90));
                    }
                    else
                    { 
                        path = Path.Combine(Server.MapPath("~/Content/uploadImage/smallImage"), trfile);

                        ImageUpload.CreateThumbnails(file, path, Convert.ToInt32(350));   // here we send image width: 280px for cutting the image

                        path = Path.Combine(Server.MapPath("~/Content/uploadImage/largeImage"), trfile);

                        ImageUpload.CreateThumbnails(file, path, Convert.ToInt32(450));   // here we send image width: 393px for cutting the image
                    }
                    }
                ModelState.Clear();
                ViewBag.Message = "File uploaded successfully";

            }
            return trfile;
        }
        //For uploading Image end


        protected void DeleteImage(string imagename)
        {
            string path1 = Server.MapPath("~/Content/uploadImage/smallImage/" + imagename);
            FileInfo fi1 = new FileInfo(path1);
            if (fi1.Exists)
                fi1.Delete();
            path1 = Server.MapPath("~/Content/uploadImage/largeImage/" + imagename);
            fi1 = new FileInfo(path1);
            if (fi1.Exists)
                fi1.Delete();



        }




        public string findFacility(List<CheckModel> obj)
        {
            string checkedFacility = "";
            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].Checked == true)
                {
                    checkedFacility += obj[i].Name + ",";
                }
            }
            if (checkedFacility != "")
            {
                checkedFacility = checkedFacility.TrimEnd(',');
            }
            return checkedFacility;
        }
        public JsonResult bindDestinationFromJson(string categoryName = "")
        {
            var res = bindDestinationFordropdown(categoryName);
            return Json(new SelectList(res.ToArray(), "id", "destination"), JsonRequestBehavior.AllowGet);
        }
        public List<Destination> bindDestinationFordropdown(string categoryName = "")
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@state", "binddestination"));
            list.Add(new SqlParameter("@categoryName", categoryName));

            DataTable dt = DataLayer.getdataTable("sptblTour", list.ToArray());
            var listdt = (from DataRow dr in dt.Rows
                          select new Destination()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              destination = dr["destination"].ToString()

                          }).ToList();
            return listdt;
        }

        public PartialViewResult _PartialTour(string categoryName = "", string destinationid = "", string state = "")
        {
            List<Tour> obj = new List<Tour>();
            try
            {
                if (state == "")
                {
                    categoryName = "";
                    destinationid = "";
                }
                obj = bindTour(categoryName, destinationid);
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public List<Tour> bindTour(string categoryName = "", string destinationid = "")
        {

            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@state", "select"));
            if (categoryName != "" && destinationid != "")
            {
                list.Add(new SqlParameter("@categoryName", categoryName));
                list.Add(new SqlParameter("@destinationid", destinationid));
            }
            else if (categoryName != "" && destinationid == "")
            {
                list.Add(new SqlParameter("@categoryName", categoryName));
            }
            DataTable dt = DataLayer.getdataTable("sptblTour", list.ToArray());
            var listdt = (from DataRow dr in dt.Rows
                          select new Tour()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              categoryName = dr["categoryName"].ToString(),
                              destination = dr["destination"].ToString(),
                              tourName = dr["tourName"].ToString(),
                              shortDescription = dr["shortDescription"].ToString(),
                              price = Convert.ToInt32(dr["price"]),
                              facility = dr["facility"].ToString(),
                              imageName = dr["imageName"].ToString(),
                              status = Convert.ToBoolean(dr["status"]),
                              setAsHomePage = Convert.ToBoolean(dr["setAsHomePage"])

                          }).ToList();
            return listdt;

        }


        public ActionResult Hotpackage(int? id)
        {
            ViewBag.active = "Hotpackage";
            try
            {
                string cmd = "";

                if (id != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["command"]))
                    {
                        cmd = Request.QueryString["command"].ToString();

                        if (cmd.Contains("delimg"))
                        {
                            var list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@State", "getimagename"));
                            DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                            string photo = "";
                            if (cmd.Contains("-2"))
                                photo = dt.Rows[0]["photo2"].ToString();
                            if (cmd.Contains("-1"))
                                photo = dt.Rows[0]["photo1"].ToString();
                            DeleteImage(photo);
                            list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            if (cmd.Contains("-2"))
                                list.Add(new SqlParameter("@photo2", "2"));
                            if (cmd.Contains("-1"))
                                list.Add(new SqlParameter("@photo1", "1"));
                            list.Add(new SqlParameter("@State", "deleteimage"));
                            int status = DataLayer.executenonquery("SPtblHotPackage", list.ToArray());
                            TempData["msg"] = "Image deleted.";
                            dt.Dispose();
                            return RedirectToAction("Hotpackage", "Admin");

                        }
                        else if (cmd == "del")
                        {
                            //Fetch fileName before delete row for delete file from the folder start
                            string photo1 = "", photo2 = "";
                            var list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@State", "getimagename"));
                            DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                            photo1 = dt.Rows[0]["photo1"].ToString();
                            photo2 = dt.Rows[0]["photo2"].ToString();
                            dt.Dispose();
                            
                            DeleteImage(photo1);
                            if (photo2 != "")
                                DeleteImage(photo2);
                            //Fetch fileName before delete row for delete file from the folder end
                            list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@State", "delete"));
                            int status = DataLayer.executenonquery("SPtblHotPackage", list.ToArray());
                            if (status >= 1)
                            {

                                TempData["msg"] = "Record deleted";

                                return RedirectToAction("Hotpackage", "Admin");


                            }
                        }
                        else if (cmd == "ed")
                        {

                            var listedit = new List<SqlParameter>();
                            listedit.Add(new SqlParameter("@id", id));
                            listedit.Add(new SqlParameter("@State", "edit"));
                            DataTable dt = DataLayer.getdataTable("SPtblHotPackage", listedit.ToArray());
                            Hotpackage obj = new Hotpackage();
                            obj.PackageName = Convert.ToString(dt.Rows[0]["PackageName"]);
                            obj.Duration = Convert.ToString(dt.Rows[0]["Duration"]);
                            obj.ShortDescription = Convert.ToString(dt.Rows[0]["ShortDescription"]);
                            obj.Details = Convert.ToString(dt.Rows[0]["Details"]);
                            obj.TermsConditions = Convert.ToString(dt.Rows[0]["TermsConditions"]);
                            ViewBag.photo1 = Convert.ToString(dt.Rows[0]["photo1"]);
                            ViewBag.photo2 = Convert.ToString(dt.Rows[0]["photo2"]);

                            obj.Status = Convert.ToBoolean(dt.Rows[0]["Status"]);
                            obj.PackageCost = Convert.ToString(dt.Rows[0]["PackageCost"]);
                            ViewBag.btn = "Update";
                            return View(obj);

                        }
                        }
                    }
                
            }
            catch (Exception ex)
            {

            }
            return View();

        }


        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Hotpackage(Hotpackage obj, IEnumerable<HttpPostedFileBase> files, string cmd, int? id)
        {
            ViewBag.active = "Hotpackage";
            try
            {
                if (cmd == "Submit")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@PackageName", obj.PackageName.Trim()));
                    list.Add(new SqlParameter("@Duration", obj.Duration));
                    list.Add(new SqlParameter("@ShortDescription", obj.ShortDescription));
                    list.Add(new SqlParameter("@Details", obj.Details));
                    list.Add(new SqlParameter("@TermsConditions", obj.TermsConditions));
                    list.Add(new SqlParameter("@PackageCost", obj.PackageCost));
                    list.Add(new SqlParameter("@Status", Convert.ToByte(obj.Status)));
                    list.Add(new SqlParameter("@State", "insert"));
                    if(obj.file1!=null)
                    list.Add(new SqlParameter("@photo1", uploadImage(obj.file1, null, "Hotpackage"))); //For image upload
                    if (obj.file2 != null)
                        list.Add(new SqlParameter("@photo2", uploadImage(obj.file2, null, "Hotpackage"))); //For image upload
                    int status = DataLayer.executenonquery("SPtblHotPackage", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Inserted";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This hot package is allready exist,Please insert with different name.";
                        //ViewBag.btn = "Update";
                        return View(obj);
                    }

                }
                else
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@id", id));
                    list.Add(new SqlParameter("@PackageName", obj.PackageName.Trim()));
                    list.Add(new SqlParameter("@Duration", obj.Duration));
                    list.Add(new SqlParameter("@ShortDescription", obj.ShortDescription));
                    list.Add(new SqlParameter("@Details", obj.Details));
                    list.Add(new SqlParameter("@TermsConditions", obj.TermsConditions));
                    list.Add(new SqlParameter("@PackageCost", obj.PackageCost));
                    list.Add(new SqlParameter("@Status", Convert.ToByte(obj.Status)));
                    list.Add(new SqlParameter("@State", "update"));
                    //For image upload start
                    int ctr = 0;

                    foreach (var file in files)
                    {
                        ctr = ctr + 1;
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {

                                string s = uploadImage(file, id, "Hotpackage",ctr);
                                if (ctr == 1)
                                    obj.photo1 = s;
                                else if (ctr == 2)
                                    obj.photo2 = s;
                            }
                        }
                    }
                    list.Add(new SqlParameter("@photo1", obj.photo1));
                    list.Add(new SqlParameter("@photo2", obj.photo2));
                    //For image upload end
                    int status = DataLayer.executenonquery("SPtblHotPackage", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Updated";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "This hot package is allready exist,Please insert with different name.";
                        ViewBag.btn = "Update";
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }


        
        public PartialViewResult _PartialHotpackage()
        {
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "select"));
                DataTable dt = DataLayer.getdataTable("SPtblHotPackage", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows
                              select new Hotpackage()
                              {
                                  id = Convert.ToInt32(dr["id"]),
                                  PackageName = dr["PackageName"].ToString(),
                                  Duration = dr["Duration"].ToString(),
                                  ShortDescription = dr["ShortDescription"].ToString(),
                                  PackageCost = dr["PackageCost"].ToString(),
                                  photo1 = dr["photo1"].ToString(),
                                  photo2 = dr["photo2"].ToString(),
                                  Status = Convert.ToBoolean(dr["Status"])

                              }).ToList();
                return PartialView(listdt);
            }
            catch (Exception ex)
            {
                return null;
            }

        }




        public ActionResult touristFeedback(int? id)
        {
            ViewBag.active = "touristFeedback";
            try
            {
                string cmd = "";

                if (id != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["command"]))
                    {
                        cmd = Request.QueryString["command"].ToString();
                        if (cmd == "del")
                        {
                            //Find imageName for delete start
                            string image1 = "";
                            //get image names from table from deleted to folder start
                            var listImage = new List<SqlParameter>();
                            listImage.Add(new SqlParameter("@id", id));
                            listImage.Add(new SqlParameter("@state", "getimagename"));
                            DataTable dt = DataLayer.getdataTable("SPtblTouristFeedback", listImage.ToArray());//For calling SP here for getting imageName and later delete it from folder
                            image1 = Convert.ToString(dt.Rows[0]["imageName"]);
                            DeleteImage(image1);
                            //Find imageName for delete end


                            var list = new List<SqlParameter>();
                            list.Add(new SqlParameter("@id", id));
                            list.Add(new SqlParameter("@state", "delete"));
                            int status = DataLayer.executenonquery("SPtblTouristFeedback", list.ToArray());
                            if (status >= 1)
                            {

                                TempData["msg"] = "Deleted Successfuly";

                                return RedirectToAction("touristFeedback", "Admin");


                            }
                        }
                        //else if(cmd=="delimg")
                        //{
                        //    //Find imageName for delete start
                        //    string image1 = "";
                        //    //get image names from table from deleted to folder start
                        //    var listImage = new List<SqlParameter>();
                        //    listImage.Add(new SqlParameter("@id", id));
                        //    listImage.Add(new SqlParameter("@state", "getimagename"));
                        //    DataTable dt = DataLayer.getdataTable("SPtblTouristFeedback", listImage.ToArray());//For calling SP here for getting imageName and later delete it from folder
                        //    image1 = Convert.ToString(dt.Rows[0]["imageName"]);
                        //    DeleteImage(image1);
                        //    TempData["msg"] = "Image deleted";

                        //    return RedirectToAction("touristFeedback", "Admin");
                        //    //Find imageName for delete end
                        //}
                        else if (cmd == "ed")
                        {

                            var listedit = new List<SqlParameter>();
                            listedit.Add(new SqlParameter("@id", id));
                            listedit.Add(new SqlParameter("@state", "edit"));
                            DataTable dt = DataLayer.getdataTable("SPtblTouristFeedback", listedit.ToArray());
                            touristFeedback obj1 = new touristFeedback();
                            obj1.name = dt.Rows[0]["name"].ToString();
                            obj1.mobileNumber = dt.Rows[0]["mobileNumber"].ToString();
                            obj1.designatin = dt.Rows[0]["designatin"].ToString();
                            obj1.shortDescription = dt.Rows[0]["shortDescription"].ToString();
                            obj1.status = Convert.ToBoolean(dt.Rows[0]["status"]);
                            ViewBag.imageName = dt.Rows[0]["imageName"].ToString();
                            obj1.imageName = Convert.ToString(dt.Rows[0]["imageName"]);
                            ViewBag.btn = "Update";
                            return View(obj1);

                        }

                    }

                }


            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult touristFeedback(touristFeedback obj, IEnumerable<HttpPostedFileBase> files, string cmd, int? id)
        {
            ViewBag.active = "touristFeedback";
            try
            {
                if (cmd == "Submit")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@name", obj.name.Trim()));
                    list.Add(new SqlParameter("@designatin", obj.designatin));
                    list.Add(new SqlParameter("@imageName", uploadImage(obj.file1, null, "touristFeedback"))); //For image upload
                    list.Add(new SqlParameter("@shortDescription", obj.shortDescription.Trim()));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@state", "insert"));
                    list.Add(new SqlParameter("@mobileNumber", obj.mobileNumber));
                    int status = DataLayer.executenonquery("SPtblTouristFeedback", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Inserted";
                        ModelState.Clear(); 
                    } 
                    else
                    {
                        ViewBag.Error = "Details for this person's name and mobile number allready exists.";
                        //ViewBag.btn = "Update";
                    } 
                }
                else
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@name", obj.name.Trim()));
                    list.Add(new SqlParameter("@designatin", obj.designatin));
                    list.Add(new SqlParameter("@shortDescription", obj.shortDescription.Trim()));
                    list.Add(new SqlParameter("@status", Convert.ToByte(obj.status)));
                    list.Add(new SqlParameter("@mobileNumber", obj.mobileNumber));
                    list.Add(new SqlParameter("@id", id));
                    list.Add(new SqlParameter("@state", "update"));
                    //For image upload start
                    foreach (var file in files)
                    {

                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                string s = uploadImage(file, id, "touristFeedback");
                                obj.imageName = s;
                            }
                        }
                    }
                    list.Add(new SqlParameter("@imageName", obj.imageName));
                    //For image upload end

                    int status = DataLayer.executenonquery("SPtblTouristFeedback", list.ToArray());
                    if (status == 1)
                    {
                        ViewBag.Success = "Record Updated";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error = "Details for this person's name and mobile number allready exists.";
                        ViewBag.btn = "Update";
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return View();
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
                                 
                              }).ToList();
                obj = listdt;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

      public ActionResult Indexenquiry()
        {
            ViewBag.active = "Indexenquiry";
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "indexdisplay"));
                DataTable dt = DataLayer.getdataTable("SPEnquiry", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows
                              select new Indexenquiry()
                              {
                                  Name = dr["Name"].ToString(),
                                  Email = dr["Email"].ToString(),
                                  Mobile = dr["Mobile"].ToString(),
                                  Message = dr["Message"].ToString()

                              }).ToList();
                return View(listdt);
            }
            catch (Exception ex)
            {
                return RedirectToAction("dashboard");
            }
        }





        public ActionResult contactusenquiry()
        {
            ViewBag.active = "ContactusEnquiry";
            try
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@state", "contactusdisplay"));
                DataTable dt = DataLayer.getdataTable("SPEnquiry", list.ToArray());
                var listdt = (from DataRow dr in dt.Rows
                              select new ContactusEnquiry() 
                              {
                                  Name = dr["Name"].ToString(),
                                  Email = dr["Email"].ToString(),
                                  Mobile = dr["Mobile"].ToString(),
                                  Message = dr["Message"].ToString(),
                                  EnquiryFor = dr["EnquiryFor"].ToString()

                              }).ToList(); 
                return View(listdt);
            }
            catch (Exception ex)
            {
                return RedirectToAction("dashboard");
            }

        } 
    }
}

