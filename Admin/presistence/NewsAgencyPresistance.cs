using Service.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Admin.presistence
{
  class NewsAgencyPresistance : INewsAgencyPresistance
  {
    private HttpClient mClient;

    public NewsAgencyPresistance(String baseAddress)
    {
      mClient = new HttpClient();
      mClient.BaseAddress = new Uri(baseAddress);
    }

    public Task<bool> CreateArticleAsync(Article article)
    {
      throw new NotImplementedException();
    }

    public Task<bool> CreateArticleImageAsync(Image image)
    {
      throw new NotImplementedException();
    }

    public Task<bool> DeleteArticleAsync(Article article)
    {
      throw new NotImplementedException();
    }

    public Task<bool> DeleteArticleImageAsync(Image image)
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

    public Task<IEnumerable<Article>> ReadAriclesAsync()
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Image>> ReadImagesAsync()
    {
      throw new NotImplementedException();
    }

    public Task<bool> UpdateArticleAsync(Article article)
    {
      throw new NotImplementedException();
    }
  }
}
