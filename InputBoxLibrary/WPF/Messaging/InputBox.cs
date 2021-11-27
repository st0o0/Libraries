﻿using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace InputBoxLibrary.WPF.Messaging
{
    public static class InputBox
    {
        public static (MessageBoxResult, string) Show(string messageBoxText)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText);
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(string messageBoxText, string caption)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption);
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(string messageBoxText, string caption, string url, MessageBoxButton button)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption, url, button);
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(string messageBoxText, string caption, string url, MessageBoxButton button, PrimaryColor primary, SecondaryColor secondary, IBaseTheme theme)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption, url, button, primary, secondary, theme);
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(Window owner, string messageBoxText)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText)
            {
                Owner = owner
            };
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(Window owner, string messageBoxText, string caption)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption)
            {
                Owner = owner
            };
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(Window owner, string messageBoxText, string caption, string url, MessageBoxButton button)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption, url, button)
            {
                Owner = owner
            };
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }

        public static (MessageBoxResult, string) Show(Window owner, string messageBoxText, string caption, string url, MessageBoxButton button, PrimaryColor primary, SecondaryColor secondary, IBaseTheme theme)
        {
            InputBoxWindow inputBoxWindow = new InputBoxWindow(messageBoxText, caption, url, button, primary, secondary, theme)
            {
                Owner = owner
            };
            var t = inputBoxWindow.CustomShowDialog();
            return (t, inputBoxWindow.InputResult);
        }
    }
}