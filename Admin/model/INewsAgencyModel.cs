using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Models;

namespace Admin.model
{
  /// <summary>
  /// Model of the news agency
  /// </summary>
  public interface INewsAgencyModel
  {
    /// <summary>
    /// Querry of Articles
    /// </summary>
    IReadOnlyList<Article> Articles { get; }

    /// <summary>
    /// Login State.
    /// </summary>
    Boolean IsUserLoggedIn { get; }

    /// <summary>
    /// Event of article change
    /// </summary>
    event EventHandler<ArticleChangeArgs> ArticleChanged;

    /// <summary>
    /// Creat new Article.
    /// </summary>
    /// <param name="article">the article to be added.</param>
    void AddArticle(Article article);

    /// <summary>
    /// Kép létrehozása.
    /// </summary>
    /// <param name="ArticleId">Épület azonosító.</param>
    /// <param name="image">Kép.</param>
    void CreateImage(Guid article,  Byte[] image);

    /// <summary>
    /// Update Article
    /// </summary>
    /// <param name="article"></param>
    void UpdateArticle(Article article);

    /// <summary>
    /// Delete Article.
    /// </summary>
    /// <param name="Article">Az épület.</param>
    void DeleteArticle(Article article);

    /// <summary>
    /// Adatok betöltése.
    /// </summary>
    Task LoadAsync();

    /// <summary>
    /// Adatok mentése.
    /// </summary>
    Task SaveAsync();

    /// <summary>
    /// Bejelentkezés.
    /// </summary>
    /// <param name="userName">Felhasználónév.</param>
    /// <param name="userPassword">Jelszó.</param>
    Task<Boolean> LoginAsync(String userName, String userPassword);

    /// <summary>
    /// Kijelentkezés.
    /// </summary>
    Task<Boolean> LogoutAsync();
  }
}
