using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using TDOfficeToolkit.Models;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

public class KnightheadViewModel : NotificationObject, IDisposable
{
  private readonly PdfFieldEditor _pdfEditor;
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
  /// PDFフィールドのコレクション
  /// </summary>
  public ObservableCollection<PdfFieldInfo> PdfFields { get; } = new();

  /// <summary>
  /// PDFを開くコマンド
  /// </summary>
  public DelegateCommand OpenPdfCommand => new DelegateCommand(_ => OpenPdf());

  /// <summary>
  /// PDFを保存するコマンド
  /// </summary>
  public DelegateCommand SavePdfCommand => new DelegateCommand(_ => SavePdf());

  public KnightheadViewModel()
  {
    _pdfEditor = new PdfFieldEditor();
  }

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
  private void LoadPdf(string filePath)
  {
    if (File.Exists(filePath))
    {
      try
      {
        // PDFをエディタで開く
        _pdfEditor.OpenPdf(filePath);

        // フィールド情報を取得
        PdfFields.Clear();
        var fields = _pdfEditor.GetAllFields();
        foreach (var field in fields)
        {
          PdfFields.Add(new PdfFieldInfo
          {
            Name = field.Key,
            Value = field.Value,
            IsEditable = true
          });
        }

        // ビューアーに表示
        CurrentPdfPath = Path.GetFileName(filePath);
        PdfSource = new Uri(filePath);
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show(
          $"PDFの読み込みに失敗しました: {ex.Message}",
          "エラー",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Error);
      }
    }
  }

  /// <summary>
  /// PDFを保存
  /// </summary>
  private void SavePdf()
  {
    var dialog = new SaveFileDialog
    {
      Filter = "PDFファイル (*.pdf)|*.pdf",
      Title = "PDFファイルを保存",
      FileName = CurrentPdfPath ?? "output.pdf"
    };

    if (dialog.ShowDialog() == true)
    {
      try
      {
        // フィールド値を更新
        var fieldValues = new System.Collections.Generic.Dictionary<string, string>();
        foreach (var field in PdfFields)
        {
          fieldValues[field.Name] = field.Value;
        }
        _pdfEditor.SetFieldValues(fieldValues);

        // 保存
        _pdfEditor.SavePdf(dialog.FileName);

        System.Windows.MessageBox.Show(
          "PDFを保存しました",
          "成功",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show(
          $"PDFの保存に失敗しました: {ex.Message}",
          "エラー",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Error);
      }
    }
  }

  public void Dispose()
  {
    _pdfEditor?.Dispose();
  }
}