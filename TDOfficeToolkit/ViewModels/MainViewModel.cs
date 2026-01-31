using System.Windows;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

internal class MainViewModel : NotificationObject
{
  /// <summary>
  /// バージョン情報表示コマンド
  /// </summary>
  public DelegateCommand ShowVersionCommand => new DelegateCommand(_ => App.ShowVersionWindow());

  /// <summary>
  /// Knighthead 申込書作成を開くコマンド
  /// </summary>
  public DelegateCommand OpenKnightheadCommand => new DelegateCommand(_ => App.ShowKnightheadWindow());

  /// <summary>
  /// 確認書作成を開くコマンド
  /// </summary>
  public DelegateCommand OpenConfirmationCommand => new DelegateCommand(_ => App.ShowConfirmationWindow());

  /// <summary>
  /// 終了コマンド
  /// </summary>
  public DelegateCommand ExitCommand => new DelegateCommand(_ => Application.Current.Shutdown());
}
