using System.Windows;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

internal class MainViewModel : NotificationObject
{
  // バージョン情報表示コマンド
  public DelegateCommand ShowVersionCommand => new DelegateCommand(_ => App.ShowVersionWindow());

  // Knighthead 申込書作成を開くコマンド
  public DelegateCommand OpenKnightheadCommand => new DelegateCommand(_ => OpenKnightheadTool());

  // 確認書作成を開くコマンド
  public DelegateCommand OpenConfirmationCommand => new DelegateCommand(_ => OpenConfirmationTool());

  // 終了コマンド
  public DelegateCommand ExitCommand => new DelegateCommand(_ => Application.Current.Shutdown());

  private void OpenKnightheadTool()
  {
    // TODO: Knighthead用のウィンドウまたはビューを表示
    MessageBox.Show("Knighthead 申込書作成を開きます", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
  }

  private void OpenConfirmationTool()
  {
    // TODO: 確認書作成用のウィンドウまたはビューを表示
    MessageBox.Show("Coming Soon...", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
  }
}
