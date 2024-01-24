using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      var folder = ApplicationData.Current.LocalFolder;
      var file = await folder.TryGetItemAsync(settingsfilename) as IStorageFile;

      if (file == null)
      {
        _Settings = _DefaultSettings;
        var newfile = await folder.CreateFileAsync(settingsfilename, CreationCollisionOption.ReplaceExisting);
        var text = JsonConvert.SerializeObject(_Settings);
        await FileIO.WriteTextAsync(newfile, text);
      }
      else
      {
        var text = await FileIO.ReadTextAsync(file);
        _Settings = JsonConvert.DeserializeObject<Setting[]>(text);
      }

      ShowSettings();
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
      _Settings.First().Value = DateTime.Now.Second.ToString();

      var folder = ApplicationData.Current.LocalFolder;
      var newfile = await folder.CreateFileAsync(settingsfilename, CreationCollisionOption.ReplaceExisting);
      var text = JsonConvert.SerializeObject(_Settings);
      await FileIO.WriteTextAsync(newfile, text);

      ShowSettings();
    }

    private void ShowSettings()
    {
      var settingstext = string.Join(", ", _Settings.Select(s => $"{s.Name} = { s.Value}"));
      Result.Text = settingstext;
    }

    string settingsfilename = "settings.json";
    Setting[] _Settings = Array.Empty<Setting>();
    Setting[] _DefaultSettings = new Setting[] 
    {
      new Setting()
      {
        Name = "LastSelected",
        Value = "0",
      },
      new Setting() 
      {
        Name = "PreferredStyle",
        Value = "Italian",
      }
    };
  }
  public class Setting
  {
    public string Name { get; set; }
    public string Value { get; set; }
  }
}
