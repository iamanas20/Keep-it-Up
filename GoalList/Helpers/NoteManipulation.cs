using GoalList.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace GoalList.Helpers
{
    class NoteManipulation
    {
        public static void AddNote(string noteBody, ObservableCollection<Note> notes, User myUser, Windows.UI.Color noteColorBrush)
        {
            // 1 - Check if the text is null or empty
            if (!String.IsNullOrEmpty(noteBody))
            {
                // 2 - add the note to both the collections
                notes.Add(new Note { Body = noteBody, CharactersLeft = (300 - noteBody.Length).ToString(), NoteColor = noteColorBrush });
                myUser.Notes.Add(new Note { Body = noteBody, CharactersLeft = (300 - noteBody.Length).ToString(), NoteColor = noteColorBrush });
                // 3 - retag both the collections
                Retag(notes, myUser);
            }
        }

        private static void Retag(ObservableCollection<Note> notes, User myUser) 
        {
            int i = 1;
            foreach (Note note in notes)
            {
                note.NoteTag = i;
                i++;
            }
            i = 1;
            foreach (Note note in myUser.Notes)
            {
                note.NoteTag = i;
                i++;
            }
        }

        public static void Delete(ObservableCollection<Note> notes, User myUser, object sender)
        {
            if (sender is Button myButton)
            {
                notes.Remove(notes.FirstOrDefault(n => n.NoteTag == Convert.ToInt16(myButton.Tag)));
                myUser.Notes.Remove(notes.FirstOrDefault(n => n.NoteTag == Convert.ToInt16(myButton.Tag)));
            }
        }

        public static void MakeTheCharactersLeftOnNoteVisible(object sender, GridView notesGridView)
        {
            TextBlock charactersOnNoteLeftTextBlock = (TextBlock)FindRepeaterChildControl.FindChildControl<TextBlock>(notesGridView, Convert.ToInt32((sender as TextBox).Tag));
            charactersOnNoteLeftTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public static void MakeTheCharactersLeftOnNoteCollapsed(object sender, GridView notesGridView)
        {
            TextBlock charactersLeftOnNoteTextBlock = (TextBlock)FindRepeaterChildControl.FindChildControl<TextBlock>(notesGridView, Convert.ToInt32((sender as TextBox).Tag));
            charactersLeftOnNoteTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public static void UpdateTheCharactersLeftOnNote(object sender, GridView notesGridView)
        {
            TextBlock charactersLeftOnNoteTextBlock = (TextBlock)FindRepeaterChildControl.FindChildControl<TextBlock>(notesGridView, Convert.ToInt16((sender as TextBox).Tag));
            charactersLeftOnNoteTextBlock.Text = (300 - (sender as TextBox).Text.Length).ToString();
        }

        public static void UpdateNoteColor(object sender, Brush currentBrush, GridView myGridView, ObservableCollection<Note> allNotes, User myUser)
        {
            //1. Find the grid!
            var myGrid = (Grid)FindRepeaterChildControl.FindChildControl<Grid>(myGridView, Convert.ToInt16((sender as StackPanel).Tag));

            //2. Update the grid's color
            myGrid.Background = currentBrush;

            //3. Save it to the collections
            Note currentNote = allNotes.FirstOrDefault(n => n.NoteTag == Convert.ToInt16((sender as StackPanel).Tag));
            currentNote.NoteColor = (Color)myGrid.Background.GetValue(SolidColorBrush.ColorProperty);

            currentNote = myUser.Notes.FirstOrDefault(n => n.NoteTag == Convert.ToInt32((sender as StackPanel).Tag));
            currentNote.NoteColor = (Color)myGrid.Background.GetValue(SolidColorBrush.ColorProperty);
        }

        public static void CheckTheSameColoredToggleButton(object sender, GridView notesGridView)
        {
            //1. Get the grid (and its background) with the same tag as this stackpanel (sender)
            Grid myGrid = (Grid)FindRepeaterChildControl.FindChildControl<Grid>(notesGridView, Convert.ToInt32((sender as StackPanel).Tag));

            //2. look for the togglebuttons inside the flyout of this button(.flyout.content)
            //      and then if the background matches, check!!
            foreach (StackPanel sp in (sender as StackPanel).Children)
            {
                foreach (ToggleButton tglbtn in sp.Children)
                {
                    if ((Color)tglbtn.Background.GetValue(SolidColorBrush.ColorProperty) == (Color)myGrid.Background.GetValue(SolidColorBrush.ColorProperty))
                    {
                        // 3. change the button's content to check
                        tglbtn.Content = "\xE8FB";
                        return;
                    }
                }
            }
        }

        public static void UnCheckAllColorToggleButtons(object sender, GridView myGridView)
        {
            foreach (StackPanel sp in (((sender as ToggleButton).Parent as Panel).Parent as Panel).Children)
            {
                foreach (ToggleButton tglb in sp.Children)
                {
                    if (!Equals(tglb, (sender as ToggleButton)))
                    {
                        tglb.Content = String.Empty;
                        tglb.IsChecked = false;
                    }
                }
            }
        }
    }
}
