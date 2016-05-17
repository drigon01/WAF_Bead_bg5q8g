using Service.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WAF_Bead_bg5q8g.Controllers
{
  public class HomeController : Controller
  {
    private News_PortalEntities mEntities;
    private int mArchiveStart = 0;


    public HomeController()
    {
      mEntities = new News_PortalEntities();
      ViewBag.News = mEntities.Articles.ToArray();
    }

    public ActionResult Index()
    {
      ViewBag.Lead = mEntities.Articles.Where(article => (bool)article.IsLead).FirstOrDefault();
      return View("Index", mEntities.Articles.OrderBy(article => article.Date).Take(10).ToList());
    }

    public ActionResult Archive()
    {
      mArchiveStart = 0;
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10).ToList());
    }

    public ActionResult Next()
    {
      mArchiveStart += 20;
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10 + mArchiveStart).Take(20));
    }

    public ActionResult Previous()
    {
      mArchiveStart = mArchiveStart - 20 < 10 ? 0 : mArchiveStart - 20;
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10 + mArchiveStart).ToList());
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "You may contact the news agency on the below adresses:";
      return View();
    }

    public ActionResult Article(Guid articleId)
    {
      var wArticle = mEntities.Articles.Where(article => article.Id == articleId).FirstOrDefault();
      ViewBag.Images = mEntities.Images.Where(image => image.News_Id == articleId).Select(image => image.Id).ToList();
      return View("Article", wArticle);
    }

    public ActionResult Galery(Guid articleId)
    {
      var wIamges = mEntities.Images.Where(image => image.News_Id == articleId).ToList();
      return View("Galery", wIamges);
    }


    [HttpPost]
    public ActionResult Results(string searchText)
    {
      var wList = new List<Article>();

      wList.AddRange(mEntities.Articles.Where(article => article.Accounts.name == searchText).ToList());
      wList.AddRange(mEntities.Articles.Where(article => article.Title.Contains(searchText)).ToList());
      wList.AddRange(mEntities.Articles.Where(article => article.Content.Contains(searchText)).ToList());

      return View("Index", wList);
    }

    /// <summary>
    /// Query image for article.
    /// </summary>
    /// <param name="Id">article id</param>
    /// <returns>the image associated with the article, or a default one</returns>
    public FileResult ArticleImage(Guid Id)
    {
      if (Id == null) { return File("~/Content/missing.jpg", "image/jpg"); }
      Byte[] wImageContent = mEntities.Images.Where(image => image.Id == Id).Select(image => image.Image1).FirstOrDefault();

      if (wImageContent == null) { return File("~/Content/missing.jpg", "image/jpg"); }

      return File(wImageContent, "image/jpg");
    }
  }
}