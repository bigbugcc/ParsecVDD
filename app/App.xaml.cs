using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using ParsecVDisplay.Languages;
using System.Threading;

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
            //Get System Language and set it to the app
            //string lang = Convert.ToString(Thread.CurrentThread.CurrentCulture.Name);
            if (Current != null)
            {
                Current.Dispatcher.Invoke(() =>
                {
                    LangManager.Instance.SetLanguage(lan);
                });
            }
        }
    }
}