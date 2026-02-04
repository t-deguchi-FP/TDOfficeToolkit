namespace TDOfficeToolkit.Models;

/// <summary>
/// 銀行情報
/// </summary>
public class BankInfo
{
  /// <summary>
  /// 金融機関名
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// 住所（番地・市区町村）
  /// </summary>
  public string Street { get; set; } = string.Empty;

  /// <summary>
  /// 県
  /// </summary>
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// 国
  /// </summary>
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// 郵便番号
  /// </summary>
  public string PostalCode { get; set; } = string.Empty;

  /// <summary>
  /// SWIFTコード
  /// </summary>
  public string SwiftCode { get; set; } = string.Empty;

  /// <summary>
  /// 表示名（ComboBox用）
  /// </summary>
  public string DisplayName => $"{Name} ({SwiftCode})";
}