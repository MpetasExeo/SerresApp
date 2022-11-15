
using MvvmHelpers;
using MvvmHelpers.Commands;

using SerresApp.Interfaces;
using SerresApp.Models;
using SerresApp.Services;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

using Command = MvvmHelpers.Commands.Command;
using Map = Xamarin.Forms.GoogleMaps.Map;

namespace SerresApp.ViewModels
{
    public partial class GoogleMapsViewModel : BaseViewModel
    {
        #region Properties
        public IGreekCitiesService GreekCitiesService { get; set; }
        //public IContentService _contentService { get; set; }
        public IWeatherService WeatherService { get; set; }
        public ICommand GetCitiesCommand { get; set; }
        public Map GoogleMap { get; set; } = new Map();
        public ObservableRangeCollection<Pin> Pins { get; set; } = new ObservableRangeCollection<Pin>();
        public ICommand NavToDetailsCommand { get; set; }
        public ICommand NavigateToPlaceCommand { get; set; }


        private bool mapLoaded;
        /// <summary>
        /// Αν έχει φορτώσει ήδη ο χάρτης. π.χ. για να μην κάνω κάθε φορά τα Pins από την αρχή
        /// </summary>
        public bool MapLoaded
        {
            get => mapLoaded;
            set
            {
                SetAndRaise(ref mapLoaded , value);
                Task.Run(async () => await AfterInitializationTask());
            }
        }

        private bool constructorFinished;
        /// <summary>
        /// Λόγω LazyLoading ο constructor τρέχει μόνο την πρώτη φορά. Στο τέλος του θέτω ConstructorFinished = true.
        /// </summary>
        public bool ConstructorFinished
        {
            get => constructorFinished;
            set
            {
                SetAndRaise(ref constructorFinished , value);
                //Task.Run(() => Load());
            }
        }

        private bool filtersClicked;

        /// <summary>
        /// Η μεταβλητή που ελέγχει αν είναι ανοιχτό το πλαίσιο με τα φίλτρα. Αν ανοίξει, θέτω HasSelectedPlace = false.
        /// </summary>
        public bool FiltersClicked
        {
            get => filtersClicked;
            set
            {
                if (value)
                {
                    HasSelectedPlace = !value;
                    IsDrawerOpen = value;
                }
                SetAndRaise(ref filtersClicked , value);

            }
        }

        private bool hasSelectedPlace;

        /// <summary>
        /// Η μεταβλητή που ελέγχει αν είναι έχω επιλέξει (κλικάρει) πάνω σε κάποιο Pin του χάρτη. Αν ανοίξει, θέτω FiltersClicked = false.
        /// </summary>
        public bool HasSelectedPlace
        {
            get => hasSelectedPlace;
            set
            {
                if (value)
                {
                    FiltersClicked = !value;
                    IsDrawerOpen = value;
                }
                SetAndRaise(ref hasSelectedPlace , value);
            }
        }

        private POISlim selectedPlace;

        /// <summary>
        /// Το object στο οποίο αποθηκεύω τις τιμές από το αντικείμενο που επιλέγω στον χάρτη.
        /// </summary>
        public POISlim SelectedPlace
        {
            get => selectedPlace;
            set
            {
                SetAndRaise(ref selectedPlace , value);
            }
        }

        /// <summary>
        /// Η λίστα στην οποία αποθηκεύω όλα τα σημεία ενδιαφέροντος ώστε να τα διαχειριστώ με όποιον τρόπο θέλω.
        /// </summary>
        public ObservableRangeCollection<POISlim> POIS { get; set; }
        private LayoutState _temperaturesState;
        public LayoutState TemperaturesState
        {
            get => _temperaturesState;
            set => SetAndRaise(ref _temperaturesState , value);
        }
        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();
        #endregion

        public GoogleMapsViewModel()
        {
            ServicesAndPropertiesInit();
            UICommandsInit();
            ConstructorFinished = true;
        }


        /// <summary>
        /// Η εντολή που τρέχει όταν επιλέξω "Λεπτομέρειες" στον drawer που ανοίγει όταν επιλέξω ένα POI στον χάρτη.
        /// </summary>
        /// <param name="poi"></param>
        /// <returns></returns>
        private async Task NavigateToDetails(POISlim poi)
        {
            await poi.NavigateToDetailsAsync();
        }


        /// <summary>
        /// Η διαδικασία που τρέχει όταν επιλέγεται το συγκεκριμένο Viewmodel από το tabs menu. 
        /// </summary>
        /// <returns></returns>
        private async Task InitializationTask()
        {
            DefineMapStyle();

            List<Task> tasks = new List<Task>
            {

                 MapInitialization()
            };
            await Task.WhenAll(tasks).ConfigureAwait(true);


            MapLoaded = true;
        }


        /// <summary>
        /// Θέτω το style του χάρτη ( Dark || Light ) από το αντίστοιχο Json.
        /// </summary>
        private void DefineMapStyle()
        {
            var assembly = typeof(GoogleMapsViewModel).GetTypeInfo().Assembly;

            var currentTheme = Application.Current.RequestedTheme;

            var assemblyStream = System.String.Empty;

            if (currentTheme == OSAppTheme.Light)
            {
                assemblyStream = "SerresApp.Json.travelstyle.json";

            }
            else
            {
                assemblyStream = "SerresApp.Json.darkstyle.json";
            }

            var stream = assembly.GetManifestResourceStream(assemblyStream);

            string styleFile;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            GoogleMap.MapStyle = MapStyle.FromJson(styleFile);
        }


        /// <summary>
        /// Αφού έχω κάνει create τον χάρτη τρέχω τις εντολές που αναθέτουν τις λειτουργίες του χάρτη.
        /// </summary>
        /// <returns></returns>
        private async Task AfterInitializationTask()
        {
            List<Task> tasks = new List<Task>
            {
                GetCitiesAsync()
            };

            UICommandsInit();

            await Task.WhenAll(tasks);
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        /// <summary>
        /// Εδώ ορίζω τις λειτουργίες του χάρτη.
        /// </summary>
        private void UICommandsInit()
        {
            GoogleMap.PinClicked += GoogleMap_PinClicked;
            NavToDetailsCommand = new AsyncCommand<POISlim>(NavigateToDetails);
            NavigateToPlaceCommand = new AsyncCommand(NavigateToPlace);
            OpenFiltersDrawerCommand = new Command(OpenFiltersDrawer);
        }



        /// <summary>
        /// Η εντολή που τρέχει όταν επιλέξω "Οδηγίες" στον drawer που ανοίγει όταν επιλέξω ένα POI στον χάρτη.
        /// </summary>
        /// <returns></returns>
        private async Task NavigateToPlace()
        {
            if (SelectedPlace.Latitude is null || SelectedPlace.Longitude is null)
            {
                return;
            }

            Location loc = new Location((double)SelectedPlace.Latitude , (double)SelectedPlace.Latitude);
            // ανοίγω directions
            await Xamarin.Essentials.Map.OpenAsync(loc , new MapLaunchOptions
            {
                Name = SelectedPlace.Title ,
                NavigationMode = NavigationMode.Driving
            }).ConfigureAwait(true);
        }


        public List<Category> Categories { get; set; } = new List<Category>();

        private bool isDrawerOpen;

        public bool IsDrawerOpen
        {
            get { return isDrawerOpen; }
            set
            {
                if (value)
                {
                    SetAndRaise(ref hasSelectedPlace , !value);
                }
                SetAndRaise(ref isDrawerOpen , value);

            }
        }

        public ICommand OpenFiltersDrawerCommand { get; set; }

        private void OpenFiltersDrawer() { FiltersClicked = true; }

        private void ServicesAndPropertiesInit()
        {
            GreekCitiesService = new GreekCitiesService();
            WeatherService = new WeatherService();
            Categories categories = new Categories();
            Categories = new List<Category>(categories.CategoriesList);
        }


        /// <summary>
        /// Ορίζω την συμπεριφορά του χάρτη. πχ αρχική τοποθεσία
        /// </summary>
        /// <returns></returns>
        private async Task MapInitialization()
        {
            await Task.Run(() =>
            {
                GoogleMap = new Xamarin.Forms.GoogleMaps.Map
                {
                    MyLocationEnabled = true ,
                };
                DefineMapStyle();
                GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                CameraPosition cameraPosition = new CameraPosition(new Position(40.57444939059151 , 22.995289142328364) , 13);
                GoogleMap.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                GoogleMap.UiSettings.MyLocationButtonEnabled = false;
            }).ConfigureAwait(true);
        }

        #region Pin Clicked 

        private void GoogleMap_PinClicked(object sender , PinClickedEventArgs e) => PinSelected(e);

        private void PinSelected(PinClickedEventArgs e)
        {
            e.Handled = true;

            try
            {
                if (SelectedPlace == null)
                {
                    FindSelectedPlace(e.Pin.Position.Latitude , e.Pin.Position.Longitude);
                    return;
                }

                if (SelectedPlace.Longitude == e.Pin.Position.Longitude && SelectedPlace.Latitude == e.Pin.Position.Latitude)
                {
                    HasSelectedPlace = true;
                    return;
                }

                FindSelectedPlace(e.Pin.Position.Latitude , e.Pin.Position.Longitude);

            }
            catch (System.Exception ex)
            {
                var st = ex.Message;
                //throw;
            }
        }


        /// <summary>
        /// Ανατρέχω στην λίστα των POIs ώστε να βρω το στοιχείο με το συγκεκριμένο lat, lon που παίρνει απο το tap στον χάρτη.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        private void FindSelectedPlace(double lat , double lon)
        {
            TemperaturesState = LayoutState.Loading;
            IsBusy = true;

            SelectedPlace =
                POIS.Where(s => s.Latitude == lat).Where(s => s.Longitude == lon).FirstOrDefault();

            if (!(SelectedPlace is null))
            {
                HasSelectedPlace = true;
            }

            //GetSelectedPlaceTemperature(lat , lon);

        }
        #endregion


        List<POISlim> _pOIsList;
        public List<POISlim> POIsList
        {
            get => _pOIsList;
            set => SetAndRaise(ref _pOIsList , value);
        }

        /// <summary>
        /// Calls the CitiesService, populates <see cref="Pins">Pins list</see> and creates a <see cref="Pin">Pin</see> for each item in the list
        /// </summary>
        private async Task GetCitiesAsync()
        {
            POIS = await GreekCitiesService.GetGreekCities();

            foreach (var poi in POIS)
            {
                var lat = poi.Latitude;
                var lng = poi.Longitude;

                if (lat != null & lng != null)
                {


                    Pin pin = new Pin()
                    {
                        Position = new Position((double)lat , (double)lng) ,
                        Label = poi.Title ,
                        Type = PinType.Place ,
                        Icon = BitmapDescriptorFactory.FromBundle(poi.CategoryId.ToString())
                    };
                    Pins.Add(pin);

                }
            }

            ThreadPool.QueueUserWorkItem(o => AddPinsToMap());

        }

        /// <summary>
        /// Adds pins to map using the UI Thread()
        /// </summary>
        private void AddPinsToMap()
        {
            foreach (var pin in Pins)
            {
                Device.BeginInvokeOnMainThread(() =>
                GoogleMap.Pins.Add(pin));
            }
        }
    }
}
