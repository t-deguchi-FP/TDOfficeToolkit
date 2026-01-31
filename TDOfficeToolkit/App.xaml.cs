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

    /// <summary>
    /// Knighthead申込書作成ウィンドウを表示
    /// </summary>
    public static void ShowKnightheadWindow()
    {
      Current.MainWindow.Hide(); // MainViewを非表示
      
      var knightheadView = new KnightheadView
      {
        DataContext = new KnightheadViewModel(),
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      
      // 閉じられたらMainViewを再表示
      knightheadView.Closed += (s, e) => Current.MainWindow.Show();
      
      knightheadView.ShowDialog();
    }

    /// <summary>
    /// 確認書作成ウィンドウを表示
    /// </summary>
    public static void ShowConfirmationWindow()
    {
      // TODO: 確認書作成ビュー実装後に追加
      MessageBox.Show("Coming Soon...", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
    }
  }
}
