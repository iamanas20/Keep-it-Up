using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalList.Models
{
    public class Goal
    {
        private string description;
        public string Description { get => description; set => description = value; }

        private bool isDone;
        public bool IsDone { get => isDone; set => isDone = value; }

        private int goalTag;
        public int GoalTag { get => goalTag; set => goalTag = value; }
    }
}
