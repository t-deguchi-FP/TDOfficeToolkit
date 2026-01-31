using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

internal class MainViewModel : NotificationObject
{
    // バージョン情報表示コマンド
    public DelegateCommand ShowVersionCommand => new DelegateCommand(App.ShowVersionWindow);
}
