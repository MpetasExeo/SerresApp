
using MvvmHelpers;

using Newtonsoft.Json;

using SerresApp.Helpers;
using SerresApp.Interfaces;

using SerresApp.Models;

using SerresApp.Views;

using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SerresApp.Services
{
    public class GreekCitiesService : IGreekCitiesService
    {
        /// <summary>
        /// Returns the deserialized list of cities from the .json file.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableRangeCollection<POISlim>> GetGreekCities()
        {
            ObservableRangeCollection<POISlim> greekCities = new ObservableRangeCollection<POISlim>();

            //`+++ **Important** : πρέπει να έχω θέσει το Build Action από τα Properties του αρχείου json σε Embedder Resource.

            var assembly = typeof(GoogleMapsPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{Constants.JsonFolder}.{Constants.JsonTitle}_{Settings.TwoLetterLocaleCode[Settings.Language]}.json");

            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                var jsonString = await reader.ReadToEndAsync().ConfigureAwait(true);
                greekCities = JsonConvert.DeserializeObject<ObservableRangeCollection<POISlim>>(jsonString);
            }

            return greekCities;
        }
    }

}
