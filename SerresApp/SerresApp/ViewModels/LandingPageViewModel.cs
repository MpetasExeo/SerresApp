
using SerresApp.Helpers;
using SerresApp.Models;

using System.Windows.Input;

using Xamarin.Forms;

using Command = MvvmHelpers.Commands.Command;

namespace SerresApp.ViewModels
{
    public class LandingPageViewModel : BaseViewModel
    {
        public LandingPageViewModel()
        {
            StartAppCommand = new Command(StartApplication);
        }

        public ICommand StartAppCommand { get; set; }

        private void StartApplication()
        {
            if (RequiredChecks.HasInternetConnection())
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                UserExperiencePrompts.NoInternetConnectionPrompt();
            }
        }
    }
}
