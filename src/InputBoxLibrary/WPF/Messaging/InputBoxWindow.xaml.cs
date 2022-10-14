using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace InputBoxLibrary.WPF.Messaging
{
    /// <summary>
    /// Interaction logic for InputBoxWindow.xaml
    /// </summary>
    public partial class InputBoxWindow : Window
    {
        private ButtonInfo cancel;
        private ButtonInfo ok;
        private ButtonInfo no;
        private ButtonInfo yes;
        private MessageBoxResult result;
        private string inputresult;

        public string InputResult { get => inputresult; set => inputresult = value; }

        public InputBoxWindow()
        {
            InitializeComponent();
            InitButtonInfos();
        }

        public InputBoxWindow(string messageBoxText) : this()
        {
            SetValues(messageBoxText, "InputBox", "google.com", MessageBoxButton.OK);
        }

        public InputBoxWindow(string messageBoxText, string caption) : this()
        {
            SetValues(messageBoxText, caption, "google.com", MessageBoxButton.OK);
        }

        public InputBoxWindow(string messageBoxText, string caption, string url, MessageBoxButton button) : this()
        {
            SetValues(messageBoxText, caption, url, button);
            OpenBrowser("opera", url);
        }

        public InputBoxWindow(string messageBoxText, string caption, string url, MessageBoxButton button, PrimaryColor primary, SecondaryColor secondary, IBaseTheme theme) : this()
        {
            theme ??= Theme.Dark;
            SetTheme(theme, primary, secondary);
            SetValues(messageBoxText, caption, url, button);
            OpenBrowser("opera", url);
        }

        private void SetTheme(IBaseTheme theme, PrimaryColor primaryColor = default, SecondaryColor secondaryColor = SecondaryColor.DeepPurple)
        {
            Color primary = SwatchHelper.Lookup[(MaterialDesignColor)primaryColor];
            Color secondary = SwatchHelper.Lookup[(MaterialDesignColor)secondaryColor];
            ResourceDictionaryExtensions.SetTheme(Application.Current.Resources, Theme.Create(theme, primary, secondary));
        }

        private void SetValues(string messageBoxText, string caption, string url, MessageBoxButton messageBoxButton)
        {
            this.TextBox.Text = messageBoxText;
            this.Title = caption;
            this.Hyperlink.NavigateUri = new Uri(url);
            AddbuttonToUniformGrid(CreateButtons(CreateButtonInfos(messageBoxButton)), "UniformGrid");
        }

        private void AddbuttonToUniformGrid(List<Button> buttons, string gridName)
        {
            ArgumentNullException.ThrowIfNull(gridName, nameof(gridName));

            foreach (var i in buttons)
            {
                UniformGrid.Children.Add(i);
            }
        }

        private List<ButtonInfo> CreateButtonInfos(MessageBoxButton messageBoxButton)
        {
            var list = new List<ButtonInfo>();
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    list.Add(ok);
                    break;

                case MessageBoxButton.OKCancel:
                    list.Add(ok);
                    list.Add(cancel);
                    break;

                case MessageBoxButton.YesNo:
                    list.Add(yes);
                    list.Add(no);
                    break;

                case MessageBoxButton.YesNoCancel:
                    list.Add(yes);
                    list.Add(no);
                    list.Add(cancel);
                    break;
            }
            return list;
        }

        private List<Button> CreateButtons(List<ButtonInfo> buttonInfos)
        {
            var result = new List<Button>();
            foreach (var i in buttonInfos)
            {
                result.Add(CreateButton(i));
            }
            return result;
        }

        private Button CreateButton(ButtonInfo buttonInfo)
        {
            Button button = new()
            {
                Content = buttonInfo.ButtonName
            };
            button.Click += buttonInfo.ButtonEventAction;
            button.Margin = new Thickness(5);
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            return button;
        }

        private void InitButtonInfos()
        {
            cancel = new ButtonInfo("Cancel", (s, e) => { result = MessageBoxResult.Cancel; this.Close(); });
            ok = new ButtonInfo("OK", (s, e) => { result = MessageBoxResult.OK; this.Close(); });
            yes = new ButtonInfo("Yes", (s, e) => { result = MessageBoxResult.Yes; this.Close(); });
            no = new ButtonInfo("No", (s, e) => { result = MessageBoxResult.No; this.Close(); });
        }

        internal MessageBoxResult CustomShowDialog()
        {
            this.ShowDialog();
            InputResult = this.Input.Text;
            return result;
        }

        private void OpenBrowser(string browserName, string url)
        {
            var processes = Process.GetProcessesByName(browserName);
            var path = processes.FirstOrDefault()?.MainModule?.FileName;
            Process.Start(path, url);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            OpenBrowser("opera", e.Uri.AbsoluteUri);
        }
    }
}
