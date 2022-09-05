using SerresApp.Models;

using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace SerresApp.Interfaces
{
    public interface IWeatherService
    {
        Task<Temperatures> GetCurrentWeatherAsync(Location location , CancellationToken cancellationToken);
    }
}
