

using MvvmHelpers.Commands;

using SerresApp.Database;
using SerresApp.Helpers;
using SerresApp.Interfaces;
using SerresApp.Models;

using SerresApp.Services;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SerresApp.ViewModels
{
    public class ItemDetailsViewModel : BaseViewModel
    {
        #region Properties,Services & Commands
        private ObservableCollection<CustomImage> _images;
        private int _currentImage;
        private LayoutState _mainState;

        public IPOIRepository Database { get; set; }
        public ICommand AddToFavouritesCommand { get; set; }

        private CancellationToken ct = new CancellationToken();

        public LayoutState MainState { get => _mainState; set => SetAndRaise(ref _mainState , value); }

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        private Distances distances;

        public Distances Distances { get => distances; set => SetAndRaise(ref distances , value); }

        public ObservableCollection<CustomImage> Images
        {
            get { return _images; }
            set
            {
                SetAndRaise(ref _images , value);
                ;
            }
        }

        public int CurrentImage { get { return _currentImage; } set { SetAndRaise(ref _currentImage , value); } }

        private IWeatherService WeatherService { get; set; }

        public IUserLocationService UserLocationService { get; set; }

        public IDistancesService DistancesService { get; set; }

        private POI _selectedPOI = new POI();

        public POI SelectedPOI { get => _selectedPOI; set { SetAndRaise(ref _selectedPOI , value); } }
        private Temperatures temperatures;

        public Thickness Thickness
        {
            get
            {
                return SelectedPOI.HasImage ? new Thickness(16 , 396 , 16 , 16) : new Thickness(16 , 50 , 16 , 16);

            }
        }


        public Temperatures Temperatures
        {
            get => temperatures;
            set
            {
                temperatures = value;
                SetAndRaise(ref temperatures , value);
            }
        }

        public Location CityPosition { get; set; }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetAndRaise(ref _isFavorite , value);
        }



        public bool ShowContactInfo
        {
            get
            {
                return HasTelephone || HasEmail || HasAddress;
            }
        }

        public bool HasEmail
        {
            get
            {
                return SelectedPOI.ContactInfo.Email != string.Empty;
            }
        }

        public bool HasAddress
        {
            get
            {
                return SelectedPOI.ContactInfo.Address != string.Empty;
            }
        }

        public bool HasTelephone
        {
            get
            {
                return SelectedPOI.ContactInfo.Tel != string.Empty;
            }
        }


        #endregion

        static readonly Stopwatch timer = new Stopwatch();

        public ItemDetailsViewModel(POI poi)
        {
            //LoadImages();
            PropertiesInit();
            timer.Restart();
            SelectedPOI = poi;
            if (SelectedPOI.Latitude != null && SelectedPOI.Longitude != null)
            {
                CityPosition = new Location((double)poi.Latitude , (double)poi.Longitude);
            }
            timer.Stop();
            var time = timer.Elapsed;
        }

        private async Task Navigate()
        {
            if (CityPosition is null)
            {
                return;
            }
            // ανοίγω directions
            await Xamarin.Essentials.Map.OpenAsync(CityPosition , new MapLaunchOptions
            {
                Name = SelectedPOI.Title ,
                NavigationMode = NavigationMode.Driving
            }).ConfigureAwait(true);
        }
        private void PropertiesInit()
        {
            //UserLocationService = new UserLocationService();
            timer.Start();
            Database = new POIRepository();
            NavigateCommand = new AsyncCommand(Navigate);
            WeatherService = new WeatherService();
            DistancesService = new DistancesService();
            AddToFavouritesCommand = new AsyncCommand(AddToFavourites);
            timer.Stop();
            var time = timer.Elapsed;
        }

        private async Task AddToFavourites()
        {
            try
            {
                if (IsFavorite)
                {
                    UserExperiencePrompts.RemovedFromFavorites();
                    await Database.DeleteItemAsync(Poi.Id);
                }
                else
                {
                    UserExperiencePrompts.AddedToFavorites();
                    await Database.SaveItemAsync(Poi);
                }
                IsFavorite = !IsFavorite;
            }
            catch (System.Exception ex)
            {
                var msg = ex.Message;
            }
        }

        private POIDatabaseItem _poi;
        public POIDatabaseItem Poi
        {
            get => _poi;
            set => SetAndRaise(ref _poi , value);
        }
        bool _showInfoPanel = true;
        public bool ShowInfoPanel
        {
            get => _showInfoPanel;
            set => SetAndRaise(ref _showInfoPanel , value);
        }
        public AsyncCommand NavigateCommand { get; set; }

        public override void Load() { LoaderNotifier.Load(_ => InitializationTask()); }


        private async Task InitializationTask()
        {
            Poi = SelectedPOI.Clone<POI , POIDatabaseItem>();
            var st = SelectedPOI.Content;

            static string StripHTML(string input)
            {
                return Regex.Replace(input , "<.*?>" , String.Empty);
            }

            var d = StripHTML(SelectedPOI.Content);
            SelectedPOI.Content = d;
            RaisePropertyChanged(nameof(SelectedPOI));
            //Poi.Id = await Database.GetItemIdAsync(Poi);
            IsFavorite = await Database.GetItemIdAsync(Poi) != -1;

            MainState = LayoutState.Loading;

            IsBusy = true;

            if (Settings.Position is null)
            {
                UserLocationService locationService = new UserLocationService();
                var userLocation = await locationService.GetUserLocationAsync(ct);
            }

            if (SelectedPOI.Latitude != null && SelectedPOI.Longitude != null)
            {
                Temperatures = await WeatherService.GetCurrentWeatherAsync(CityPosition , ct);
                Distances = await DistancesService.GetDistancesFromUserAsync(CityPosition , Settings.Position);
            }
            else
            {
                ShowInfoPanel = false;
            }
            IsBusy = false;

            MainState = LayoutState.None;
        }
    }
}
