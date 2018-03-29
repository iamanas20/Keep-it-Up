using GoalList.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GoalList.Helpers
{
    class GoalManipulation
    {
        /// <summary>
        /// Adds the goal to the two collections and then it retags them starting by one!
        /// </summary>
        /// <param name="goalsList">Contains the allGoals Collection that's been bound to the ListView in the <c>MainPage.xaml</c></param>
        /// <param name="myUser">Contains the App.MyUser, even though it's application based variable it's more convenient to just pass it as a parameter</param>
        /// <param name="goalDescription">this is the string that's been generated from the NewGoalTextBox </param>
        public static void AddGoal(ObservableCollection<Goal> goalsList, User myUser, string goalDescription)
        {
            if (goalDescription != null)
            {
                goalsList.Add(new Goal() { Description = goalDescription });
                myUser.Goals.Add(new Goal() { Description = goalDescription });
                Retag(goalsList, myUser);
            }
        }

        /// <summary>
        /// Retags the collections (both) to let the ability to track down certain controls within
        /// the items in the ListView of Goals!
        /// </summary>
        /// <param name="_goalsList">Contains the allGoals <c>ObservableCollection</c> of type <c>Goal</c></param>
        /// <param name="_myUser">Contains the App.MyUser</param>
        private static void Retag(ObservableCollection<Goal> _goalsList, User _myUser)
        {
            int i = 1;
            foreach (Goal goal in _goalsList)
            {
                goal.GoalTag = i;
                i++;
            }
            i = 1;
            foreach (Goal goal in _myUser.Goals)
            {
                goal.GoalTag = i;
                i++;
            }
        }

        /// <summary>
        /// Deletes the goal with the same tag as the one in the ListView.
        /// </summary>
        /// <param name="myListView">Contains the ListView to undo the goal if it's done.</param>
        /// <param name="goalsList">Contains the ObservableCollection of type Goal</param>
        /// <param name="myUser">Contains the App level User</param>
        /// <param name="tag">Contains The Tag that's been retrieved from the ListView.</param>
        public static void DeleteGoal(ListView myListView, ObservableCollection<Goal> goalsList, User myUser, object tag)
        {
            var wantedGoalToDelete = goalsList.FirstOrDefault(g => g.GoalTag == Convert.ToInt16(tag));
            if (wantedGoalToDelete.IsDone)
                MakeGoalUnDone(myListView, wantedGoalToDelete);
            goalsList.Remove(wantedGoalToDelete);
            myUser.Goals.Remove(wantedGoalToDelete);
        }

        /// <summary>
        /// Makes the goal -with the same tag as the <c>_sender</c> which is the delete button- done .
        /// <para>
        ///     First of all it searches for the border - with the same tag as the <c>_sender</c>
        ///     it find this border by calling the <see cref="FindRepeaterChildControl.FindChildControl{T}(DependencyObject, int)"/>
        /// </para>
        /// <para>
        ///     Then it changes the border's background to light green, and also the <c>_sender</c>'s content to check.
        /// </para>
        /// <para>
        ///     And then it fetches for the goal in both the lists that has the same goal tag as the one
        ///     that wanted to be done in the ListView
        /// </para>
        /// </summary>
        /// <param name="goals"></param>
        /// <param name="_user"></param>
        /// <param name="myListView"></param>
        /// <param name="_sender"></param>
        public static void MakeGoalDone(ObservableCollection<Goal> goals, User _user, ListView myListView, object _sender)
        {
            var border = FindRepeaterChildControl.FindChildControl<Border>(myListView,
                    Convert.ToInt16((_sender as Button).Tag)) as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(255, 115, 255, 81));
            (_sender as Button).Content = String.Empty;
            (_sender as Button).Content = "\xE8FB";
            var doneItemOfObservableCollection = goals.FirstOrDefault(g => g.GoalTag == Convert.ToInt32((_sender as Button).Tag));
            doneItemOfObservableCollection.IsDone = true;
            var doneItemOfUsersCollection = _user.Goals.FirstOrDefault(g => g.GoalTag == Convert.ToInt16((_sender as Button).Tag));
            doneItemOfUsersCollection.IsDone = true;
        }

        /// <summary>
        /// This is the second version of the <seealso cref="GoalManipulation.MakeGoalDone(ObservableCollection{Goal}, User, ListView, object)"/>
        /// in this version it's just the navigation update of the goals
        /// </summary>
        /// <param name="myListView"></param>
        /// <param name="goal"></param>
        public static void MakeGoalDone(ListView myListView, Goal goal)
        {
            var border = FindRepeaterChildControl.FindChildControl<Border>(myListView,
                    Convert.ToInt16(goal.GoalTag)) as Border;
            var button = FindRepeaterChildControl.FindChildControl<Button>(myListView, Convert.ToInt16(goal.GoalTag)) as Button;
            border.Background = new SolidColorBrush(Color.FromArgb(255, 115, 255, 81));
            button.Content = String.Empty;
            button.Content = "\xE8FB";
            goal.IsDone = true;
        }

        /// <summary>
        /// makes the goal undone by returning it to the nominal stage.
        /// </summary>
        /// <param name="myListView"></param>
        /// <param name="goal"></param>
        public static void MakeGoalUnDone(ListView myListView, Goal goal)
        {
            var button = FindRepeaterChildControl.FindChildControl<Button>(myListView, Convert.ToInt16(goal.GoalTag)) as Button;
            var border = FindRepeaterChildControl.FindChildControl<Border>(myListView, Convert.ToInt16(goal.GoalTag)) as Border;
            border.Background = new SolidColorBrush(Colors.White);
            button.Content = String.Empty;
            button.Content = "\xE739";
            (goal as Goal).IsDone = false;
        }
    }
}
