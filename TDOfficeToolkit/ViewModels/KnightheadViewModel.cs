using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TDOfficeToolkit.Models;
using YKToolkit.Bindings;
using YKToolkit.Controls;

namespace TDOfficeToolkit.ViewModels;

/// <summary>
/// Knighthead 申込書作成画面の ViewModel
/// </summary>
public class KnightheadViewModel : NotificationObject, IDisposable
{
  private readonly PdfFieldEditor _pdfEditor;
  private string? _currentPdfPath;
  private Uri? _pdfSource;
  private TrustParticipantInfo? _trustParticipant;
  private FundingAccountInfo? _fundingAccount;
  private BeneficiaryInfo? _primaryBeneficiary;
  private ProductSelectionsInfo? _productSelections;

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
  /// 信託参加者情報
  /// </summary>
  public TrustParticipantInfo TrustParticipant
  {
    get => _trustParticipant ??= new TrustParticipantInfo();
    set => SetProperty(ref _trustParticipant, value);
  }

  /// <summary>
  /// 資金口座情報
  /// </summary>
  public FundingAccountInfo FundingAccount
  {
    get => _fundingAccount ??= new FundingAccountInfo();
    set => SetProperty(ref _fundingAccount, value);
  }

  /// <summary>
  /// 受益者情報
  /// </summary>
  public BeneficiaryInfo PrimaryBeneficiary
  {
    get => _primaryBeneficiary ??= new BeneficiaryInfo();
    set => SetProperty(ref _primaryBeneficiary, value);
  }

  /// <summary>
  /// 商品選択情報
  /// </summary>
  public ProductSelectionsInfo ProductSelections
  {
    get => _productSelections ??= new ProductSelectionsInfo();
    set => SetProperty(ref _productSelections, value);
  }

  /// <summary>
  /// PDF フィールドのコレクション
  /// </summary>
  public ObservableCollection<PdfFieldInfo> PdfFields { get; } = new();

  /// <summary>
  /// 新規申込コマンド
  /// </summary>
  public DelegateCommand NewApplicationCommand => new DelegateCommand(_ => NewApplication());

  /// <summary>
  /// PDF読込コマンド
  /// </summary>
  public DelegateCommand LoadPdfCommand => new DelegateCommand(_ => LoadPdfDialog());

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
  /// 新規申込を開始
  /// </summary>
  private void NewApplication()
  {
    var result = YKToolkit.Controls.MessageBox.Show(
      "新規申込を開始します。入力中のデータはクリアされますがよろしいですか?",
      "確認",
      MessageBoxButton.YesNo,
      MessageBoxImage.Question);

    if (result == MessageBoxResult.Yes)
    {
      // すべての入力フィールドをクリア
      TrustParticipant = new TrustParticipantInfo();
      FundingAccount = new FundingAccountInfo();
      PrimaryBeneficiary = new BeneficiaryInfo();
      ProductSelections = new ProductSelectionsInfo();
      
      // PDF関連もクリア
      CurrentPdfPath = null;
      PdfSource = null;
      PdfFields.Clear();
      
      YKToolkit.Controls.MessageBox.Show(
        "新規申込を開始しました",
        "情報",
        MessageBoxButton.OK,
        MessageBoxImage.Information);
    }
  }

  /// <summary>
  /// PDFファイル読込ダイアログを表示
  /// </summary>
  private void LoadPdfDialog()
  {
    var dialog = new OpenFileDialog
    {
      Filter = "PDF ファイル (*.pdf)|*.pdf|すべてのファイル (*.*)|*.*",
      Title = "PDFファイルを選択"
    };

    if (dialog.ShowDialog() == true)
    {
      LoadPdfFile(dialog.FileName);
    }
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
  private void LoadPdfFile(string filePath)
  {
    if (File.Exists(filePath))
    {
      try
      {
        // PDF をエディタで開く
        _pdfEditor.OpenPdf(filePath);

        // フィールド情報を取得して入力フォームに反映
        var fields = _pdfEditor.GetAllFields();
        
        // 信託参加者情報の読み込み
        TrustParticipant.LastName = GetFieldValue(fields, "LastName");
        TrustParticipant.FirstName = GetFieldValue(fields, "FirstName");
        TrustParticipant.Address = GetFieldValue(fields, "Address");
        TrustParticipant.City = GetFieldValue(fields, "City");
        TrustParticipant.Country = GetFieldValue(fields, "Country");
        TrustParticipant.PostalCode = GetFieldValue(fields, "PostalCode");
        TrustParticipant.Gender = GetFieldValue(fields, "Gender");
        TrustParticipant.Phone = GetFieldValue(fields, "Phone");
        TrustParticipant.Email = GetFieldValue(fields, "Email");
        TrustParticipant.TaxIdentificationNumber = GetFieldValue(fields, "TaxIdentificationNumber");
        TrustParticipant.NameOfEmployer = GetFieldValue(fields, "NameOfEmployer");
        TrustParticipant.Position = GetFieldValue(fields, "Position");
        TrustParticipant.BusinessPhone = GetFieldValue(fields, "BusinessPhone");

        // 資金口座情報の読み込み
        FundingAccount.FinancialInstitutionName = GetFieldValue(fields, "FinancialInstitutionName");
        FundingAccount.Street = GetFieldValue(fields, "Street");
        FundingAccount.City = GetFieldValue(fields, "FundingCity");
        FundingAccount.Country = GetFieldValue(fields, "FundingCountry");
        FundingAccount.PostalCode = GetFieldValue(fields, "FundingPostalCode");
        FundingAccount.SwiftCode = GetFieldValue(fields, "SwiftCode");
        FundingAccount.ClientAccountNumber = GetFieldValue(fields, "ClientAccountNumber");

        // 受益者情報の読み込み
        PrimaryBeneficiary.LastName = GetFieldValue(fields, "BeneficiaryLastName");
        PrimaryBeneficiary.FirstName = GetFieldValue(fields, "BeneficiaryFirstName");
        PrimaryBeneficiary.Relationship = GetFieldValue(fields, "Relationship");

        // 商品選択情報の読み込み
        ProductSelections.NumberOfYears = GetFieldValue(fields, "NumberOfYears");
        ProductSelections.Amount = GetFieldValue(fields, "Amount");

        // ビューアーに表示
        CurrentPdfPath = Path.GetFileName(filePath);
        PdfSource = new Uri(filePath);

        YKToolkit.Controls.MessageBox.Show(
          "PDF を読み込みました",
          "情報",
          MessageBoxButton.OK,
          MessageBoxImage.Information);

      }
      catch (Exception ex)
      {
        YKToolkit.Controls.MessageBox.Show(
          $"PDF の読み込みに失敗しました: {ex.Message}",
          "エラー",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
      }
    }
  }

  /// <summary>
  /// フィールド値を取得
  /// </summary>
  private string GetFieldValue(System.Collections.Generic.Dictionary<string, string> fields, string key)
  {
    return fields.TryGetValue(key, out var value) ? value : string.Empty;
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
        YKToolkit.Controls.MessageBox.Show(
          $"PDF の読み込みに失敗しました: {ex.Message}",
          "エラー",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
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

        YKToolkit.Controls.MessageBox.Show(
          "PDF を保存しました",
          "成功",
          MessageBoxButton.OK,
          MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        YKToolkit.Controls.MessageBox.Show(
          $"PDF の保存に失敗しました: {ex.Message}",
          "エラー",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
      }
    }
  }

  /// <summary>
  /// リソースの解放
  /// </summary>
  public void Dispose()
  {
    _pdfEditor?.Dispose();
  }
}

// 各情報クラスの定義
public class TrustParticipantInfo : NotificationObject
{
  private string? _lastName;
  private string? _firstName;
  private string? _address;
  private string? _city;
  private string? _country;
  private string? _postalCode;
  private string? _gender;
  private string? _phone;
  private string? _email;
  private string? _taxIdentificationNumber;
  private DateTime? _dateOfIssue;
  private DateTime? _expirationDate;
  private string? _nameOfEmployer;
  private string? _position;
  private string? _businessPhone;

  public string? LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
  public string? FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
  public string? Address { get => _address; set => SetProperty(ref _address, value); }
  public string? City { get => _city; set => SetProperty(ref _city, value); }
  public string? Country { get => _country; set => SetProperty(ref _country, value); }
  public string? PostalCode { get => _postalCode; set => SetProperty(ref _postalCode, value); }
  public string? Gender { get => _gender; set => SetProperty(ref _gender, value); }
  public string? Phone { get => _phone; set => SetProperty(ref _phone, value); }
  public string? Email { get => _email; set => SetProperty(ref _email, value); }
  public string? TaxIdentificationNumber { get => _taxIdentificationNumber; set => SetProperty(ref _taxIdentificationNumber, value); }
  public DateTime? DateOfIssue { get => _dateOfIssue; set => SetProperty(ref _dateOfIssue, value); }
  public DateTime? ExpirationDate { get => _expirationDate; set => SetProperty(ref _expirationDate, value); }
  public string? NameOfEmployer { get => _nameOfEmployer; set => SetProperty(ref _nameOfEmployer, value); }
  public string? Position { get => _position; set => SetProperty(ref _position, value); }
  public string? BusinessPhone { get => _businessPhone; set => SetProperty(ref _businessPhone, value); }
}

public class FundingAccountInfo : NotificationObject
{
  private string? _financialInstitutionName;
  private string? _street;
  private string? _city;
  private string? _country;
  private string? _postalCode;
  private string? _swiftCode;
  private string? _clientAccountNumber;

  public string? FinancialInstitutionName { get => _financialInstitutionName; set => SetProperty(ref _financialInstitutionName, value); }
  public string? Street { get => _street; set => SetProperty(ref _street, value); }
  public string? City { get => _city; set => SetProperty(ref _city, value); }
  public string? Country { get => _country; set => SetProperty(ref _country, value); }
  public string? PostalCode { get => _postalCode; set => SetProperty(ref _postalCode, value); }
  public string? SwiftCode { get => _swiftCode; set => SetProperty(ref _swiftCode, value); }
  public string? ClientAccountNumber { get => _clientAccountNumber; set => SetProperty(ref _clientAccountNumber, value); }
}

public class BeneficiaryInfo : NotificationObject
{
  private string? _lastName;
  private string? _firstName;
  private DateTime? _dateOfBirth;
  private string? _relationship;

  public string? LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
  public string? FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
  public DateTime? DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }
  public string? Relationship { get => _relationship; set => SetProperty(ref _relationship, value); }
}

public class ProductSelectionsInfo : NotificationObject
{
  private string? _numberOfYears;
  private string? _amount;

  public string? NumberOfYears { get => _numberOfYears; set => SetProperty(ref _numberOfYears, value); }
  public string? Amount { get => _amount; set => SetProperty(ref _amount, value); }
}