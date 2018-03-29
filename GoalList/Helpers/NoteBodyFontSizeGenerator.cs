using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalList.Helpers
{
    class NoteBodyFontSizeGenerator
    {
        public static double GetNoteFontSize(string body)
        {
            if (body.Length != 0 && body.Length <= 20)
            {
                return 36;
            }
            if (body.Length > 20 && body.Length <= 60)
            {
                return 28;
            }
            if (body.Length > 60 && body.Length < 150)
            {
                return 20;
            }
            else
            {
                return 15;
            }
        }
    }
}
