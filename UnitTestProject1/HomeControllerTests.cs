using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WAF_Bead_bg5q8g.Controllers;
using Service.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace UnitTestProject1
{
  [TestClass]
  public class HomeControllerTests
  {
    private HomeController mController;

    [TestInitialize]
    public void InitializeTests()
    {
      mController = new HomeController();
    }

    [TestMethod]
    public void ControllerHasData()
    {
      var wNews = mController.ViewBag.News;
      Assert.IsNotNull(wNews);
    }

    [TestMethod]
    public void ControllerDataIsVAlid()
    {
      var wNews = mController.ViewBag.News;
      Assert.IsNotNull(wNews as Article[]);
    }

    [TestMethod]
    public void ArticlesAreQueryable()
    {
      var wArticles = mController.Articles() as JsonResult;
      Assert.IsNotNull(wArticles);
    }


    [TestMethod]
    public void ArticlesAreSerializedProperly()
    {
      var wSerializedArticles = mController.Articles() as JsonResult;
      var wArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>(wSerializedArticles.Data.ToString());
      Assert.IsNotNull(wArticles);
      foreach (var wArticle in wArticles)
      {
        Assert.IsNotNull(wArticle);
        Assert.IsTrue(wArticle.Id != Guid.Empty);
      }
    }

    [TestMethod]
    public void IndexReturnsView()
    {
      var wInedxPage = mController.Index() as ViewResult;

      Assert.IsNotNull(wInedxPage.ViewData);
      Assert.AreEqual("Index", wInedxPage.ViewName);
    }

    [TestMethod]
    public void IndexHasEntitiesFromDB()
    {
      var wInedxPage = mController.Index() as ViewResult;
      IEnumerable<Article> wArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Take(10).ToList();
      Assert.AreEqual(wArticles.Count(), (wInedxPage.Model as IEnumerable<Article>).Count());
    }


    [TestMethod]
    public void ArchiveReturnsView()
    {
      var wArchive = mController.Archive() as ViewResult;

      Assert.IsNotNull(wArchive.ViewData);
      Assert.AreEqual("Archive", wArchive.ViewName);
    }

    [TestMethod]
    public void ArrchiveHasEntitiesFromDB()
    {
      var wArchive = mController.Archive() as ViewResult;
      IEnumerable<Article> wArchivedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Skip(10).ToList().Take(20).ToList();
      Assert.AreEqual(wArchivedArticles.Count(), (wArchive.Model as IEnumerable<Article>).Count());
    }

    [TestMethod]
    public void ArchiveStepEqualsToArchive()
    {
      var wArchiveStep = mController.ArchiveStep(0) as ViewResult;
      var wArchive = mController.Archive() as ViewResult;
      Assert.AreEqual((wArchive.Model as IEnumerable<Article>).FirstOrDefault().Id,
        (wArchiveStep.Model as IEnumerable<Article>).FirstOrDefault().Id);
    }


    [TestMethod, Ignore]
    public void ArchiveStepNotEqualsToArchive()
    {
      var wArchive = mController.Archive() as ViewResult;
      var wArchiveStep = mController.ArchiveStep(1) as ViewResult;

      Assert.AreNotEqual((wArchive.Model as IEnumerable<Article>).FirstOrDefault().Title,
        (wArchiveStep.Model as IEnumerable<Article>).FirstOrDefault().Title);
    }


    [TestMethod]
    public void ContactHasView()
    {
      var wContact = mController.Contact() as ViewResult;
      Assert.IsNotNull(wContact);
    }


    [TestMethod]
    public void ArticleDetailHasView()
    {
      IEnumerable<Article> wArchivedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Skip(10).ToList().Take(20).ToList();

      var wArticle = mController.Article(wArchivedArticles.ElementAt(2).Id) as ViewResult;
      Assert.IsNotNull(wArticle);
    }

    [TestMethod]
    public void ArticleDetail()
    {
      IEnumerable<Article> wArchivedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Skip(10).ToList().Take(20).ToList();

      var wArticle = mController.Article(wArchivedArticles.ElementAt(2).Id) as ViewResult;
      Assert.AreEqual((wArticle.Model as Article).Title, wArchivedArticles.ElementAt(2).Title);
    }


    [TestMethod, Ignore]
    public void UpdateArticle()
    {
      var wArticle = JsonConvert.DeserializeObject<List<Article>>((mController.Articles() as JsonResult).Data.ToString());

      new System.IO.StreamWriter(mController.Request.InputStream).Write(wArticle[7]);

      var wUpdatedArticle = mController.Article();

      Assert.IsNull(wUpdatedArticle);
    }

    [TestMethod]
    public void GAleryHasView()
    {
      IEnumerable<Article> wArchivedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Skip(10).ToList().Take(20).ToList();

      var wGalery = mController.Galery(wArchivedArticles.ElementAt(2).Id, 0) as ViewResult;
      Assert.IsNotNull(wGalery);
      Assert.AreEqual((wGalery as ViewResult).ViewName, "Galery");
    }

    [TestMethod]
    public void GAleryempty()
    {
      IEnumerable<Article> wArchivedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>((mController.Articles() as JsonResult).Data.ToString()).Skip(10).ToList().Take(20).ToList();

      var wGalery = mController.Article(wArchivedArticles.ElementAt(2).Id) as ViewResult;
      Assert.IsNotNull((wGalery as ViewResult).Model);
    }
  }
}
