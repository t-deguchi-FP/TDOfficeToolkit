using System;
using System.Collections.Generic;
using System.Text;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels
{
    internal class VersionViewModel : NotificationObject
    {
        private readonly Action _closeAction;

        public VersionViewModel(Action closeAction)
        {
            _closeAction = closeAction;
        }

        // アプリケーション名
        public string ApplicationName => "TDOfficeToolkit";

        // バージョン情報
        public string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

        // コピーライト情報
        public string Copyright => $"© {DateTime.Now.Year} TDOfficeToolkit";

        // 制作者
        public string Developer => "T.Deguchi";

        // 閉じるコマンド
        public DelegateCommand CloseCommand => new DelegateCommand(_ => _closeAction?.Invoke());
    }
}
