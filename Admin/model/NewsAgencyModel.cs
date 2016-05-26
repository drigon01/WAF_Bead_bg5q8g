using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.presistence;
using Service.Models;

namespace Admin.model
{
  class NewsAgencyModel : INewsAgencyModel
  {

    private enum DataFlag
    {
      Create,
      Update,
      Delete
    }
    private List<Article> mArticles;
    private NewsAgencyPresistance mNewsAgencyPresistance;
    private Dictionary<Article, DataFlag> mArticleFlags;
    private Dictionary<Image, DataFlag> mImageFlags;

    public NewsAgencyModel(NewsAgencyPresistance presistance)
    {
      IsUserLoggedIn = false;
      mNewsAgencyPresistance = presistance;
    }

    public IReadOnlyList<Article> Articles { get { return mArticles; } }

    public bool IsUserLoggedIn { get; private set; }

    public event EventHandler<ArticleChangeArgs> ArticleChanged;

    public void AddArticle(Article article)
    {
      if (article == null)
        throw new ArgumentNullException("article");
      if (mArticles.Contains(article))
        throw new ArgumentException("The article is already in the collection.", "article");

      article.Id = Guid.NewGuid();// generálunk egy új, ideiglenes azonosítót (nem fog átkerülni a szerverre)
      mArticleFlags.Add(article, DataFlag.Create);
      mArticles.Add(article);
    }

    public void CreateImage(Guid articleID, byte[] image)
    {
      Article wArticle = mArticles.FirstOrDefault(b => b.Id == articleID);
      if (wArticle == null)
        throw new ArgumentException("The article does not exist.", "articleID");

      // létrehozzuk a képet
      Image wImage = new Image
      {
        Id = Guid.NewGuid(),
        News_Id = articleID,
        Image1 = image
      };

      // hozzáadjuk
      wArticle.Images.Add(wImage);
      mImageFlags.Add(wImage, DataFlag.Create);

      // jellezzük a változást
      OnArticleChanged(wArticle.Id);
    }

    public void DeleteArticle(Article article)
    {
      if (article == null)
        throw new ArgumentNullException("building");
      if (!mArticles.Contains(article))
        throw new ArgumentException("The building does not exist.", "building");

      // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
      if (mArticleFlags.ContainsKey(article) && mArticleFlags[article] == DataFlag.Create)
        mArticleFlags.Remove(article);
      else
        mArticleFlags[article] = DataFlag.Delete;

      mArticleFlags.Remove(article);
    }

    public async Task LoadAsync()
    {
      // adatok
      mArticles = (await mNewsAgencyPresistance.ReadAriclesAsync()).ToList();

      // állapotjelzések
      mArticleFlags = new Dictionary<Article, DataFlag>();
      mImageFlags = new Dictionary<Image, DataFlag>();
    }

    public async Task<bool> LoginAsync(string userName, string userPassword)
    {
      IsUserLoggedIn = await mNewsAgencyPresistance.LoginAsync(userName, userPassword);
      return IsUserLoggedIn;
    }

    public async Task<bool> LogoutAsync()
    {
      if (!IsUserLoggedIn)
        return true;

      IsUserLoggedIn = !(await mNewsAgencyPresistance.LogoutAsync());

      return IsUserLoggedIn;
    }

    public async Task SaveAsync()
    {
      // épületek
      List<Article> wArticlesToSave = mArticleFlags.Keys.ToList();

      foreach (Article wArticle in wArticlesToSave)
      {
        Boolean result = true;

        // az állapotjelzőnek megfelelő műveletet végezzük el
        switch (mArticleFlags[wArticle])
        {
          case DataFlag.Create:
            result = await mNewsAgencyPresistance.CreateArticleAsync(wArticle);
            break;
          case DataFlag.Delete:
            result = await mNewsAgencyPresistance.DeleteArticleAsync(wArticle);
            break;
          case DataFlag.Update:
            result = await mNewsAgencyPresistance.UpdateArticleAsync(wArticle);
            break;
        }

        if (!result)
          throw new InvalidOperationException("Operation " + mArticleFlags[wArticle] + " failed on building " + wArticle.Id);

        // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
        mArticleFlags.Remove(wArticle);
      }

      // képek
      List<Image> imagesToSave = mImageFlags.Keys.ToList();

      foreach (Image image in imagesToSave)
      {
        Boolean result = true;

        switch (mImageFlags[image])
        {
          case DataFlag.Create:
            result = await mNewsAgencyPresistance.CreateArticleImageAsync(image);
            break;
          case DataFlag.Delete:
            throw new NotImplementedException("Can't delete image!");
        }

        if (!result)
          throw new InvalidOperationException("Operation " + mImageFlags[image] + " failed on image " + image.Id);

        // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
        mImageFlags.Remove(image);
      }

    }

    public void UpdateArticle(Article article)
    {
      if (article == null)
        throw new ArgumentNullException("building");

      // keresés azonosító alapján
      Article wArticle = Articles.FirstOrDefault(b => b.Id == article.Id);

      if (wArticle == null)
        throw new ArgumentException("The building does not exist.", "building");

      // módosítások végrehajtása
      wArticle.Content = article.Content;
      wArticle.Summary = article.Summary;
      wArticle.Title = article.Title;
      wArticle.IsLead = article.IsLead;

      // külön állapottal jelezzük, ha egy adat újonnan hozzávett
      if (mArticleFlags.ContainsKey(wArticle) && mArticleFlags[wArticle] == DataFlag.Create)
      {
        mArticleFlags[wArticle] = DataFlag.Create;
      }
      else
      {
        mArticleFlags[wArticle] = DataFlag.Update;
      }

      // jelezzük a változást
      OnArticleChanged(article.Id);
    }

    private void OnArticleChanged(Guid articleID)
    {
      ArticleChanged?.Invoke(this, new ArticleChangeArgs { ArticleId = articleID });
    }
  }
}
