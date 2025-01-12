using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using System.Windows.Markup;

namespace ParsecVDisplay.Languages
{
    public class LangManager : INotifyPropertyChanged
    {
        private readonly ResourceManager _resourceManager;
        public static LangManager Instance => _lazy.Value;

        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Lazy<LangManager> _lazy = new Lazy<LangManager>(() => new LangManager());

        public static Dictionary<string, string> DicLang = new Dictionary<string, string>
        {
            { "English", "en-US" },
            { "Tiếng Việt", "vi-VN" },
            { "简体中文", "zh-CN" }
        };

        private LangManager()
        {
            _resourceManager = new ResourceManager("ParsecVDisplay.Languages.Lang", typeof(LangManager).Assembly);
        }

        public string this[string name]
        {
            get
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));

                return _resourceManager.GetString(name) ?? string.Empty;
            }
        }

        public void SetLanguage(string lang = "English")
        {
            if (string.IsNullOrEmpty(lang) || !DicLang.ContainsKey(lang))
                lang = "English";
            if (DicLang.ContainsKey(lang))
                lang = DicLang[lang];
            SetLangCode(lang);
        }

        public void SetLangCode(string langcode = "en-US")
        {
            if (string.IsNullOrEmpty(langcode) || !DicLang.ContainsValue(langcode))
                langcode = "en-US";
            var cultureInfo = new CultureInfo(langcode);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

        /// <summary>
        /// Use key to get string from resource file
        /// </summary>
        public static string Get(string key, params object[] args)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string format = Instance[key];
            return string.Format(format, args);
        }
    }

    public class LangExtension : MarkupExtension
    {
        public string Key { get; set; }

        public LangExtension(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding($"[{Key}]")
            {
                Source = LangManager.Instance,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
