using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WAF_Bead_bg5q8g.Controllers
{
  public class HomeController : Controller
  {
    private News_PortalEntities mEntities;
    private int mArchiveStart = 0;
    private static int mGaleryPosition = 0;


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
      ViewBag.ArchiveLength = ((wArchivedArticles.Count / 20) + (wArchivedArticles.Count % 20 == 0 ? 0 : 1));
      return View("Archive", wArchivedArticles.Take(20).ToList());
    }

    public ActionResult ArchiveStep(int step)
    {
      mArchiveStart = (step * 20);
      mGaleryPosition = 0;
      var wArchivedArticles = mEntities.Articles.OrderBy(article => article.Date).Skip(10).ToList();
      ViewBag.ArchiveLength = ((wArchivedArticles.Count / 20) + (wArchivedArticles.Count % 20 == 0 ? 0 : 1));
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10 + mArchiveStart).ToList().Take(20).ToList());
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
      if (!wArticle.Date.HasValue) { wArticle.Date = DateTime.Now; }
      return View("Article", wArticle);
    }

    [HttpPut]
    public ActionResult Article()
    {
      Stream wRequestStream = Request.InputStream;
      wRequestStream.Seek(0, SeekOrigin.Begin);
      string wJson = new StreamReader(wRequestStream).ReadToEnd();

      Article wArticle = null;
      try
      {
        wArticle = JsonConvert.DeserializeObject<Article>(wJson);
      }
      catch (Exception ex)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
      }

      Article wArticleToUpdate = mEntities.Articles.FirstOrDefault(article => article.Id == wArticle.Id);

      if (wArticleToUpdate == null)
      {
        var wNewArticle = new Article();
        wNewArticle.Accounts = wNewArticle.Accounts ?? new Account { Id = Guid.NewGuid() };
        wNewArticle.Date = DateTime.Now;
        wNewArticle.Images = new List<Service.Models.Image>();
        wNewArticle.Id = Guid.NewGuid();
        mEntities.Articles.Add(wNewArticle);
        wArticleToUpdate = wNewArticle;
      }

      wArticleToUpdate.Content = wArticle.Content.Substring(0, Math.Min(wArticle.Content.Length, 499));
      wArticleToUpdate.Summary = wArticle.Summary.Substring(0, Math.Min(wArticle.Summary.Length, 99));
      wArticleToUpdate.Title = wArticle.Title.Substring(0, Math.Min(wArticle.Title.Length, 99));
      wArticleToUpdate.IsLead = (bool)wArticle.IsLead;

      wArticleToUpdate.Summary.Replace(" ", "");

      wArticleToUpdate.Images = (IList<Service.Models.Image>)wArticle.Images;

      try
      {
        mEntities.SaveChanges();
      }
      catch (DbEntityValidationException dbEx)
      {
        foreach (var validationErrors in dbEx.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
          }
        }
      }
      catch (Exception wE)
      {
        Debugger.Log(1, "error", wE.Message);
      }
      return null;
    }

    public ActionResult Galery(Guid articleId, int step)
    {
      ViewBag.articleId = articleId;
      var wIamges = mEntities.Images.Where(image => image.News_Id == articleId).ToList();

      mGaleryPosition += ((mGaleryPosition + step) < 0 ? wIamges.Count - 1 : step);

      ViewBag.EnlargedIndex = mGaleryPosition;

      return View("Galery", wIamges);
    }
    [HttpPut]
    public ActionResult CreateImage()
    {
      Stream wRequestStream = Request.InputStream;
      wRequestStream.Seek(0, SeekOrigin.Begin);
      string wJson = new StreamReader(wRequestStream).ReadToEnd();

      Service.Models.Image wImage = null;
      try
      {
        wImage = JsonConvert.DeserializeObject<Service.Models.Image>(wJson);
      }
      catch (Exception ex)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
      }

      if (wImage != null && wImage.Image1 != null)
      {
        var wArticle = mEntities.Articles.FirstOrDefault(a => a.Id == wImage.News_Id);
        wImage.Article = wArticle;
        wArticle.Images.Add(wImage);
        mEntities.Images.Add(wImage);
      }

      mEntities.SaveChanges();

      var wJsonresult = new JsonResult();
      wJsonresult.Data = wImage.Id;
      return wJsonresult;
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

    [HttpDelete]
    public ActionResult Article(string id)
    {
      var wId = Guid.Parse(id);

      mEntities.Articles.Remove(mEntities.Articles.FirstOrDefault(article => article.Id == wId));
      mEntities.SaveChanges();
      return null;
    }
  }
}