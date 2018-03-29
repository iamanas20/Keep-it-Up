using GoalList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalList.Services
{
    class QuoteSelectorService
    {
        public static Quote GetRandomQuote()
        {
            Random r = new Random();
            int quoteIndex = r.Next(0, 63);
            return QuotesManager.MyListOfQuotes[quoteIndex];
        }
    }
}
