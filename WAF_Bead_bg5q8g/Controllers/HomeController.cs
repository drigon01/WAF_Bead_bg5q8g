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

    public HomeController()
    {
      mEntities = new News_PortalEntities();
      ViewBag.News = mEntities.Articles.ToArray();
    }

    public ActionResult Index()
    {
      return View("Index", mEntities.Articles.OrderBy(article => article.Date).Take(10).ToList());
    }

    public ActionResult Archive()
    {
      ViewBag.Message = "Here You may Browse the archived Articles";
      return View("Archive", mEntities.Articles.OrderBy(article => article.Date).Skip(10).ToList());
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

    /// <summary>
    /// Query image for article.
    /// </summary>
    /// <param name="articleId">article id</param>
    /// <returns>the image associated with the article, or a default one</returns>
    public FileResult ArticelImage(Guid articleId)
    {
      if (articleId == null) { return File("~/Content/missing.jpg", "image/jpg"); }
      Byte[] wImageContent = mEntities.Images.Where(image => image.Id == articleId).Select(image => image.Image1).FirstOrDefault();

      if (wImageContent == null) { return File("~/Content/missing.jpg", "image/jpg"); }

      return File(wImageContent, "image/png");
    }

    /// <summary>
    /// Épület egyik képének lekérdezése.
    /// </summary>
    /// <param name="imageId">Kép azonosítója.</param>
    /// <param name="isLarge">Nagy méretű kép lekérése.</param>
    /// <returns>Az épület egy képe, vagy az alapértelmezett kép.</returns>
    public FileResult Image(Guid? imageId, Boolean isLarge = false)
    {
      if (imageId == null) // nem adtak meg azonosítót
        return File("~/Content/missing.jpg", "image/jpg");

      // lekérjük a megadott azonosítóval rendelkező képet
      byte[] imageContent = mEntities.Images.Where(image => image.Id == imageId).Select(image => isLarge ? image.Image1 : CreateThumbnail(image.Image1, 100)).FirstOrDefault();

      if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
        return File("~/Content/missing.jpg", "image/jpg");

      return File(imageContent, "image/jpg");
    }


    //Referenced from : https://social.msdn.microsoft.com/Forums/vstudio/en-US/e2d8bd99-1af4-404a-a6bd-5fae9f540c2d/how-to-resize-an-image-in-byte-?forum=csharpgeneral
    // (RESIZE an image in a byte[] variable.)   should move to separatae class
    #region image manipulation 
    private static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
    {
      byte[] ReturnedThumbnail;

      using (MemoryStream StartMemoryStream = new MemoryStream(),
                          NewMemoryStream = new MemoryStream())
      {
        // write the string to the stream  
        StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

        // create the start Bitmap from the MemoryStream that contains the image  
        Bitmap startBitmap = new Bitmap(StartMemoryStream);

        // set thumbnail height and width proportional to the original image.  
        int newHeight;
        int newWidth;
        double HW_ratio;
        if (startBitmap.Height > startBitmap.Width)
        {
          newHeight = LargestSide;
          HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
          newWidth = (int)(HW_ratio * (double)startBitmap.Width);
        }
        else
        {
          newWidth = LargestSide;
          HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
          newHeight = (int)(HW_ratio * (double)startBitmap.Height);
        }

        // create a new Bitmap with dimensions for the thumbnail.  
        Bitmap newBitmap = new Bitmap(newWidth, newHeight);

        // Copy the image from the START Bitmap into the NEW Bitmap.  
        // This will create a thumnail size of the same image.  
        newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

        // Save this image to the specified stream in the specified format.  
        newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        // Fill the byte[] for the thumbnail from the new MemoryStream.  
        ReturnedThumbnail = NewMemoryStream.ToArray();
      }

      // return the resized image as a string of bytes.  
      return ReturnedThumbnail;
    }

    // Resize a Bitmap  
    private static Bitmap ResizeImage(Bitmap image, int width, int height)
    {
      Bitmap resizedImage = new Bitmap(width, height);
      using (Graphics gfx = Graphics.FromImage(resizedImage))
      {
        gfx.DrawImage(image, new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
      }
      return resizedImage;
    }

    #endregion



  }
}