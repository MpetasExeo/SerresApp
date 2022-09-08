
using SerresApp.ViewModels;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SerresApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailsPage : ContentPage
    {
        public ItemDetailsPage() { InitializeComponent(); }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ItemDetailsViewModel vm = this.BindingContext as ItemDetailsViewModel;
            vm.Load();

            //detailContainer.FadeTo(1 , 200 , Easing.CubicInOut);
            //detailContainer.TranslateTo(0 , 0 , 200 , Easing.CubicInOut);

            //descriptionContainer.FadeTo(1 , 350 , Easing.CubicInOut);
            //descriptionContainer.TranslateTo(0 , 0 , 350 , Easing.CubicInOut);


            try
            {
                await Task.Delay(150).ConfigureAwait(true);

                infoFrame.Opacity = 0;
                infoFrame.TranslationY = 600;

                title.Opacity = 0;
                title.TranslationY = 25;

                region.Opacity = 0;
                //region.TranslationY = 25;


                details.Opacity = 0;
                details.TranslationY = 50;

                contactInfo.TranslationY = 600;
                DetailsView.TranslationY = 600;

                await Task.WhenAll(
                    DetailsView.TranslateTo(0 , 0 , 750 , Easing.SinOut) ,
                    infoFrame.TranslateTo(0 , 0 , 750 , Easing.SinOut) ,
                    contactInfo.TranslateTo(0 , 0 , 750 , Easing.SinOut))
                    .ConfigureAwait(true);


                await Task.WhenAll(

                    title.TranslateTo(0 , 0 , 450 , Easing.SinOut) ,
                    details.TranslateTo(0 , 0 , 450 , Easing.SinOut) ,
                    title.FadeTo(1 , 450 , Easing.SinOut) ,
                    details.FadeTo(1 , 450 , Easing.SinOut) ,
                    region.FadeTo(1 , 750 , Easing.SinOut) ,
                    infoFrame.FadeTo(1 , 450 , Easing.SinOut) ,
                    contactInfo.FadeTo(1 , 450 , Easing.SinOut))
                    .ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}