using Service.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Admin.model;

namespace Admin.ViewModel
{
  /// <summary>
  /// A nézetmodell típusa.
  /// </summary>
  public class MainViewModel : ViewModelBase
  {
    private INewsAgencyModel mModel;
    private ObservableCollection<Article> mArticles;
    private Article mSelectedArticle;
    private Boolean mIsLoaded;

    /// <summary>
    /// Query articles;
    /// </summary>
    public ObservableCollection<Article> Articles
    {
      get { return mArticles; }
      private set
      {
        if (mArticles != value)
        {
          mArticles = value;
          OnPropertyChanged();
        }
      }
    }
    /// <summary>
    /// Betöltöttség lekérdezése.
    /// </summary>
    public Boolean IsLoaded
    {
      get { return mIsLoaded; }
      private set
      {
        if (mIsLoaded != value)
        {
          mIsLoaded = value;
          OnPropertyChanged();
        }
      }
    }

    public Article SelectedArticle
    {
      get { return mSelectedArticle; }
      set
      {
        if (mSelectedArticle != value)
        {
          mSelectedArticle = value;
          OnPropertyChanged();
        }
      }
    }

    public Article EditedArticle { get; private set; }

    public DelegateCommand CreateArticleCommand { get; private set; }
    public DelegateCommand CreateImageCommand { get; private set; }
    public DelegateCommand UpdateArticleCommand { get; private set; }
    public DelegateCommand DeleteArticleCommand { get; private set; }
    public DelegateCommand SaveChangesCommand { get; private set; }
    public DelegateCommand CancelChangesCommand { get; private set; }
    public DelegateCommand ExitCommand { get; private set; }
    public DelegateCommand LoadCommand { get; private set; }
    public DelegateCommand SaveCommand { get; private set; }

    public event EventHandler ExitApplication;
    public event EventHandler ArticleEditingStarted;
    public event EventHandler ArticleEditingFinished;
    public event EventHandler<ArticleChangeArgs> ImageAttachmentStarted;


    /// <summary>
    /// Nézetmodell példányosítása.
    /// </summary>
    /// <param name="model">A modell.</param>
    public MainViewModel(INewsAgencyModel model)
    {
      if (model == null)
        throw new ArgumentNullException("model");

      mModel = model;
      mModel.ArticleChanged += Model_ArticleChanged;
      mIsLoaded = false;

      CreateArticleCommand = new DelegateCommand(param =>
      {
        EditedArticle = new Article();
        OnArticleEditingStarted();
      });
      CreateImageCommand = new DelegateCommand(param => { if ((param as Article) == null) { return; } OnImageAttachment((param as Article).Id); });
      UpdateArticleCommand = new DelegateCommand(param => UpdateArticle(param as Article));
      DeleteArticleCommand = new DelegateCommand(param => DeleteArticle(param as Article));
      SaveChangesCommand = new DelegateCommand(param => SaveChanges());
      CancelChangesCommand = new DelegateCommand(param => CancelChanges());
      LoadCommand = new DelegateCommand(param => LoadAsync());
      SaveCommand = new DelegateCommand(param => SaveAsync());
      ExitCommand = new DelegateCommand(param => OnExitApplication());
    }

    /// <summary>
    /// Épület frissítése.
    /// </summary>
    /// <param name="article">Az épület.</param>
    private void UpdateArticle(Article article)
    {
      if (article == null)
        return;

      EditedArticle = new Article
      {
        Id = article.Id,
        Title = article.Title,
        Content = article.Content,
        Summary = article.Summary,
        Images = article.Images,
        IsLead = article.IsLead,
      }; // a szerkesztett épület adatait áttöltjük a kijelöltből

      OnArticleEditingStarted();
    }

    /// <summary>
    /// Épület törlése.
    /// </summary>
    /// <param name="Article">Az épület.</param>
    private void DeleteArticle(Article Article)
    {
      if (Article == null || !Articles.Contains(Article))
        return;

      Articles.Remove(Article);

      mModel.DeleteArticle(Article);
    }

    /// <summary>
    /// Változtatások mentése.
    /// </summary>
    private void SaveChanges()
    {
      if (String.IsNullOrEmpty(EditedArticle.Title))
      {
        OnMessageApplication("No title defined!");
        return;
      }
      if (EditedArticle.Summary == null)
      {
        OnMessageApplication("Summary missing.");
        return;
      }
      if (EditedArticle.Content == null)
      {
        OnMessageApplication("The article should have some content...");
        return;
      }

      if (EditedArticle.Id == Guid.Empty)
      {
        mModel.AddArticle(EditedArticle);
        Articles.Add(EditedArticle);
        SelectedArticle = EditedArticle;
      }
      else
      {
        mModel.UpdateArticle(EditedArticle);
      }

      EditedArticle = null;

      OnArticleEditingFinished();
    }

    private void CancelChanges()
    {
      EditedArticle = null;
      OnArticleEditingFinished();
    }

    private async void LoadAsync()
    {
      try
      {
        await mModel.LoadAsync();
        Articles = new ObservableCollection<Article>(mModel.Articles);
        IsLoaded = true;
      }
      catch (Exception)
      {
        OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
      }
    }

    private async void SaveAsync()
    {
      try
      {
        await mModel.SaveAsync();
        OnMessageApplication("A mentés sikeres!");
      }
      catch (Exception)
      {
        OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
      }
    }

    private void Model_ArticleChanged(object sender, ArticleChangeArgs e)
    {
      Int32 index = Articles.IndexOf(Articles.FirstOrDefault(Article => Article.Id == e.ArticleId));
      Articles.RemoveAt(index); // módosítjuk a kollekciót
      Articles.Insert(index, mModel.Articles[index]);

      SelectedArticle = Articles[index]; // és az aktuális épületet
    }

    private void OnExitApplication()
    {
      ExitApplication?.Invoke(this, EventArgs.Empty);
    }

    private void OnArticleEditingStarted()
    {
      ArticleEditingStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnArticleEditingFinished()
    {
      ArticleEditingFinished?.Invoke(this, EventArgs.Empty);
    }

    private void OnImageAttachment(Guid id)
    {
      ImageAttachmentStarted?.Invoke(this, new ArticleChangeArgs { ArticleId = id });
    }
  }
}
