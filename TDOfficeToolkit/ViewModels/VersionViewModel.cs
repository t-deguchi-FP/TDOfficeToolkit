using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
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

    // Instagram URL
    public string InstagramUrl => "https://www.instagram.com/teruhiko.deguchi/";

    // GitHub URL
    public string GitHubUrl => "https://github.com/t-deguchi-FP/TDOfficeToolkit";

    // 閉じるコマンド
    public DelegateCommand CloseCommand => new DelegateCommand(_ => _closeAction?.Invoke());

    // Instagramを開くコマンド
    public DelegateCommand OpenInstagramCommand => new DelegateCommand(_ => OpenUrl(InstagramUrl));

    // GitHubを開くコマンド
    public DelegateCommand OpenGitHubCommand => new DelegateCommand(_ => OpenUrl(GitHubUrl));

    private void OpenUrl(string url)
    {
      try
      {
        Process.Start(new ProcessStartInfo
        {
          FileName = url,
          UseShellExecute = true
        });
      }
      catch (Exception ex)
      {
        // エラーハンドリング（必要に応じて）
        System.Diagnostics.Debug.WriteLine($"URLを開けませんでした: {ex.Message}");
      }
    }
  }
}