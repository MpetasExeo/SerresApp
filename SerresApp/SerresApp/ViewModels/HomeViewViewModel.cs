

using MvvmHelpers;
using MvvmHelpers.Commands;

using SerresApp.Helpers;
using SerresApp.Interfaces;

using SerresApp.Models;
using SerresApp.Services;

using Sharpnado.TaskLoaderView;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SerresApp.ViewModels
{
    public partial class HomeViewViewModel : BaseViewModel
    {
        #region Properties

        ICommand loadMoreExperiencesCommand = null;
        public ICommand LoadMoreExperiencesCommand
        {
            get { return loadMoreExperiencesCommand; }
            set
            {
                if (loadMoreExperiencesCommand != value)
                {
                    loadMoreExperiencesCommand = value;
                    RaisePropertyChanged("loadMoreExperiencesCommand ");
                }
            }
        }

        ICommand loadMoreHighlightsCommand = null;
        public ICommand LoadMoreHighlightsCommand
        {
            get { return loadMoreHighlightsCommand; }
            set
            {
                if (loadMoreHighlightsCommand != value)
                {
                    loadMoreHighlightsCommand = value;
                    RaisePropertyChanged("loadMoreHighlightsCommand");
                }
            }
        }

        ICommand loadMoreUsefulCommand = null;
        public ICommand LoadMoreUsefulCommand
        {
            get { return loadMoreUsefulCommand; }
            set
            {
                if (loadMoreUsefulCommand != value)
                {
                    loadMoreUsefulCommand = value;
                    RaisePropertyChanged("loadMoreUsefulCommand");
                }
            }
        }

        bool isRefreshingExperiences = false;
        public bool IsRefreshingExperiences
        {
            get => isRefreshingExperiences;
            set
            {
                SetAndRaise(ref isRefreshingExperiences , value);
            }
        }

        protected internal bool isRefreshingUseful = false;
        public bool IsRefreshingUseful
        {
            get => isRefreshingUseful;
            set
            {
                SetAndRaise(ref isRefreshingUseful , value);
            }
        }

        bool isRefreshingHighlights = false;
        public bool IsRefreshingHighlights
        {
            get => isRefreshingHighlights;
            set
            {
                SetAndRaise(ref isRefreshingHighlights , value);
            }
        }


        bool _canLoadMoreExperiences = true;
        public bool CanLoadMoreExperiences
        {
            get
            {
                return _canLoadMoreExperiences;
            }
            set
            {
                SetAndRaise(property: ref _canLoadMoreExperiences , value);
            }
        }
        bool _canLoadMoreUseful = true;
        public bool CanLoadMoreUseful
        {
            get
            {
                return _canLoadMoreUseful;
            }
            set
            {
                SetAndRaise(property: ref _canLoadMoreUseful , value);
            }
        }

        bool _canLoadMoreHighlights = true;
        public bool CanLoadMoreHighlights
        {
            get
            {
                return _canLoadMoreHighlights;
            }
            set
            {
                SetAndRaise(property: ref _canLoadMoreHighlights , value);
            }
        }

        private bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get { return _isDrawerOpen; }
            set => SetAndRaise(ref _isDrawerOpen , value);
        }
        private bool _isLoaded;

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                SetAndRaise(ref _isLoaded , value);
            }
        }


        public ICommand OpenDrawerCommand { get; set; }
        public ICommand NavToDetailsCommand { get; set; }

        #region Filtered Results properties

        private List<Category> _categories;

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                SetAndRaise(ref _categories , value);
                SelectedCategories = Categories.Where(x => x.IsSelected == true).Select(x => x.Id).ToList();
            }
        }

        private List<int> _selectedCategories;
        public List<int> SelectedCategories
        {
            get => _selectedCategories;
            set
            {
                SetAndRaise(ref _selectedCategories , value);
            }
        }


        private ObservableRangeCollection<POISlim> _useful;
        public ObservableRangeCollection<POISlim> Useful
        {
            get => _useful;
            set
            {
                SetAndRaise(ref _useful , value);
                //RaisePropertyChanged(nameof(IsEmptyList));
            }
        }

        private ObservableRangeCollection<POISlim> _highlights;
        public ObservableRangeCollection<POISlim> Highlights
        {
            get => _highlights;
            set
            {
                SetAndRaise(ref _highlights , value);
                //RaisePropertyChanged(nameof(IsEmptyList));
            }
        }

        private ObservableRangeCollection<POISlim> _experiences;
        public ObservableRangeCollection<POISlim> Experiences
        {
            get => _experiences;
            set
            {
                SetAndRaise(ref _experiences , value);
                //RaisePropertyChanged(nameof(IsEmptyList));
            }
        }

        private ObservableRangeCollection<POISlim> _pois;
        public ObservableRangeCollection<POISlim> POIS
        {
            get => _pois;
            set
            {
                SetAndRaise(ref _pois , value);
            }
        }


        private IGreekCitiesService _greekCitiesService { get; set; }

        private IUserLocationService _userLocationService;

        #endregion

        //private IWeatherService WeatherService { get; set; }
        private CancellationToken _ct = new CancellationToken();
        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        public int MaxPageExperience { get; set; } = 1;

        public int MaxPageUseful { get; set; } = 1;

        public int MaxPageHighlights { get; set; } = 1;
        #endregion

        public HomeViewViewModel()
        {
            PropertiesInit();
        }

        public async Task InitializationTask()
        {
            if (IsLoaded)
            {
                POIS = await _greekCitiesService.GetGreekCities();
                return;
            }

            if (Settings.Position is null)
            {
                await _userLocationService.GetUserLocationAsync(_ct).ConfigureAwait(true);
            }

            Categories = new List<Category>(Models.Categories.CategoriesList);

            POIS = await _greekCitiesService.GetGreekCities();

            await Task.Delay(2000);

            IsLoaded = true;
        }

        //public void ExecuteLoadMoreExperiencesCommand()
        //{

        //    if (ExperiencesPage <= MaxPageExperience)
        //    {
        //        CanLoadMoreExperiences = true;
        //    }
        //    else
        //    {
        //        CanLoadMoreExperiences = false;
        //    }

        //    if (CanLoadMoreExperiences)
        //    {
        //        var data = _contentService.GetPagedListItem(1 , page: ExperiencesPage , category: experiencesArray);

        //        MaxPageExperience = data.TotalPages;

        //        foreach (var item in data.Data)
        //        {
        //            Experiences.Add(item);
        //        }

        //        ExperiencesPage++;
        //    }

        //    MainThread.BeginInvokeOnMainThread(() => IsRefreshingExperiences = false);
        //}

        //public void ExecuteLoadMoreHighlightsCommand()
        //{

        //    if (HighlightsPage <= MaxPageHighlights)
        //    {
        //        CanLoadMoreHighlights = true;
        //    }
        //    else
        //    {
        //        CanLoadMoreHighlights = false;
        //    }

        //    if (CanLoadMoreHighlights)
        //    {
        //        var data = _contentService.GetPagedListItem(1 , page: HighlightsPage , category: highlightsArray);

        //        MaxPageHighlights = data.TotalPages;

        //        foreach (var item in data.Data)
        //        {
        //            Highlights.Add(item);
        //        }

        //        HighlightsPage++;
        //    }

        //    MainThread.BeginInvokeOnMainThread(() => IsRefreshingHighlights = false);
        //}

        //public void ExecuteLoadMoreUsefulCommand()
        //{

        //    if (UsefulPage <= MaxPageUseful)
        //    {
        //        CanLoadMoreUseful = true;
        //    }
        //    else
        //    {
        //        CanLoadMoreUseful = false;
        //    }

        //    if (CanLoadMoreUseful)
        //    {
        //        var data = _contentService.GetPagedListItem(1 , page: UsefulPage , category: usefulArray);

        //        MaxPageUseful = data.TotalPages;

        //        foreach (var item in data.Data)
        //        {
        //            Useful.Add(item);
        //        }

        //        UsefulPage++;
        //    }

        //    MainThread.BeginInvokeOnMainThread(() => IsRefreshingUseful = false);
        //}

        private async Task NavigateToDetails(POISlim poi)
        {
            await poi.NavigateToDetailsAsync();
        }

        public override void Load()
        {
            LoaderNotifier.Load(_ => InitializationTask());
        }

        private void PropertiesInit()
        {
            OpenDrawerCommand = new AsyncCommand(OpenDrawer);
            _userLocationService = new UserLocationService();

            _greekCitiesService = new GreekCitiesService();
            NavToDetailsCommand = new AsyncCommand<POISlim>(NavigateToDetails);
            //LoadMoreExperiencesCommand = new Xamarin.Forms.Command(ExecuteLoadMoreExperiencesCommand);

            //LoadMoreUsefulCommand = new Xamarin.Forms.Command(ExecuteLoadMoreUsefulCommand);

            //LoadMoreHighlightsCommand = new Xamarin.Forms.Command(ExecuteLoadMoreHighlightsCommand);

            POIS = new ObservableRangeCollection<POISlim>();

            //Experiences = new ObservableRangeCollection<POISlim>();
            //Useful = new ObservableRangeCollection<POISlim>();
            //Highlights = new ObservableRangeCollection<POISlim>();
            SelectedCategories = new List<int>();
        }

        private Task OpenDrawer()
        {
            IsDrawerOpen = true;
            return Task.FromResult(IsDrawerOpen);
        }

    }
}
