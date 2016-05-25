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
    private NewsAgencyPresistance newsAgencyPresistance;

    public NewsAgencyModel(NewsAgencyPresistance newsAgencyPresistance)
    {
      this.newsAgencyPresistance = newsAgencyPresistance;
    }

    public IList<Article> Articles
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsUserLoggedIn
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public event EventHandler<ArticleChangeArgs> ArticleChanged;

    public void AddArticle(Article article)
    {
      throw new NotImplementedException();
    }

    public void CreateImage(Guid article, byte[] image)
    {
      throw new NotImplementedException();
    }

    public void DeleteArticle(Article article)
    {
      throw new NotImplementedException();
    }

    public Task LoadAsync()
    {
      throw new NotImplementedException();
    }

    public Task<bool> LoginAsync(string userName, string userPassword)
    {
      throw new NotImplementedException();
    }

    public Task<bool> LogoutAsync()
    {
      throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
      throw new NotImplementedException();
    }

    public void UpdateArticle(Article article)
    {
      throw new NotImplementedException();
    }
  }
}
