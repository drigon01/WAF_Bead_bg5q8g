using Admin.model;
using Admin.ViewModel;
using Admin.view;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Admin.presistence;

namespace Admin
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private INewsAgencyModel mModel;
    private LoginViewModel mLoginViewModel;
    private LoginWindow mLoginView;
    private MainViewModel mMainViewModel;
    private MainWindow mMainView;
    private ArticleEditorWindow mEditorView;


    public App()
    {
      Startup += new StartupEventHandler(App_Startup);
      Exit += new ExitEventHandler(App_Exit);
    }

    private void App_Startup(object sender, StartupEventArgs e)
    {
      mModel = new NewsAgencyModel(new NewsAgencyPresistance("http://localhost:51827")); // megadjuk a szolgáltatás címét

      mLoginViewModel = new LoginViewModel(mModel);
      mLoginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
      mLoginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
      mLoginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

      mLoginView = new LoginWindow();
      mLoginView.DataContext = mLoginViewModel;
      mLoginView.Show();
    }

    public async void App_Exit(object sender, ExitEventArgs e)
    {
      if (mModel.IsUserLoggedIn) // amennyiben be vagyunk jelentkezve, kijelentkezünk
      {
        await mModel.LogoutAsync();
      }
    }

    private void ViewModel_LoginSuccess(object sender, EventArgs e)
    {
      mMainViewModel = new MainViewModel(mModel);
      mMainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
      mMainViewModel.ArticleEditingStarted += new EventHandler(MainViewModel_ArticleEditingStarted);
      mMainViewModel.ArticleEditingFinished += new EventHandler(MainViewModel_ArticleEditingFinished);
      mMainViewModel.ImageAttachmentStarted += new EventHandler<ArticleChangeArgs>(MainViewModel_ImageEditingStarted);
      mMainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

      mMainView = new MainWindow();
      mMainView.DataContext = mMainViewModel;
      mMainView.Show();

      mLoginView.Close();
    }

    private void ViewModel_LoginFailed(object sender, EventArgs e)
    {
      MessageBox.Show("A bejelentkezés sikertelen!", "News Agency", MessageBoxButton.OK, MessageBoxImage.Asterisk);
    }

    private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
    {
      MessageBox.Show(e.Message, "News Agency", MessageBoxButton.OK, MessageBoxImage.Asterisk);
    }

    private void MainViewModel_ArticleEditingStarted(object sender, EventArgs e)
    {
      mEditorView = new ArticleEditorWindow(); // külön szerkesztő dialógus az épületekre
      mEditorView.DataContext = mMainViewModel;
      mEditorView.Show();
    }

    private void MainViewModel_ArticleEditingFinished(object sender, EventArgs e)
    {
      mEditorView.Close();
    }

    private void MainViewModel_ImageEditingStarted(object sender, ArticleChangeArgs e)
    {
      try
      {
        // egy dialógusablakban bekérjük a fájlnevet
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.CheckFileExists = true;
        dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        Boolean? result = dialog.ShowDialog();

        if (result == true)
        {
          // kép létrehozása (a megfelelő méretekkel)
          mModel.CreateImage(e.ArticleId, ImageHandler.OpenAndResize(dialog.FileName, 600));
        }
      }
      catch { }
    }

    private void ViewModel_ExitApplication(object sender, System.EventArgs e)
    {
      Shutdown();
    }
  }
}