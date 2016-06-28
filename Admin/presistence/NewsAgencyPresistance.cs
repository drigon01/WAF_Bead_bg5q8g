using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
      return await UpdateArticleAsync(article);
    }

    public async Task<bool> UpdateArticleAsync(Article article)
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.PutAsJsonAsync("home/article", article);
        return wResponse.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        throw new Exception("error while updating article", ex);
      }
    }

    public async Task<bool> CreateArticleImageAsync(Image image)
    {
      try
      {
        HttpResponseMessage wResponse = await mClient.PutAsJsonAsync("home/CreateImage/", image); // elküldjük a képet
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
        HttpResponseMessage response = await mClient.DeleteAsync("home/article/" + article.Id);
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
        var wFormContent = new FormUrlEncodedContent(new[]
        {
          new KeyValuePair<string, string>("User", userName),
          new KeyValuePair<string, string>("Password", userPassword)
        });

        HttpResponseMessage wResponse = await mClient.PostAsync("/account/login?returnUrl=index", wFormContent);
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
        HttpResponseMessage wResponse = await mClient.GetAsync("/account/logout");
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
        HttpResponseMessage wResponse = await mClient.GetAsync("/home/articles");
        if (wResponse.IsSuccessStatusCode) // amennyiben sikeres a művelet
        {
          var wRawData = (await wResponse.Content.ReadAsStringAsync()).ToString().Replace("\\", "").Trim('\"').ToString();
          var wArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>(wRawData);
          // a tartalmat JSON formátumból objektumokká alakítjuk

          // képek lekérdezése:
          foreach (Article wArticle in wArticles)
          {

            wResponse = await mClient.GetAsync(string.Format("home/GetGalery?articleId={0}", wArticle.Id));
            if (wResponse.IsSuccessStatusCode)
            {
              wRawData = (await wResponse.Content.ReadAsStringAsync()).ToString().Replace("\\", "").Trim('\"').ToString();
              wArticle.Images = JsonConvert.DeserializeObject<IList<Image>>(wRawData);
            }
          }
          return wArticles as IEnumerable<Article>;
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
  }
}
