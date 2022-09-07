
using SerresApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SerresApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailsPage : ContentPage
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ItemDetailsViewModel vm = this.BindingContext as ItemDetailsViewModel;
            vm.Load();

            detailContainer.FadeTo(1 , 200 , Easing.CubicInOut);
            detailContainer.TranslateTo(0 , 0 , 200 , Easing.CubicInOut);

            descriptionContainer.FadeTo(1 , 350 , Easing.CubicInOut);
            descriptionContainer.TranslateTo(0 , 0 , 350 , Easing.CubicInOut);

        }
    }
}