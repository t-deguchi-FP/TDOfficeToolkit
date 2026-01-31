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

    /// <summary>
    /// アプリケーション名
    /// </summary>
    public string ApplicationName => "TDOfficeToolkit";

    /// <summary>
    /// バージョン情報
    /// </summary>
    public string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

    /// <summary>
    /// コピーライト情報
    /// </summary>
    public string Copyright => $"© {DateTime.Now.Year} TDOfficeToolkit";

    /// <summary>
    /// 制作者
    /// </summary>
    public string Developer => "T.Deguchi";

    /// <summary>
    /// Instagram URL
    /// </summary>
    public string InstagramUrl => "https://www.instagram.com/teruhiko.deguchi/";

    /// <summary>
    /// GitHub URL
    /// </summary>
    public string GitHubUrl => "https://github.com/t-deguchi-FP/TDOfficeToolkit";

    /// <summary>
    /// 閉じるコマンド
    /// </summary>
    public DelegateCommand CloseCommand => new DelegateCommand(_ => _closeAction?.Invoke());

    /// <summary>
    /// Instagram を開くコマンド
    /// </summary>
    public DelegateCommand OpenInstagramCommand => new DelegateCommand(_ => OpenUrl(InstagramUrl));

    /// <summary>
    /// GitHub を開くコマンド
    /// </summary>
    public DelegateCommand OpenGitHubCommand => new DelegateCommand(_ => OpenUrl(GitHubUrl));

    /// <summary>
    /// 指定された URL をブラウザで開く
    /// </summary>
    /// <param name="url">開く URL</param>
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
        System.Diagnostics.Debug.WriteLine($"URL を開けませんでした: {ex.Message}");
      }
    }
  }
}