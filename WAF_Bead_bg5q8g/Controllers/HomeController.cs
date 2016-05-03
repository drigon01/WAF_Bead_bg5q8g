using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WAF_Bead_bg5q8g.Controllers
{
  public class HomeController : Controller
  {
    private News_PortalEntities mEntities;

    public HomeController()
    {
      mEntities = new News_PortalEntities();

      ViewBag.News = mEntities.Articles.ToArray();
    }

    public ActionResult Index()
    {
      return View("Index", mEntities.Articles.ToList());
    }

    public ActionResult Archive()
    {
      ViewBag.Message = "Here You may Browse the archived Articles";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "You may contact the news agency on the below adresses:";

      return View();
    }

    /// <summary>
    /// Query first imasge for article.
    /// </summary>
    /// <param name="articleId">article id</param>
    /// <returns>the image associated with the article, or a default one</returns>
    public FileResult ArticelImage(Guid articleId)
    {
      if (articleId == null) { return File("~/Content/missing.jpg", "image/png"); }
      Byte[] wImageContent = mEntities.Images.Where(image => image.Id == articleId).Select(image => image.Image1).FirstOrDefault();

      if (wImageContent == null) { return File("~/Content/missing.jpg", "image/png"); }

      return File(wImageContent, "image/png");
    }
  }
}