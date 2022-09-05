
using SerresApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SerresApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionViewPage : ContentPage
    {
        public CollectionViewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CollectionViewViewModel vm = this.BindingContext as CollectionViewViewModel;
            if (!vm.IsLoaded)
            {
                vm.IsLoaded = true;
            }
            //vm.Load();
        }
    }
}