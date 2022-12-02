// Decompiled with JetBrains decompiler
// Type: AppsAndroidEnW10Mobile.App
// Assembly: AppsAndroidEnW10Mobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEED7C7F-387F-4248-B907-79D52D807D92
// Assembly location: C:\Users\Admin\AppData\Roaming\APKTOW10M\app\AppsAndroidEnW10Mobile.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace AppsAndroidEnW10Mobile
{
  public class App : Application
  {
    private CultureInfo cultureOverride = CultureInfo.InstalledUICulture;

    public App()
    {
      if (this.cultureOverride == null)
        return;
      Thread.CurrentThread.CurrentUICulture = this.cultureOverride;
      Thread.CurrentThread.CurrentCulture = this.cultureOverride;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent() => this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
