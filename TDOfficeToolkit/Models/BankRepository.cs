using System.Collections.Generic;
using System.Linq;

namespace TDOfficeToolkit.Models;

/// <summary>
/// 銀行情報リポジトリ
/// </summary>
public class BankRepository
{
  private static readonly List<BankInfo> _banks = new()
  {
    new BankInfo
    {
      Name = "三菱UFJ銀行",
      Street = "2-7-1 Marunouchi",
      City = "Chiyoda-ku, Tokyo",
      Country = "Japan",
      PostalCode = "100-8388",
      SwiftCode = "BOTKJPJT"
    },
    new BankInfo
    {
      Name = "三井住友銀行",
      Street = "1-1-2 Marunouchi",
      City = "Chiyoda-ku, Tokyo",
      Country = "Japan",
      PostalCode = "100-0005",
      SwiftCode = "SMBCJPJT"
    },
    new BankInfo
    {
      Name = "みずほ銀行",
      Street = "1-5-5 Otemachi",
      City = "Chiyoda-ku, Tokyo",
      Country = "Japan",
      PostalCode = "100-8176",
      SwiftCode = "MHCBJPJT"
    },
    new BankInfo
    {
      Name = "りそな銀行",
      Street = "5-5 Odori Nishi",
      City = "Chuo-ku, Osaka",
      Country = "Japan",
      PostalCode = "540-8610",
      SwiftCode = "DIWAJPJT"
    },
    new BankInfo
    {
      Name = "ゆうちょ銀行",
      Street = "1-3-2 Kasumigaseki",
      City = "Chiyoda-ku, Tokyo",
      Country = "Japan",
      PostalCode = "100-8798",
      SwiftCode = "JURUJPJZ"
    },
    new BankInfo
    {
      Name = "JPMorgan Chase Bank",
      Street = "383 Madison Avenue",
      City = "New York, NY",
      Country = "United States",
      PostalCode = "10179",
      SwiftCode = "CHASUS33"
    },
    new BankInfo
    {
      Name = "Bank of America",
      Street = "100 North Tryon Street",
      City = "Charlotte, NC",
      Country = "United States",
      PostalCode = "28255",
      SwiftCode = "BOFAUS3N"
    },
    new BankInfo
    {
      Name = "Citibank",
      Street = "388 Greenwich Street",
      City = "New York, NY",
      Country = "United States",
      PostalCode = "10013",
      SwiftCode = "CITIUS33"
    },
    new BankInfo
    {
      Name = "HSBC Bank",
      Street = "8 Canada Square",
      City = "London",
      Country = "United Kingdom",
      PostalCode = "E14 5HQ",
      SwiftCode = "HBUKGB4B"
    },
    new BankInfo
    {
      Name = "UBS Switzerland",
      Street = "Bahnhofstrasse 45",
      City = "Zurich",
      Country = "Switzerland",
      PostalCode = "8001",
      SwiftCode = "UBSWCHZH80A"
    }
  };

  /// <summary>
  /// すべての銀行情報を取得
  /// </summary>
  public static List<BankInfo> GetAllBanks()
  {
    return _banks.OrderBy(b => b.Name).ToList();
  }

  /// <summary>
  /// SWIFTコードで銀行情報を取得
  /// </summary>
  public static BankInfo? GetBankBySwiftCode(string swiftCode)
  {
    return _banks.FirstOrDefault(b => b.SwiftCode == swiftCode);
  }

  /// <summary>
  /// 銀行名で銀行情報を取得
  /// </summary>
  public static BankInfo? GetBankByName(string name)
  {
    return _banks.FirstOrDefault(b => b.Name == name);
  }
}