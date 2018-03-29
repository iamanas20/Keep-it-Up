using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace GoalList.Models
{
    class User
    {
        public User()
        {
            this.Goals = new ObservableCollection<Goal>();
            this.Notes = new ObservableCollection<Note>();
        }

        public Color MyColor;

        public bool IsAccentColorSelected;

        private ObservableCollection<Goal> goals;

        public ObservableCollection<Goal> Goals
        {
            get { return goals; }
            set { goals = value; }
        }

        private ObservableCollection<Note> notes;

        public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set { notes = value; }
        }
    }
}
