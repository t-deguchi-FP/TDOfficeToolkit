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
    /// <summary>
    /// アプリケーション起動時の処理
    /// </summary>
    /// <param name="e">起動イベント引数</param>
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
    /// Knighthead 申込書作成ウィンドウを表示
    /// </summary>
    public static void ShowKnightheadWindow()
    {
      // MainView を非表示
      Current.MainWindow.Hide();
      
      var knightheadView = new KnightheadView
      {
        DataContext = new KnightheadViewModel(),
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      
      // 閉じられたら MainView を再表示
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
