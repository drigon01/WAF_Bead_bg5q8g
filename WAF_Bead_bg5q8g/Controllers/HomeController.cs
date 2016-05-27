using Newtonsoft.Json;
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
    private int mGaleryPosition = 0;


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

    public ActionResult Articles()
    {
      
      var wSerializedData = JsonConvert.SerializeObject(mEntities.Articles.OrderBy(article => article.Date).ToList());
      return new JsonResult
      {
        RecursionLimit = 1,
        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        Data = wSerializedData
      };
    }

    public ActionResult Archive()
    {
      mArchiveStart = 0;
      mGaleryPosition = 0;
      ViewBag.Message = "Here You may Browse the archived Articles";
      var wArchivedArticles = mEntities.Articles.OrderBy(article => article.Date).Skip(10).ToList();
      ViewBag.ArchiveLength = wArchivedArticles.Count / 20;
      return View("Archive", wArchivedArticles.Take(20).ToList());
    }

    public ActionResult ArchiveStep(int step)
    {
      mArchiveStart = (step * 20);
      mGaleryPosition = 0;
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10 + mArchiveStart).Take(20).ToList());
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "You may contact the news agency on the below adresses:";
      return View();
    }

    public ActionResult Article(Guid articleId)
    {
      mGaleryPosition = 0;
      var wArticle = mEntities.Articles.Where(article => article.Id == articleId).FirstOrDefault();
      ViewBag.ArticleImageId = mEntities.Images.Where(image => image.News_Id == articleId).Select(image => image.Id).ToList().FirstOrDefault();
      return View("Article", wArticle);
    }

    public ActionResult Galery(Guid articleId, int step)
    {
      var wIamges = mEntities.Images.Where(image => image.News_Id == articleId).ToList();

      mGaleryPosition += (mGaleryPosition + step) < 0 ? wIamges.Count : step;

      return View("Galery", wIamges[mGaleryPosition]);
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
      if (Id == null) { return File(Stream.Null, "image/jpg"); }
      Byte[] wImageContent = mEntities.Images.Where(image => image.Id == Id).Select(image => image.Image1).FirstOrDefault();

      if (wImageContent == null) { return File(Stream.Null, "image/jpg"); }

      return File(wImageContent, "image/jpg");
    }
  }
}