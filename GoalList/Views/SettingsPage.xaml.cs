using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoalList.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using GoalList.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;

namespace GoalList.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        // TODO WTS: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        private bool IsRefreshedTitleBar;
        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get { return _isLightThemeEnabled; }
            set { Set(ref _isLightThemeEnabled, value); }
        }

        private string _appDescription;
        public string AppDescription
        {
            get { return _appDescription; }
            set { Set(ref _appDescription, value); }
        }

        bool alreadyInitialized;

        public SettingsPage()
        {
            InitializeComponent();
            SoundEffectsToggleSwitch.IsOn = App.AreSoundEffectsAreOn;
            alreadyInitialized = true;
            IsRefreshedTitleBar = false;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            if (!IsRefreshedTitleBar)
            {
                #region TitleBarStyling
                SolidColorBrush _currentAccent = (SolidColorBrush)Application.Current.Resources["AccentButtonBackground"];
                ApplicationViewTitleBar titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ForegroundColor = Colors.White;
                titleBar.BackgroundColor = _currentAccent.Color;
                titleBar.ButtonBackgroundColor = _currentAccent.Color;
                titleBar.ButtonForegroundColor = Colors.White;
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = false;
                #endregion 
                IsRefreshedTitleBar = true;
            }
        }

        private void Initialize()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Only switch theme if value has changed (not on initialization)
            if (sender is ToggleSwitch toggleSwitch)
            {
                if (toggleSwitch.IsOn != ThemeSelectorService.IsLightThemeEnabled)
                {
                    await ThemeSelectorService.SwitchThemeAsync();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        Brush currentAccent;

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Brush selectedColor = null;
            if ((sender as Button).Tag.ToString() == "Accent")
            {
                selectedColor = (Brush)Application.Current.Resources["SystemControlBackgroundAccentBrush"];
                App.MyUser.IsAccentColorSelected = true;
            }
            else
            {
                selectedColor = (sender as Button).Background;
                App.MyUser.IsAccentColorSelected = false;
            }

            currentAccent = selectedColor;
            byte a = ((Color)currentAccent.GetValue(SolidColorBrush.ColorProperty)).A;
            byte g = ((Color)currentAccent.GetValue(SolidColorBrush.ColorProperty)).G;
            byte r = ((Color)currentAccent.GetValue(SolidColorBrush.ColorProperty)).R;
            byte b = ((Color)currentAccent.GetValue(SolidColorBrush.ColorProperty)).B;
            Application.Current.Resources["AccentButtonBackground"] = selectedColor;
            Application.Current.Resources["SystemControlHighlightListAccentHighBrush"] = new SolidColorBrush(new Color { A = 170, R = r, G = g, B = b });

            App.MyUser.MyColor.A = a;
            App.MyUser.MyColor.R = r;
            App.MyUser.MyColor.G = g;
            App.MyUser.MyColor.B = b;
            
            #region TitleBarStyling
            SolidColorBrush _currentAccent = (SolidColorBrush)Application.Current.Resources["AccentButtonBackground"];
            ApplicationViewTitleBar titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ForegroundColor = Colors.White;
            titleBar.BackgroundColor = _currentAccent.Color;
            titleBar.ButtonBackgroundColor = _currentAccent.Color;
            titleBar.ButtonForegroundColor = Colors.White;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            #endregion

            string userAsJsonString = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJsonString);

            //to update the title row's background
            Frame.Navigate(typeof(SettingsPage));
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }

        private async void SoundEffectsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (alreadyInitialized)
            {
                App.AreSoundEffectsAreOn = (sender as ToggleSwitch).IsOn ? true : false;
                string soundChoiceAsJson = Serializer.Serialize(App.AreSoundEffectsAreOn);
                await Serializer.SaveSoundChoiceAsync(soundChoiceAsJson);
            }
        }
    }
}
