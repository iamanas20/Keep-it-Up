using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace GoalList.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        Color myAccent1 = Color.FromArgb(App.MyUser.MyColor.A, App.MyUser.MyColor.R, App.MyUser.MyColor.G, App.MyUser.MyColor.B);
        Color myAccent2 = Color.FromArgb(170, App.MyUser.MyColor.R, App.MyUser.MyColor.G, App.MyUser.MyColor.B);
        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            this.InitializeComponent();
        }
    }
}
