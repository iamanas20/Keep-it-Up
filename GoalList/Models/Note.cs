using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace GoalList.Models
{
    class Note
    {
        private string body;
        public string Body { get => body; set => body = value; }

        private int noteTag;
        public int NoteTag { get => noteTag; set => noteTag = value; }

        private string charactersLeft;
        public string CharactersLeft { get => charactersLeft; set => charactersLeft = value; }

        private double noteFontSize;
        public double NoteFontSize { get => noteFontSize; set => noteFontSize = value; }
        
        private Color noteColor;
        public Color NoteColor { get => noteColor; set => noteColor = value; }
    }
}
