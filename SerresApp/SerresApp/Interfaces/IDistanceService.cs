
using SerresApp.Models;

using System.Threading.Tasks;

using Xamarin.Essentials;

namespace SerresApp.Interfaces
{
    public interface IDistancesService
    {
        Task<Distances> GetDistancesFromUserAsync(Location PlacePosition , Location UserPosition);
    }
}
