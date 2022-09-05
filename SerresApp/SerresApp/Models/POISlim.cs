using SerresApp.Helpers;
using SerresApp.Interfaces;
using SerresApp.Services;
using SerresApp.ViewModels;
using SerresApp.Views;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;

namespace SerresApp.Models
{
    public class POISlim
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }


        public string ImageUrl { get { return $"img{CategoryId}{Id}.jpg"; } }

        [JsonProperty(propertyName: "Content")]
        public string Content { get; set; }

        [JsonProperty("Latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double? Longitude { get; set; }
        public double Distance => CalculateDistanceFromUser();
        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }
        public string Category
        {
            get
            {
                return Models.Categories.CategoriesList.Where(c => c.Id == CategoryId).Select(c => c.Text).FirstOrDefault();
            }
        }
        public bool ShowDistance
        {
            get
            {
                if ((Distance < 1500) && (Distance > 0.1))
                {
                    return true;
                }
                return false;
            }
        }
        public string DistanceFromUser
        {
            get => Distance > 1500 ? "∞" : Distance.ToString();
        }

        public double CalculateDistanceFromUser()
        {
            if (Latitude == null || Longitude == null || Settings.Position is null)
            {
                return -1;
            }
            return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , (double)Latitude , (double)Longitude , DistanceUnits.Kilometers) , 1);
        }

        public async Task NavigateToDetailsAsync()
        {
            if (!RequiredChecks.HasInternetConnection())
            {

                IToastMessage toastMessage = new Toaster();
                await toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);
                return;
            }

            ItemDetailsPage detailsPage = new ItemDetailsPage();



            var contentService = new GreekCitiesService();
            var pois = await contentService.GetGreekCities();

            var poi = pois.Where(x => x.Id == this.Id).FirstOrDefault().Clone<POISlim , POI>();

            ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel(poi);
            detailsPage.BindingContext = itemDetailsViewModel;

            await Shell.Current.Navigation.PushAsync(detailsPage , true).ConfigureAwait(false);

        }

    }
}
