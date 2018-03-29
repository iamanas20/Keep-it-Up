using GoalList.Helpers;
using GoalList.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using System.Numerics;
using Windows.Storage;
using Newtonsoft.Json;
using Windows.ApplicationModel;
using GoalList.Services;
using System.Threading.Tasks;

namespace GoalList.Views
{
    //TODO: Create a content and timing feature for the goals
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        ObservableCollection<Goal> allGoals;

        /// <summary>
        /// This is the entry code of the page, I intanciate the allGoals <c>ObservableCollection</c> 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            allGoals = new ObservableCollection<Goal>();
        }
         
        /// <summary>
        ///     Handles the event of this page being navigated to from other pages!
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Well first of all, <c>try</c> to retrieve the user from the stream, 
        ///         of course if there is no stream created there will be an <c>Exception</c> raised!
        ///         And then (if nominal) assign it to the <c>App</c> level encapsulated field <c>App.MyUser</c>
        ///     </para>
        ///     <para>
        ///         check <c>if(App.MyUser != null)</c> which means that everything was nominal
        ///         (if not create a new instance of the object to insure that the use is legit)
        ///         and we managed to get the user from the stream, thus I get the color saved in the
        ///         <c>App.MyUser.MyColor</c> and update the Accent color of the app! And then
        ///         gets the items in the <c>App.MyUser.Goals</c> and assign them to the
        ///         <c>ObservableCollection</c> allGoals toupdate the LisView related to it, 
        ///         (you can remark that the done goals are not done by default due to 
        ///         the xaml schemas"/>
        ///     </para>
        ///     <para>
        ///         Updates the Accent colors of the app to the ones that are saved and retrieved!
        ///     </para>
        ///     <para>
        ///         Checks if the sounds are on or not! and then the sound effects are based on that!
        ///     </para>
        ///     <para>
        ///         Calls <c>MakeDelayAsync()</c> to create a delay to let the listview completely build the items 
        ///         and therefore update the done items!
        ///     </para>
        ///     <para>
        ///         Updates the newGoalBackground to its typical background color!
        ///     </para>
        /// </remarks>
        /// <param name="e">Event data of the event</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string userAsJsonString = await Serializer.ReadUserAsync();
                App.MyUser = Serializer.Deserialize<User>(userAsJsonString);
            }
            catch (Exception)
            {
                // Do nothing about the exception cuz then the file does not exist!
            }
            if (App.MyUser != null)
            {
                if (!App.MyUser.IsAccentColorSelected)
                {
                    if (App.MyUser.MyColor.A != 0)
                    {
                        Application.Current.Resources["AccentButtonBackground"] = new SolidColorBrush(App.MyUser.MyColor);
                        Application.Current.Resources["SystemControlHighlightListAccentHighBrush"] = new SolidColorBrush(new Color { A = 170, R = App.MyUser.MyColor.R, G = App.MyUser.MyColor.G, B = App.MyUser.MyColor.B });
                    }
                }
                if (App.MyUser.Goals != null)
                {
                    foreach (Goal goal in App.MyUser.Goals)
                    {
                        allGoals.Add(goal);
                    }
                }
            }
            else
            {
                App.MyUser = new User();
            }

            #region UpdatingTheThemeColorOfTheApp

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

            titleBackground.Background = (Brush)Application.Current.Resources["AccentButtonBackground"];
            NewGoalDescriptionTextBox.BorderBrush = (Brush)Application.Current.Resources["AccentButtonBackground"];

            Brush myBrush = (Brush)Application.Current.Resources["AccentButtonBackground"];
            Color _myColor = (Color)myBrush.GetValue(SolidColorBrush.ColorProperty);
            newGoalBackground.Background = new SolidColorBrush(new Color { A = 170, R = _myColor.R, G = _myColor.G, B = _myColor.B });
            #endregion

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size(410, 450));

            App.AreSoundEffectsAreOn = true;
            try
            {
                string soundChoiceAsJson = await Serializer.ReadSoundChoiceAsync();
                App.AreSoundEffectsAreOn = Serializer.Deserialize<bool>(soundChoiceAsJson);
            }
            catch (Exception)
            {
                //Do nothing about the exception cause the file doesn't exist!!
            }

            await MakeDelayAsync();
            foreach (Goal goal in allGoals)
            {
                if (goal.IsDone)
                {
                    GoalManipulation.MakeGoalDone(GoalsListView, goal);
                }
            }
            
            myBrush = (Brush)Application.Current.Resources["AccentButtonBackground"];
            _myColor = (Color)myBrush.GetValue(SolidColorBrush.ColorProperty);
            newGoalBackground.Background = new SolidColorBrush(new Color { A = 170, R = _myColor.R, G = _myColor.G, B = _myColor.B });
        }

        /// <summary>
        /// Creates a delay to let the ListView successfully 
        /// build the items in order to update the done items.
        /// </summary>
        /// <returns></returns>
        private async Task MakeDelayAsync()
        {
            await Task.Delay(200);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Handles the KeyUp event to add the <c>Goal</c> to the <c>ListView</c></summary>
        /// <remarks>If the key that raised the event is <c>VirtualKey.Enter</c> then call the 
        /// <see cref="AddButton_Click(object, RoutedEventArgs)"/> event handler method.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"> Contains the event data of the event, used the <c>e.Key</c> property
        /// to see if the key preesed is Enter</param>
        private void MainPage_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddButton_Click(new object(), new Windows.UI.Xaml.RoutedEventArgs());
            }
        }

        /// <summary>
        ///     Property Changed event handler method
        /// </summary>
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
        
        /// <summary> Add new goal to the user's list Of Goals </summary>
        /// <remarks>
        /// This is where the new goal is added to the ObservableCollection allGoals,
        /// and the ListView is automatically updated,
        /// <see cref="GoalManipulation.AddGoal(ObservableCollection{Goal}, User, string)"/> 
        /// then a cash register sound will be played
        /// <see cref="PlayMediaFilesFromPackageFolderService.PlayMedia(string, string)"/>
        /// And the App.MyUser is saved as a Json string in the stream,
        /// <see cref="Serializer"/></remarks>
        /// <param name="sender"> The data of the XAML control (<c>Button</c>) that raised the event</param>
        /// <param name="e">Event data of the event</param>
        private async void AddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Add the goal
            GoalManipulation.AddGoal(allGoals, App.MyUser, String.IsNullOrEmpty(NewGoalDescriptionTextBox.Text) ? null : NewGoalDescriptionTextBox.Text);
            NewGoalDescriptionTextBox.Text = String.Empty;

            if (App.AreSoundEffectsAreOn)
            {
                //Play the cash register sound effect
                PlayMediaFilesFromPackageFolderService.PlayMedia("Assets", "cashregister.mp3");
            }

            //Save the user to the stream!
            string userAsJsonString = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJsonString);
        }

        /// <summary>
        /// This is the DoneButton event handler, done if undone and undone if done!
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         First of all a <c>goal</c> is retreived as the one that
        ///         has the same tag as the <c>sender</c> that raised the event,
        ///         using a <see cref="System.Linq"/> metohd compact expression.
        ///         <example>
        ///             <code>
        ///                 var myGoal = allgoals.FirstOrDefault(g => g.GoalTag == (sender as Button).Tag));
        ///             </code>
        ///         </example>
        ///     </para>
        ///     <para>
        ///         Then I check if that goal <c>!IsDone</c>, And if it is then call the
        ///         <see cref="GoalManipulation.MakeGoalDone(ObservableCollection{Goal}, User, ListView, object)"/>
        ///         method to make the goal done!
        ///     </para>    
        ///     <para>
        ///         At the same level I check for the oposite, if the goal <c>IsDone</c>
        ///         if it is then undo the goal calling the <see cref="GoalManipulation.MakeGoalUnDone(ListView, Goal)"/>
        ///     </para>
        ///     <para>
        ///         And then I save the user!
        ///     </para>
        /// </remarks>
        /// <param name="sender">Holds the data about the <c>Button</c> that raised the event!</param>
        /// <param name="e">Event data about the event</param>
        private async void DoneButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Border goes green and change the content to an accept
            //to get the border with the tag!
            //https://stackoverflow.com/questions/15189715/how-to-access-a-control-inside-the-data-template-in-c-sharp-metro-ui-in-the-code

            //get the goal with the same tag as the (sender as Button)
            var goal = allGoals.FirstOrDefault(g => g.GoalTag == Convert.ToInt16((sender as Button).Tag));
            if (!goal.IsDone)
            {
                GoalManipulation.MakeGoalDone(allGoals, App.MyUser, GoalsListView, sender);

                if (App.AreSoundEffectsAreOn)
                {
                    //play the bell sound effect!
                    PlayMediaFilesFromPackageFolderService.PlayMedia("Assets", "smallbell.mp3");
                }
            }
            else
            {
                GoalManipulation.MakeGoalUnDone(GoalsListView, goal);
            }

            string userAsJsonString = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJsonString);
        }

        /// <summary>
        /// This is the deleteButton event handler method, delete the goal!
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         by calling the <see cref="GoalManipulation.DeleteGoal(ListView, ObservableCollection{Goal}, User, object)"/>
        ///         it perform the functionality to delete the goal and (undo if done)
        ///         <see cref="GoalManipulation.DeleteGoal(ListView, ObservableCollection{Goal}, User, object)"/>
        ///     </para>
        ///     <para>
        ///         Then at last I save the user!
        ///     </para>
        /// </remarks>
        /// <param name="sender">Contains the data of the XAML control (<c>Button</c>) that raised the event</param>
        /// <param name="e">Event data of the event</param>
        private async void DeleteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Delete the goal
            GoalManipulation.DeleteGoal(GoalsListView, allGoals, App.MyUser, (sender as Button).Tag);

            string userAsJsonString = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJsonString);
        }
    }
}
