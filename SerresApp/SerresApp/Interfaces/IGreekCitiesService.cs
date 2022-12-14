using MvvmHelpers;

using SerresApp.Models;

using System.Threading.Tasks;

namespace SerresApp.Interfaces
{
    public interface IGreekCitiesService
    {
        /// <summary>
        /// Returns the cities deserialized from the json file.
        /// </summary>
        /// <returns></returns>
        Task<ObservableRangeCollection<POISlim>> GetGreekCities();
    }
}
