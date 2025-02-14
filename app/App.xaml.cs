﻿using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using ParsecVDisplay.Languages;

namespace ParsecVDisplay
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Disable GPU to prevent flickering when adding display
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            base.OnStartup(e);

            var silent = e.Args.Contains("-silent");
            var window = new MainWindow();
            if (!silent)
            {
                window.Show();
            }
            SetLanguage(Config.Language);
        }

        public static void SetLanguage(string lan)
        {
            //string lang = Convert.ToString(Thread.CurrentThread.CurrentCulture.Name);
            if (Current != null)
            {
                //MainWindow
                Current.Dispatcher.Invoke(() =>
                {
                    LangManager.Instance.SetLanguage(lan);
                });
            }
            //Tray Thread
            LangManager.Instance.SetLanguage(lan);
        }
    }
}