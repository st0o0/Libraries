using System.Windows;

namespace InputBoxLibrary.WPF.Messaging
{
    internal struct ButtonInfo
    {
        internal ButtonInfo(string buttonName, RoutedEventHandler eventHandler)
        {
            ButtonEventAction = eventHandler;
            ButtonName = buttonName;
        }

        internal RoutedEventHandler ButtonEventAction { get; private set; }

        internal string ButtonName { get; private set; }
    }
}