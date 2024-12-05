using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobApplication.DAL;
using JobApplication.Models;

using static System.Net.Mime.MediaTypeNames;

namespace JobApplication.Controllers
{
    public class ApplicationController : Controller
    {
        Application_DAL _applicationDAL = new Application_DAL();
        public ActionResult Index()
        {
            var applicationList = _applicationDAL.GetAllApplications();
            if (applicationList.Count == 0)
            {
                TempData["InfoMessage"] = "Currenty no applications availabe";
            }
            return View(applicationList);
        }

        // GET: Application/Details/5
        public ActionResult ApplicationDetails(int id)
        {
            var applications = _applicationDAL.GetApplicationByID(id).FirstOrDefault();
            if (applications == null)
            {
                TempData["ErrorMessage"] = "application not avialable with id :" + id.ToString();
                return RedirectToAction("Index");
            }
            return View(applications);
        }

        // GET: Application/Create
        public ActionResult RegisterApplication()
        {
            return View();
        }
        /// <summary>
        /// Register student applications
        /// </summary>
        /// <param name="applications"></param>
        /// <returns></returns>
        // POST: Application/Create
        [HttpPost]
        public ActionResult RegisterApplication(Applications applications)
        {
            var files = Request.Files;
            try
            {
                HttpPostedFileBase photoFile = files[0];
                if (photoFile != null && photoFile.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        photoFile.InputStream.CopyTo(memoryStream);
                        applications.Photo = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a photo file.";
                    return View(applications);
                }

                HttpPostedFileBase resumeFile = files[1];
                if (resumeFile != null && resumeFile.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        resumeFile.InputStream.CopyTo(memoryStream);
                        applications.Resume = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a resume file.";
                    return View(applications);
                }

                if (_applicationDAL.InsertApplication(applications))
                {
                    TempData["SuccessMessage"] = "Application submitted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to submit the application.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(applications);
            }
        }
        // GET: Application/Edit/5
        public ActionResult UpdateApplication(int id)
        {
            var applications = _applicationDAL.GetApplicationByID(id).FirstOrDefault();
            if (applications == null)
            {
                TempData["ErrorMessage"] = "Application not avialable with id :" + id.ToString();
                return RedirectToAction("Index");
            }
            return View(applications);
        }
        // POST: Application/Edit/5
        [HttpPost]
        public ActionResult UpdateApplication(Applications applications)
        {
            var files = Request.Files;
            try
            {
                HttpPostedFileBase photoFile = files[0];
                if (photoFile != null && photoFile.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        photoFile.InputStream.CopyTo(memoryStream);
                        applications.Photo = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a photo file.";
                    return View(applications);
                }
                HttpPostedFileBase resumeFile = files[1];
                if (resumeFile != null && resumeFile.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        resumeFile.InputStream.CopyTo(memoryStream);
                        applications.Resume = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a resume file.";
                    return View(applications);
                }

                if (_applicationDAL.Update(applications))
                {
                    TempData["SuccessMessage"] = "Application updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update the details.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(applications);
            }
        }

        // GET: Application/Delete/5
        public ActionResult DeleteApplication(int id)
        {
            try
            {
                var application = _applicationDAL.GetApplicationByID(id).FirstOrDefault();
                if (application == null)
                {
                    TempData["ErrorMessage"] = "Application not avialable with id :" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(application);
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = exception.Message;
                return View();
            }
        }
        // POST: Application/Delete/5
        [HttpPost, ActionName("DeleteApplication")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = _applicationDAL.DeleteApplication(id);
                if (result.Contains("Deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = exception.Message;
                return View();
            }
        }
        public FileResult DownloadResume(int id)
        {
            var applications = _applicationDAL.GetApplicationByID(id).FirstOrDefault();

            if (applications != null && applications.Resume != null)
            {
                return File(applications.Resume, "application/pdf", "Resume.pdf");
            }
            TempData["ErrorMessage"] = "Resume file not found.";
            return null;
        }

    }
}
