using System;

namespace Admin.model
{
  public class ArticleChangeArgs : EventArgs
  {
    public Guid ArticleId { get; set; }
  }
}