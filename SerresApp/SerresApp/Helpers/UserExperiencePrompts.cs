using SerresApp.Interfaces;
using SerresApp.Models;
using SerresApp.Resources;

namespace SerresApp.Helpers
{
    public static class UserExperiencePrompts
    {
        public static void NoInternetConnectionPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(AppResources.NoInternetConnectionFound);
        }

        public static void AddedToFavorites()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(AppResources.Favourited);
        }

        public static void RemovedFromFavorites()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(AppResources.RemovedFromFavourites);
        }

        public static void NoGPSPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(AppResources.DeviceLocationNA);
        }

        public static void ApplicationCannotStartPrompt()
        {
            IToastMessage toastMessage = new Toaster();
            toastMessage.MakeToastAsync(AppResources.ApplicationCannotStart);
        }

    }
}
