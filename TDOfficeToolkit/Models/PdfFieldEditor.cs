using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TDOfficeToolkit.Models;

/// <summary>
/// PDFフォームフィールドの読み取り・編集を行うクラス
/// </summary>
public class PdfFieldEditor : IDisposable
{
  private PdfDocument? _pdfDocument;
  private PdfAcroForm? _form;
  private string? _currentFilePath;

  /// <summary>
  /// PDFファイルを開く
  /// </summary>
  public void OpenPdf(string filePath)
  {
    ClosePdf();

    _currentFilePath = filePath;
    var reader = new PdfReader(filePath);
    var writer = new PdfWriter(new MemoryStream());
    _pdfDocument = new PdfDocument(reader, writer);
    _form = PdfAcroForm.GetAcroForm(_pdfDocument, true);
  }

  /// <summary>
  /// PDFをファイルに保存
  /// </summary>
  public void SavePdf(string outputPath)
  {
    if (_pdfDocument == null || _form == null)
      throw new InvalidOperationException("PDFが開かれていません");

    // 新しいPDFドキュメントを作成して保存
    using var reader = new PdfReader(_currentFilePath!);
    using var writer = new PdfWriter(outputPath);
    using var pdfDoc = new PdfDocument(reader, writer);
    var form = PdfAcroForm.GetAcroForm(pdfDoc, true);

    // フィールド値を適用
    foreach (var field in GetAllFields())
    {
      var pdfField = form.GetField(field.Key);
      if (pdfField != null)
      {
        pdfField.SetValue(field.Value);
      }
    }

    pdfDoc.Close();
  }

  /// <summary>
  /// すべてのフィールド名と値を取得
  /// </summary>
  public Dictionary<string, string> GetAllFields()
  {
    if (_form == null)
      throw new InvalidOperationException("PDFが開かれていません");

    var fields = new Dictionary<string, string>();
    var fieldNames = _form.GetAllFormFields();

    foreach (var kvp in fieldNames)
    {
      var fieldName = kvp.Key;
      var field = kvp.Value;
      var value = field.GetValueAsString() ?? string.Empty;
      fields[fieldName] = value;
    }

    return fields;
  }

  /// <summary>
  /// 特定のフィールドの値を取得
  /// </summary>
  public string? GetFieldValue(string fieldName)
  {
    if (_form == null)
      throw new InvalidOperationException("PDFが開かれていません");

    var field = _form.GetField(fieldName);
    return field?.GetValueAsString();
  }

  /// <summary>
  /// 特定のフィールドに値を設定
  /// </summary>
  public void SetFieldValue(string fieldName, string value)
  {
    if (_form == null)
      throw new InvalidOperationException("PDFが開かれていません");

    var field = _form.GetField(fieldName);
    if (field != null)
    {
      field.SetValue(value);
    }
    else
    {
      throw new ArgumentException($"フィールド '{fieldName}' が見つかりません");
    }
  }

  /// <summary>
  /// 複数のフィールドに値を一括設定
  /// </summary>
  public void SetFieldValues(Dictionary<string, string> fieldValues)
  {
    foreach (var kvp in fieldValues)
    {
      try
      {
        SetFieldValue(kvp.Key, kvp.Value);
      }
      catch (ArgumentException)
      {
        // フィールドが存在しない場合はスキップ
        continue;
      }
    }
  }

  /// <summary>
  /// フィールドが存在するかチェック
  /// </summary>
  public bool FieldExists(string fieldName)
  {
    if (_form == null)
      throw new InvalidOperationException("PDFが開かれていません");

    return _form.GetField(fieldName) != null;
  }

  /// <summary>
  /// PDFを閉じる
  /// </summary>
  public void ClosePdf()
  {
    _pdfDocument?.Close();
    _pdfDocument = null;
    _form = null;
    _currentFilePath = null;
  }

  public void Dispose()
  {
    ClosePdf();
  }
}