using TDOfficeToolkit.ViewModels;
using System.Windows;
using TDOfficeToolkit.Views;

namespace TDOfficeToolkit
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      new MainView() { DataContext = new MainViewModel() }.Show();
    }
  }
}
