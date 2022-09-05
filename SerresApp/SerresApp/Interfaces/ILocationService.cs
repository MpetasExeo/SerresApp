using System.Threading.Tasks;

using Xamarin.Essentials;

namespace SerresApp.Interfaces
{
    internal interface ILocationService
    {
        Task<Location> GetCurrentLocation();
        Task<PermissionStatus> RequestLocationPermission();
        Task<PermissionStatus> CheckLocationPermission();
        Task<bool> IsLocationPermissionGranted();
    }
}
