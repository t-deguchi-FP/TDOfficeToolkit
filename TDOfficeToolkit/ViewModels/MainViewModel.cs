using System.Windows;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

internal class MainViewModel : NotificationObject
{
  // バージョン情報表示コマンド
  public DelegateCommand ShowVersionCommand => new DelegateCommand(_ => App.ShowVersionWindow());

  // Knighthead 申込書作成を開くコマンド
  public DelegateCommand OpenKnightheadCommand => new DelegateCommand(_ => App.ShowKnightheadWindow());

  // 確認書作成を開くコマンド
  public DelegateCommand OpenConfirmationCommand => new DelegateCommand(_ => App.ShowConfirmationWindow());

  // 終了コマンド
  public DelegateCommand ExitCommand => new DelegateCommand(_ => Application.Current.Shutdown());
}
