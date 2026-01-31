using System;
using System.Collections.Generic;
using System.Text;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels
{
  internal class VersionViewModel : NotificationObject
  {
    // アプリケーション名
    public string ApplicationName => "TDOfficeToolkit";

    // バージョン情報
    public string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

    // コピーライト情報
    public string Copyright => $"© {DateTime.Now.Year} TDOfficeToolkit";
  }
}
