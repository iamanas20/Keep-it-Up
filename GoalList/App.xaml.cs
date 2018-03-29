using System;

using GoalList.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using GoalList.Models;
using GoalList.Helpers;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GoalList
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Quote _MyQuote => QuoteSelectorService.GetRandomQuote();

        private static Quote myQuote;
        internal static Quote MyQuote { get => myQuote; set => myQuote = value; }

        private static User myUser;
        internal static User MyUser { get => myUser; set => myUser = value; }

        private static bool areSoundEffectsAreOn;
        internal static bool AreSoundEffectsAreOn { get => areSoundEffectsAreOn; set => areSoundEffectsAreOn = value; }

        private Lazy<ActivationService> _activationService;
        private ActivationService ActivationService => _activationService.Value;

        internal static bool IsAlreadyBeenRefreshed;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            //Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
            App.MyUser = new User();
            App.MyQuote = _MyQuote;

            App.IsAlreadyBeenRefreshed = false;
        }
        
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!e.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(e);
            }
        }

        /// <summary>
        /// Invoked when the application is activated by some means other than normal launching.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Views.ShellPage());
        }
    }
}
