using Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.presistence
{
  interface INewsAgencyPresistance
  {
    Task<IEnumerable<Article>> ReadAriclesAsync();

    Task<Boolean> CreateArticleAsync(Article article);

    Task<Boolean> UpdateArticleAsync(Article article);

    Task<Boolean> DeleteArticleAsync(Article article);

    Task<Boolean> LoginAsync(String userName, String userPassword);

    Task<Boolean> LogoutAsync();
  }
}
