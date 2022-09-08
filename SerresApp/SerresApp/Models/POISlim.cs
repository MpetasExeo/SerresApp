
using Newtonsoft.Json;

using SerresApp.Helpers;
using SerresApp.Interfaces;
using SerresApp.Resources;
using SerresApp.Services;

using SerresApp.ViewModels;

using SerresApp.Views;

using System;

using System.Linq;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SerresApp.Models
{
    public class POISlim
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("ContactInfo")]
        public ContactInfo ContactInfo { get; set; }

        public bool HasImage
        {
            get { return DependencyService.Get<IImageChecker>().DoesImageExist($"img{CategoryId}_{Id}.jpg"); }
        }

        public string ImageUrl { get { return HasImage ? $"img{CategoryId}_{Id}.jpg" : $"c{CategoryId}2x.png"; } }

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
                switch(CategoryId)
                {
                    case 0:
                        return AppResources.NatureAndWildlife;
                    case 1:
                        return AppResources.TangibleCulturalHeritage;
                    case 2:
                        return AppResources.IntangibleCulturalHeritage;
                    default:
                        return string.Empty;
                }
                ;
            }
        }

        public bool ShowDistance
        {
            get
            {
                if((Distance < 1500) && (Distance > 0.1))
                {
                    return true;
                }
                return false;
            }
        }

        public string DistanceFromUser { get => Distance > 1500 ? "∞" : Distance.ToString(); }

        public double CalculateDistanceFromUser()
        {
            if(Latitude == null || Longitude == null || Settings.Position is null)
            {
                return -1;
            }
            return Math.Round(
                Location.CalculateDistance(
                    Settings.Position.Latitude,
                    Settings.Position.Longitude,
                    (double)Latitude,
                    (double)Longitude,
                    DistanceUnits.Kilometers),
                1);
        }

        public async Task NavigateToDetailsAsync()
        {
            ItemDetailsPage detailsPage = new ItemDetailsPage();

            GreekCitiesService contentService = new GreekCitiesService();
            var pois = await contentService.GetGreekCities();

            var poi = pois.Where(x => x.Id == this.Id).FirstOrDefault().Clone<POISlim, POI>();
            if(ContactInfo != null)
            {
                poi.ContactInfo = this.ContactInfo;
            }

            ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel(poi);
            detailsPage.BindingContext = itemDetailsViewModel;

            await Shell.Current.Navigation.PushAsync(detailsPage, true).ConfigureAwait(false);
        }
    }
}
