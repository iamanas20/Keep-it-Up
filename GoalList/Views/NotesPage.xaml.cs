using GoalList.Helpers;
using GoalList.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace GoalList.Views
{
    public sealed partial class NotesPage : Page, INotifyPropertyChanged
    {
        ObservableCollection<Note> notes;
        bool openedAtFirst;
        Brush noteBrush;
        public NotesPage()
        {
            InitializeComponent();
            notes = new ObservableCollection<Note>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (Note note in App.MyUser.Notes)
            {
                notes.Add(note);
            }
            openedAtFirst = false;

            noteBrush = new SolidColorBrush(Colors.White);
            myStackPanel.Background = noteBrush;
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

        /// <summary>
        /// Handles the AddButton Click which adds the note.
        /// </summary>
        /// <remarks>
        /// It adds the note to both the <c>ObservableCollection{Note}</c> and the <c>User</c>'s
        /// does this by calling the <see cref="NoteManipulation.AddNote(string, ObservableCollection{Note}, User, Color)"/>
        /// </remarks>
        /// <remarks>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NoteManipulation.AddNote(NewNoteTextBox.Text, notes, App.MyUser, (Color)noteBrush.GetValue(SolidColorBrush.ColorProperty));
            
            NewNoteTextBox.Text = String.Empty;
            myStackPanel.Background = new SolidColorBrush(Colors.White);

            foreach (StackPanel sp in (NewNoteColorFlyout.Content as StackPanel).Children)
            {
                foreach (ToggleButton tb in sp.Children)
                {
                    if (tb.IsChecked == true)
                    {
                        tb.IsChecked = false;
                        tb.Content = String.Empty;
                    }
                    if ((Color)tb.Background.GetValue(SolidColorBrush.ColorProperty) == Colors.White)
                    {
                        tb.IsChecked = true;
                        tb.Content = "\xE8FB";
                        noteBrush = tb.Background;
                    }
                }
            }
            
            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);

            myStackPanel.Visibility = Visibility.Collapsed;
            WriteNoteClone.Visibility = Visibility.Visible;
        }

        private async void DeleteNoteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NoteManipulation.Delete(notes, App.MyUser, sender);

            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);
        }

        private void NewNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CountTextBlock.Text = (300 - NewNoteTextBox.Text.Length).ToString();
            NewNoteTextBox.FontSize = NoteBodyFontSizeGenerator.GetNoteFontSize(NewNoteTextBox.Text);
        }

        protected async override void OnPointerExited(PointerRoutedEventArgs e)
        {
            App.MyUser.Notes = new ObservableCollection<Note>();
            foreach (Note note in notes)
            {
                App.MyUser.Notes.Add(note);
            }
            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);
        }

        private void NoteBodyTextBox_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) => 
            NoteManipulation.MakeTheCharactersLeftOnNoteVisible(sender, NotesGridView);

        private async void NoteBodyTextBox_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            NoteManipulation.MakeTheCharactersLeftOnNoteCollapsed(sender, NotesGridView);
            App.MyUser.Notes = new ObservableCollection<Note>();
            foreach (Note note in notes)
            {
                App.MyUser.Notes.Add(note);
            }
            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);
        }

        private void NoteBodyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NoteManipulation.UpdateTheCharactersLeftOnNote(sender, NotesGridView);
            var currentNote = notes.FirstOrDefault(n => n.NoteTag == Convert.ToInt16((sender as TextBox).Tag));
            currentNote.Body = (sender as TextBox).Text;
            (sender as TextBox).FontSize = NoteBodyFontSizeGenerator.GetNoteFontSize((sender as TextBox).Text);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.MyUser.Notes = new ObservableCollection<Note>();
            foreach (Note note in notes)
            {
                App.MyUser.Notes.Add(note);
            }
            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);
        }

        /// <summary>
        /// Unchecks the previously checked <c>ToggleButton</c>
        /// <remarks>
        /// <para>
        /// It does this by fetching all of the ToggleButtons:
        /// it's really tricky that only what you get is the <c>ToggleButton</c> as the sender
        /// so we get its parent then its parent's parent which is the outter most level <c>StackPanel</c>
        /// and then we loop through its inner most StackPanels and then we loop through their children
        /// which are of course the <c>ToggleButton</c>s. 
        /// </para>
        /// <para>
        /// And after that, we see if that ToggleButton is checked and if it is not <c>sender as ToggleButton</c>
        /// And so if it finds it it unchecks it and changes its content to <c>String.Empty</c>
        /// </para>
        /// <para>
        /// And so it changes the <c>(sender as ToggleButton).Content</c> to a check markz 
        /// </para>
        ///</remarks>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is ToggleButton checkedToggleButton)
            {
                try
                {
                    foreach (StackPanel stackPanel in ((checkedToggleButton.Parent as Panel).Parent as Panel).Children)
                    {
                        foreach (ToggleButton toggleButton in (stackPanel as Panel).Children)
                        {
                            if ((toggleButton != checkedToggleButton && (toggleButton.IsChecked == true)))
                            {
                                toggleButton.IsChecked = false;
                                toggleButton.Content = "";
                            }
                        }
                    }
                    checkedToggleButton.Content = "\xE8FB";
                    noteBrush = checkedToggleButton.Background;
                    myStackPanel.Background = noteBrush;
                    NewNoteColorFlyout.Hide();
                }
                catch (Exception)
                {
                    
                }
                
            }
        }
        
        /// <summary>
        /// Checks the white <c>ToggleButton</c> as a starting color for the NewNoteTextBox
        /// </summary>
        /// <param name="sender">Contains the object that fired the event.</param>
        /// <param name="e">Contains the event data of the event.</param>
        private void Flyout_Opened(object sender, object e)
        {
            if (!openedAtFirst)
            {
                WhiteNoteColorToggleButton.IsChecked = true;
                openedAtFirst = true;
            }
        }

        /// <summary>
        ///     Checks for the note's grid's background to update the flyout's button with the same background
        /// </summary>
        /// <remarks>
        ///     It does this by calling the <see cref="NoteManipulation.CheckTheSameColoredToggleButton(object, GridView)"/>
        ///     which -as described in its summary- gets the grid with the same tag as the flyout's content's tag
        ///     which is stackpanel and this stackpanel contains multiple stackpanels and
        ///     it searches all the buttons inside them to find which one is as the same
        ///     background as the grid.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flyout_Opened_1(object sender, object e) =>
            NoteManipulation.CheckTheSameColoredToggleButton(((sender as Flyout).Content as StackPanel), NotesGridView);

        /// <summary>
        /// Checks for the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            //TODO: updates the grid's background color, finds it and changes its background
            //      based on the sender as ToggleButton
            NoteManipulation.UpdateNoteColor((((sender as ToggleButton).Parent as Panel).Parent as Panel), (sender as ToggleButton).Background, NotesGridView, notes, App.MyUser);
            (sender as ToggleButton).Content = "\xE8FB";
            
            //uncheck all the other togglebuttons that were previously checked
            NoteManipulation.UnCheckAllColorToggleButtons(sender, NotesGridView);
            
            string userAsJson = Serializer.Serialize(App.MyUser);
            await Serializer.SaveUserAsync(userAsJson);
        }

        private void WriteNoteClone_Tapped(object sender, TappedRoutedEventArgs e)
        {
            myStackPanel.Visibility = Visibility.Visible;
            NewNoteTextBox.Focus(FocusState.Pointer);
        }

        private void MinimizeNoteButton_Click(object sender, RoutedEventArgs e)
        {
            myStackPanel.Visibility = Visibility.Collapsed;
            WriteNoteClone.Visibility = Visibility.Visible;
        }
    }
}
