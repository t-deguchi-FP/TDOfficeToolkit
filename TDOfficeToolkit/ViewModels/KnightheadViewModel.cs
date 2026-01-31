using Microsoft.Win32;
using System;
using System.IO;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

public class KnightheadViewModel : NotificationObject
{
  private string? _currentPdfPath;
  private Uri? _pdfSource;

  /// <summary>
  /// 現在開いているPDFファイルのパス
  /// </summary>
  public string? CurrentPdfPath
  {
    get => _currentPdfPath;
    set => SetProperty(ref _currentPdfPath, value);
  }

  /// <summary>
  /// WebView2に表示するPDFのソース
  /// </summary>
  public Uri? PdfSource
  {
    get => _pdfSource;
    set => SetProperty(ref _pdfSource, value);
  }

  /// <summary>
  /// PDFを開くコマンド
  /// </summary>
  public DelegateCommand OpenPdfCommand => new DelegateCommand(_ => OpenPdf());

  /// <summary>
  /// PDFファイルを開くダイアログを表示
  /// </summary>
  private void OpenPdf()
  {
    var dialog = new OpenFileDialog
    {
      Filter = "PDFファイル (*.pdf)|*.pdf|すべてのファイル (*.*)|*.*",
      Title = "PDFファイルを選択"
    };

    if (dialog.ShowDialog() == true)
    {
      LoadPdf(dialog.FileName);
    }
  }

  /// <summary>
  /// 指定されたPDFファイルを読み込む
  /// </summary>
  public void LoadPdf(string filePath)
  {
    if (File.Exists(filePath))
    {
      CurrentPdfPath = Path.GetFileName(filePath);
      PdfSource = new Uri(filePath);
    }
  }
}