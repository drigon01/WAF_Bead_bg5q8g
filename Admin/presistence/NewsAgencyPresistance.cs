using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<bool> CreateArticleAsync(Article article)
    {
      try
      {
        HttpResponseMessage response = await mClient.PostAsJsonAsync("api/articles/", article); // az értékeket azonnal JSON formátumra alakítjuk
        article.Id = (await response.Content.ReadAsAsync<Article>()).Id; // a válaszüzenetben megkapjuk a végleges azonosítót
        return response.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("error wile creating article", ex);
      }
    }

    public async Task<bool> CreateArticleImageAsync(Image image)
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.PostAsJsonAsync("api/images/", image); // elküldjük a képet
        if (wResponse.IsSuccessStatusCode)
        {
          image.Id = await wResponse.Content.ReadAsAsync<Guid>(); // a válaszüzenetben megkapjuk az azonosítót
        }
        return wResponse.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("Image creation failed!", ex);
      }
    }

    public async Task<bool> DeleteArticleAsync(Article article)
    {
      try
      {
        HttpResponseMessage response = await mClient.DeleteAsync("api/buildings/" + article.Id);
        return response.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("Error deleting article", ex);
      }
    }

    public async Task<bool> LoginAsync(string userName, string userPassword)
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.GetAsync("api/account/login/" + userName + "/" + userPassword);
        return wResponse.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
      }
      catch (Exception ex)
      {
        throw new Exception("Error Logging in", ex);
      }
    }

    public async Task<bool> LogoutAsync()
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.GetAsync("api/account/logout");
        return !wResponse.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("Error Logging Out!", ex);
      }
    }

    public async Task<IEnumerable<Article>> ReadAriclesAsync()
    {
      try
      {
        // a kéréseket a kliensen keresztül végezzük
        HttpResponseMessage wResponse = await mClient.GetAsync("api/buildings/");
        if (wResponse.IsSuccessStatusCode) // amennyiben sikeres a művelet
        {
          IEnumerable<Article> wArticles = await wResponse.Content.ReadAsAsync<IEnumerable<Article>>();
          // a tartalmat JSON formátumból objektumokká alakítjuk

          // képek lekérdezése:
          foreach (Article wArticle in wArticles)
          {
            wResponse = await mClient.GetAsync("api/images/articles/" + wArticle.Id);
            if (wResponse.IsSuccessStatusCode)
            {
              wArticle.Images = (await wResponse.Content.ReadAsAsync<IEnumerable<Image>>()).ToList();
            }
          }

          return wArticles;
        }
        else
        {
          throw new Exception("Service returned response: " + wResponse.StatusCode);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Articles could not be read!", ex);
      }

    }

    public Task<IEnumerable<Image>> ReadImagesAsync()
    {
      throw new NotImplementedException();
    }

    public async Task<bool> UpdateArticleAsync(Article article)
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.PutAsJsonAsync("api/article/", article);
        return wResponse.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("error while updating article", ex);
      }
    }
  }
}
