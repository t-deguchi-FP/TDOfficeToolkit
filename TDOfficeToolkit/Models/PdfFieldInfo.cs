namespace TDOfficeToolkit.Models;

/// <summary>
/// PDF フィールドの情報を保持するモデル
/// </summary>
public class PdfFieldInfo
{
  /// <summary>
  /// フィールド名
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// フィールドの値
  /// </summary>
  public string Value { get; set; } = string.Empty;

  /// <summary>
  /// フィールドタイプ（テキスト、チェックボックスなど）
  /// </summary>
  public string FieldType { get; set; } = string.Empty;

  /// <summary>
  /// 編集可能かどうか
  /// </summary>
  public bool IsEditable { get; set; } = true;
}