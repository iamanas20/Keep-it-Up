using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalList.Models
{
    public class Quote
    {
        private string body;
        public string Body { get => body; set => body = value; }

        private string author;
        public string Author { get => author; set => author = value; }
    }

    class QuotesManager
    {
        public static List<Quote> MyListOfQuotes
        {
            get => new List<Quote>() {
                new Quote() {Body ="\"The only way to do great work is to love what you do\"" , Author = "-Steve Jobs"},
                new Quote() {Body ="\"If someone works 40 hours a week and you work at the same field 100 hours a week you'll achieve in 4 months what takes them a year or so to obtain\"", Author="-Elon Musk"},
                new Quote() {Body ="\"Whatever the mind of man can conceive and believe, it can achieve.\"", Author="-Napoleon Hill"},
                new Quote() {Body ="\"Strive not to be a success, but rather to be of value.\"", Author="-Albert Einstein"},
                new Quote() {Body ="\"Two roads diverged in a wood, and I—I took the one less traveled by, And that has made all the difference.\"", Author="-Robert Frost"},
                new Quote() {Body ="\"I attribute my success to this: I never gave or took any excuse.\"", Author="-Florence Nightingale"},
                new Quote() {Body ="\"You miss 100% of the shots you don’t take.\"", Author="-Wayne Gretzky"},
                new Quote() {Body ="\"I've missed more than 9000 shots in my career. I've lost almost 300 games. 26 times I've been trusted to take the game winning shot and missed. I've failed over and over and over again in my life. And that is why I succeed", Author="-Michael Jordan"},
                new Quote() {Body ="\"The most difficult thing is the decision to act, the rest is merely tenacity. ", Author="-Amelia Earhart"},
                new Quote() {Body ="\"Every strike brings me closer to the next home run.\"", Author="-Babe Ruth"},
                new Quote() {Body ="\"Life isn't about getting and having, it's about giving and being.\"", Author="-Kevin Kruse"},
                new Quote() {Body ="\"Life is what happens to you while you’re busy making other plans.\"", Author="-John Lennon"},
                new Quote() {Body ="\"We become what we think about.\"", Author="-Earl Nightingale"},
                new Quote() {Body ="\"Twenty years from now you will be more disappointed by the things that you didn’t do than by the ones you did do, so throw off the bowlines, sail away from safe harbor, catch the trade winds in your sails.  Explore, Dream, Discover.\"", Author="-Mark Twain"},
                new Quote() {Body ="\"Life is 10% what happens to me and 90% of how I react to it.\"", Author="-Charles Swindoll"},
                new Quote() {Body ="\"The most common way people give up their power is by thinking they don’t have any.\"", Author="-Alice Walker"},
                new Quote() {Body ="\"The mind is everything. What you think you become.\"", Author="-Buddha"},
                new Quote() {Body ="\"The best time to plant a tree was 20 years ago. The second best time is now.\"", Author="-Chinese Proverb"},
                new Quote() {Body ="\"An unexamined life is not worth living.\"", Author="-Socrates"},
                new Quote() {Body ="\" Eighty percent of success is showing up.\"", Author="-Woody Allen"},
                new Quote() {Body ="\"Your time is limited, so don’t waste it living someone else’s life.\"", Author="-Steve Jobs"},
                new Quote() {Body ="\"Winning isn’t everything, but wanting to win is.\"", Author="-Vince Lombardi"},
                new Quote() {Body ="\"I am not a product of my circumstances. I am a product of my decisions.\"", Author="-Stephen Covey"},
                new Quote() {Body ="\"Every child is an artist.  The problem is how to remain an artist once he grows up.\"", Author="-Pablo Picasso"},
                new Quote() {Body ="\"You can never cross the ocean until you have the courage to lose sight of the shore.\"", Author="-Christopher Columbus"},
                new Quote() {Body ="\"I’ve learned that people will forget what you said, people will forget what you did, but people will never forget how you made them feel.\"", Author="-Maya Angelou"},
                new Quote() {Body ="\"Either you run the day, or the day runs you.\"", Author="-Jim Rohn"},
                new Quote() {Body ="\"Whether you think you can or you think you can’t, you’re right.\"", Author="-Henry Ford"},
                new Quote() {Body ="\"The two most important days in your life are the day you are born and the day you find out why.\"", Author="-Mark Twain"},
                new Quote() {Body ="\"Whatever you can do, or dream you can, begin it.  Boldness has genius, power and magic in it.\"", Author="-Johann Wolfgang von Goethe"},
                new Quote() {Body ="\"The best revenge is massive success.\"", Author="-Frank Sinatra"},
                new Quote() {Body ="\"People often say that motivation doesn’t last. Well, neither does bathing.  That’s why we recommend it daily.\"", Author="-Zig Ziglar"},
                new Quote() {Body ="\"Life shrinks or expands in proportion to one's courage.\"", Author="-Anais Nin"},
                new Quote() {Body ="\"If you hear a voice within you say “you cannot paint,” then by all means paint and that voice will be silenced.\"", Author="-Vincent Van Gogh"},
                new Quote() {Body ="\"There is only one way to avoid criticism: do nothing, say nothing, and be nothing.\"", Author="-Aristotle"},
                new Quote() {Body ="\"Ask and it will be given to you; search, and you will find; knock and the door will be opened for you.\"", Author="-Jesus Christ"},
                new Quote(){Body ="\"The only person you are destined to become is the person you decide to be.\"", Author="-Ralph Waldo Emerson"},
                new Quote(){Body ="\"Go confidently in the direction of your dreams.  Live the life you have imagined.\"", Author="-Henry David Thoreau"},
                new Quote(){Body ="\"When I stand before God at the end of my life, I would hope that I would not have a single bit of talent left and could say, I used everything you gave me.\"", Author="-Erma Bombeck"},
                new Quote(){Body ="\"Few things can help an individual more than to place responsibility on him, and to let him know that you trust him.\"", Author="-Booker T. Washington"},
                new Quote(){Body ="\"Certain things catch your eye, but pursue only those that capture the heart.\"", Author="- Ancient Indian Proverb"},
                new Quote(){Body ="\"Believe you can and you’re halfway there.\"", Author="-Theodore Roosevelt"},
                new Quote(){Body ="\"Everything you’ve ever wanted is on the other side of fear.\"", Author="-George Addair"},
                new Quote(){Body ="\"We can easily forgive a child who is afraid of the dark; the real tragedy of life is when men are afraid of the light.\"", Author="-Plato"},
                new Quote(){Body ="\"Start where you are. Use what you have.  Do what you can.\"", Author="-Arthur Ashe"},
                new Quote(){Body ="\"When I was 5 years old, my mother always told me that happiness was the key to life.  When I went to school, they asked me what I wanted to be when I grew up.  I wrote down ‘happy’.  They told me I didn’t understand the assignment, and I told them they didn’t understand life.\"", Author="-John Lennon"},
                new Quote(){Body ="\"Fall seven times and stand up eight.\"", Author="-Japanese Proverb"},
                new Quote(){Body ="\"When one door of happiness closes, another opens, but often we look so long at the closed door that we do not see the one that has been opened for us.\"", Author="-Helen Keller"},
                new Quote(){Body ="\"Everything has beauty, but not everyone can see.\"", Author="-Confucius"},
                new Quote(){Body ="\"How wonderful it is that nobody need wait a single moment before starting to improve the world.\"", Author="-Anne Frank"},
                new Quote(){Body ="\"When I let go of what I am, I become what I might be.\"", Author="-Lao Tzu"},
                new Quote(){Body ="\"Life is not measured by the number of breaths we take, but by the moments that take our breath away.\"", Author="-Maya Angelou"},
                new Quote(){Body ="\"Happiness is not something readymade.  It comes from your own actions.\"", Author="-Dalai Lama"},
                new Quote(){Body ="\"If you're offered a seat on a rocket ship, don't ask what seat! Just get on.\"", Author="-Sheryl Sandberg"},
                new Quote(){Body ="\"First, have a definite, clear practical ideal; a goal, an objective. Second, have the necessary means to achieve your ends; wisdom, money, materials, and methods. Third, adjust all your means to that end.\"", Author="-Aristotle"},
                new Quote(){Body ="\"If the wind will not serve, take to the oars.\"", Author="-Latin Proverb"},
                new Quote(){Body ="\"You can’t fall if you don’t climb.  But there’s no joy in living your whole life on the ground.\"", Author="-Unknown"},
                new Quote(){Body ="\"We must believe that we are gifted for something, and that this thing, at whatever cost, must be attained.\"", Author="-Marie Curie"},
                new Quote(){Body ="\"You take your life in your own hands, and what happens? A terrible thing, no one to blame.\"", Author="-Erica Jong"},
                new Quote(){Body ="\"A person who never made a mistake never tried anything new.\"", Author="-ALbert Einstein"},
                new Quote(){Body ="\"The person who says it cannot be done should not interrupt the person who is doing it.\"", Author="-Chinese Proverb"},
                new Quote(){Body ="\"I would rather die of passion than of boredom.\"", Author="-Vincent van Gogh"},
                new Quote(){Body ="\"It does not matter how slowly you go as long as you do not stop.\"", Author="-Confucius"},
                new Quote(){Body ="\"Dream big and dare to fail.\"", Author = "-Norman Vaughan"},
            };
        }
    }
}
