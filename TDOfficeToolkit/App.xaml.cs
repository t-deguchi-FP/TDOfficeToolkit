using TDOfficeToolkit.ViewModels;
using System.Windows;
using TDOfficeToolkit.Views;

namespace TDOfficeToolkit
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      new MainView() { DataContext = new MainViewModel() }.Show();
    }

    /// <summary>
    /// バージョン情報ウィンドウを表示
    /// </summary>
    public static void ShowVersionWindow()
    {
      var versionView = new VersionView();
      versionView.DataContext = new VersionViewModel(() => versionView.Close());
      versionView.ShowDialog();
    }
  }
}
