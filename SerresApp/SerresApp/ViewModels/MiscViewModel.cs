using SerresApp.Views;

using Sharpnado.TaskLoaderView;

using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using Command = MvvmHelpers.Commands.Command;

namespace SerresApp.ViewModels
{
    public class MiscViewModel : BaseViewModel
    {
        public MiscViewModel()
        {
            PropertiesInit();
        }

        #region Properties
        public ICommand OpenLanguageDrawerCommand { get; set; }
        public ICommand AboutAppCommand { get; set; }
        public ICommand OpenThemeDrawerCommand { get; set; }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        bool _themeClicked;
        public bool ThemeClicked
        {
            get => _themeClicked;
            set
            {
                SetAndRaise(ref _themeClicked , value);
                if (value)
                {
                    LanguageClicked = !value;
                    IsDrawerOpen = value;
                }
            }
        }


        bool _languageClicked;
        public bool LanguageClicked
        {
            get => _languageClicked;
            set
            {
                SetAndRaise(ref _languageClicked , value);
                if (value)
                {
                    ThemeClicked = !value;
                    IsDrawerOpen = value;
                }
            }
        }

        bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get => _isDrawerOpen;
            set
            {
                SetAndRaise(ref _isDrawerOpen , value);
            }
        }


        #endregion

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        private async Task InitializationTask()
        {
            await Task.Delay(0);
        }

        void PropertiesInit()
        {
            OpenThemeDrawerCommand = new Command(OpenThemeDrawer);
            OpenLanguageDrawerCommand = new Command(OpenLanguageDrawer);
            AboutAppCommand = new Command(GoToAboutApp);
        }

        public void OpenLanguageDrawer() { LanguageClicked = true; }
        public void OpenThemeDrawer() { ThemeClicked = true; }
        public async void GoToAboutApp()
        {
            AboutPage aboutPage = new AboutPage();
            await Shell.Current.Navigation.PushAsync(aboutPage , true).ConfigureAwait(false);
        }

    }
}
