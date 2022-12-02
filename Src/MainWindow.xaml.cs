// AppsAndroidEnW10Mobile.MainWindow

using AppsAndroidEnW10Mobile.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace AppsAndroidEnW10Mobile
{
  public partial class MainWindow : Window, IComponentConnector
  {
    private string file = "";
    private string result = "Install Has Finished";//Resources.InstallHasFinished;

    //MainWindow constructor
    public MainWindow()
    {
        this.InitializeComponent();

        this.DeactivateStep3And4();

        this.checkdevice();
    }//

    private async void CheckStep2_Click(object sender, RoutedEventArgs e)
    {
        this.CheckConnectivity.IsEnabled = false;
        this.DisconnectAll.IsEnabled = false;
        this.CheckStep2.IsEnabled = false;

        //("%appdata%\\APKTOW10M\\connect\\wconnect.exe usb "
        string str = await this.ExecuteCommandASync("wconnect.exe usb " 
            + this.Code.Text);

        if (str.Contains("Error c"))
        {
            //int num = (int) 
            MessageBox.Show("Error: " + Environment.NewLine + str.Trim());
        }
        this.checkdevice();
        this.CheckConnectivity.IsEnabled = true;
        this.DisconnectAll.IsEnabled = true;
        this.CheckStep2.IsEnabled = true;
    }//

    private async void CheckStep2IP_Click(object sender, RoutedEventArgs e)
    {
        this.CheckConnectivity.IsEnabled = false;
        this.DisconnectAll.IsEnabled = false;
        this.CheckStep2.IsEnabled = false;

        // ("%appdata%\\APKTOW10M\\connect\\wconnect.exe "
        string str = await this.ExecuteCommandASync("wconnect.exe " 
            + this.CodeIPCode.Text + " " + this.Code.Text);

        if (str.Contains("Error c"))
        {
            //int num = (int) 
            MessageBox.Show("Error: " + Environment.NewLine + str.Trim());
        }

        this.checkdevice();

        this.CheckConnectivity.IsEnabled = true;
        this.DisconnectAll.IsEnabled = true;
        this.CheckStep2.IsEnabled = true;
    }//

    private async void checkdevice()
    {
        // "%appdata%\\APKTOW10M\\connect\\wconnect.exe devices")) 
        if ((await this.ExecuteCommandASync("wconnect.exe devices"))
            .Contains("are no connected"))
        {
            this.NotConnected.Visibility = Visibility.Visible;
            this.Connected.Visibility = Visibility.Visible;
                    this.DeviceSituation.Text = "Device Not Connected";// Resources.DeviceNotConnected;

            this.DeactivateStep3And4();
        }
        else
        {
            this.NotConnected.Visibility = Visibility.Collapsed;
            this.Connected.Visibility = Visibility.Visible;
            this.DeviceSituation.Text = "Device Already Connected";//Resources.DeviceAlreadyConnected;
        
            this.ActivateStep3And4();
        }
    }//

    private async void disconnectall()
    {
      this.CheckConnectivity.IsEnabled = false;
      this.DisconnectAll.IsEnabled = false;
      this.Code.Text = "";
      this.CheckStep2.IsEnabled = false;

      // "%appdata%\\APKTOW10M\\connect\\wconnect.exe disconnect"
      await this.ExecuteCommandASync("wconnect.exe disconnect");
      
      this.checkdevice();
      
      this.CheckConnectivity.IsEnabled = true;
      this.DisconnectAll.IsEnabled = true;
    }//

    public async Task<string> ExecuteCommandASync(string command)
    {
      try
      {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command);
        
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;

        Process process = new Process();
        
        process.StartInfo = processStartInfo;
        
        process.Start();
        
        return process.StandardOutput.ReadToEnd();
      }
      catch (Exception ex)
      {
         Debug.WriteLine("[ex] " + ex.Message);
         return "";
      }
    }//

    public void DeactivateStep3And4()
    {
      this.Step3.Opacity = 0.5;
      this.Step4.Opacity = 0.5;
      this.CheckStep3.IsEnabled = false;
    }//

    public void ActivateStep3And4()
    {
      this.Step3.Opacity = 1.0;
      this.Step4.Opacity = 1.0;
      this.CheckStep3.IsEnabled = true;
    }//

    private void Grid_Drop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      if (((IEnumerable<string>) data).Count<string>() <= 0)
        return;
      if (!((IEnumerable<string>) data).ElementAt<string>(0).ToLower().EndsWith(".apk"))
      {
        //int num = (int) 
        MessageBox.Show(/*Resources.ThisIsNotAnAPK*/ "This Is Not An APK " 
            + Environment.NewLine + /*Resources.PleaseDropAPK*/ "Please Drop APK ");
      }
      else
      {
        this.file = ((IEnumerable<string>) data).ElementAt<string>(0);
        this.File.Text = this.file;
      }
    }//

    private void Code_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.Code != null && this.CheckStep2 != null && this.Code.Text.Length > 5 
                && !this.Code.Text.Contains(" "))
        this.CheckStep2.IsEnabled = true;
      else
        this.CheckStep2.IsEnabled = false;

      if (this.CodeIPCode != null && this.CheckStep2IP != null 
                && this.CodeIPCode.Text.Length > 5 && !this.CodeIPCode.Text.Contains(" "))
      {
        this.CheckStep2IP.IsEnabled = true;
      }
      else
      {
        try
        {
             if (this.CheckStep2IP != null)
                this.CheckStep2IP.IsEnabled = false;
        }
        catch
        {
        }
      }
    }//

    private void CheckConnectivity_Click(object sender, RoutedEventArgs e)
    {
        this.checkdevice();
    }//

    private async void CheckStep3_Click(object sender, RoutedEventArgs e)
    {
      this.InstallProgress.Visibility = Visibility.Visible;
      this.CheckStep3.IsEnabled = false;
      this.CheckConnectivity.IsEnabled = false;
      
      await Task.Factory.StartNew((Action) (async () => 
      await this.performInstall())).ContinueWith
      (
          (Action<Task>) (t =>
          {
            this.InstallProgress.Visibility = Visibility.Collapsed;
            this.CheckStep3.IsEnabled = true;
            this.CheckConnectivity.IsEnabled = true;
            
            //int num = (int) 
            MessageBox.Show(this.result);
            
            //Process.Start("http://www.microsoftinsider.es?apktool");
          }
          ), TaskScheduler.FromCurrentSynchronizationContext()
      );
    }//

    private async Task performInstall()
    {
      if (this.file == "")
      {
        int num = (int) MessageBox.Show("[Alert] Drop APK On Square "/*Resources.DropAPKOnSquareAlert*/);
        this.InstallProgress.Visibility = Visibility.Visible;
      }
      else if ((await this.ExecuteCommandASync("adb install \""//"%appdata%\\APKTOW10M\\platform-tools\\adb install \""
          + this.file + "\"")).ToLower().Contains("success"))
      {
        string installationOk = "Installation OK";//Resources.InstallationOK;

        MessageBox.Show("Status: " + Environment.NewLine + installationOk.Trim());
      }
      else
      {
        string installationFailed = "Installation Failed";//Resources.InstallationFailed;

        MessageBox.Show("Status: " + Environment.NewLine + installationFailed.Trim());
       }
    }//

    private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
    {
        // ?
        //Process.Start("http://www.microsoftinsider.es?apktool");
    }//

    private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
    {
        // ?
        //Process.Start("http://www.microsoftinsider.es?apktowin10m");
    }//

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      if (this.ConnectByUSB == null || this.ConnectByIP == null)
        return;
      this.ConnectByUSB.Visibility = Visibility.Collapsed;
      this.ConnectByIP.Visibility = Visibility.Visible;
    }//

    private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
    {
      if (this.ConnectByUSB == null || this.ConnectByIP == null)
        return;

      this.ConnectByUSB.Visibility = Visibility.Visible;
      this.ConnectByIP.Visibility = Visibility.Collapsed;
    }//

    private void DisconnectAll_Click(object sender, RoutedEventArgs e)
    {
       this.disconnectall();
    }//

  }//class end

}//namespace end
