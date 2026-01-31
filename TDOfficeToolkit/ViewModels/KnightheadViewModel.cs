using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using TDOfficeToolkit.Models;
using YKToolkit.Bindings;

namespace TDOfficeToolkit.ViewModels;

/// <summary>
/// Knighthead 申込書作成画面の ViewModel
/// </summary>
public class KnightheadViewModel : NotificationObject, IDisposable
{
  private readonly PdfFieldEditor _pdfEditor;
  private string? _currentPdfPath;
  private Uri? _pdfSource;

  /// <summary>
  /// 現在開いている PDF ファイルのパス
  /// </summary>
  public string? CurrentPdfPath
  {
    get => _currentPdfPath;
    set => SetProperty(ref _currentPdfPath, value);
  }

  /// <summary>
  /// WebView2 に表示する PDF のソース
  /// </summary>
  public Uri? PdfSource
  {
    get => _pdfSource;
    set => SetProperty(ref _pdfSource, value);
  }

  /// <summary>
  /// PDF フィールドのコレクション
  /// </summary>
  public ObservableCollection<PdfFieldInfo> PdfFields { get; } = new();

  /// <summary>
  /// PDF を開くコマンド
  /// </summary>
  public DelegateCommand OpenPdfCommand => new DelegateCommand(_ => OpenPdf());

  /// <summary>
  /// PDF を保存するコマンド
  /// </summary>
  public DelegateCommand SavePdfCommand => new DelegateCommand(_ => SavePdf());

  /// <summary>
  /// コンストラクタ
  /// </summary>
  public KnightheadViewModel()
  {
    _pdfEditor = new PdfFieldEditor();
  }

  /// <summary>
  /// PDF ファイルを開くダイアログを表示
  /// </summary>
  private void OpenPdf()
  {
    var dialog = new OpenFileDialog
    {
      Filter = "PDF ファイル (*.pdf)|*.pdf|すべてのファイル (*.*)|*.*",
      Title = "PDF ファイルを選択"
    };

    if (dialog.ShowDialog() == true)
    {
      LoadPdf(dialog.FileName);
    }
  }

  /// <summary>
  /// 指定された PDF ファイルを読み込む
  /// </summary>
  /// <param name="filePath">PDF ファイルのパス</param>
  private void LoadPdf(string filePath)
  {
    if (File.Exists(filePath))
    {
      try
      {
        // PDF をエディタで開く
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
          $"PDF の読み込みに失敗しました: {ex.Message}",
          "エラー",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Error);
      }
    }
  }

  /// <summary>
  /// PDF を保存
  /// </summary>
  private void SavePdf()
  {
    var dialog = new SaveFileDialog
    {
      Filter = "PDF ファイル (*.pdf)|*.pdf",
      Title = "PDF ファイルを保存",
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
          "PDF を保存しました",
          "成功",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show(
          $"PDF の保存に失敗しました: {ex.Message}",
          "エラー",
          System.Windows.MessageBoxButton.OK,
          System.Windows.MessageBoxImage.Error);
      }
    }
  }

  /// <summary>
  /// リソースを解放
  /// </summary>
  public void Dispose()
  {
    _pdfEditor?.Dispose();
  }
}