
using MvvmHelpers;
using MvvmHelpers.Commands;

using SerresApp.Helpers;
using SerresApp.Interfaces;
using SerresApp.Models;
using SerresApp.Services;

using Sharpnado.TaskLoaderView;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Essentials;

namespace SerresApp.ViewModels
{
    public partial class CollectionViewViewModel : BaseViewModel
    {
        #region Properties

        ICommand loadMoreCommand = null;
        public ICommand LoadMoreCommand
        {
            get { return loadMoreCommand; }
            set
            {
                if (loadMoreCommand != value)
                {
                    loadMoreCommand = value;
                    RaisePropertyChanged("LoadMoreCommand");
                }
            }
        }

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetAndRaise(ref isRefreshing , value);
            }
        }

        private int Page { get; set; } = 1;

        bool _canLoadMore = true;
        public bool CanLoadMore
        {
            get
            {
                return _canLoadMore;
            }
            set
            {
                SetAndRaise(property: ref _canLoadMore , value);
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


        private CancellationToken _ct = new CancellationToken();
        public ICommand OpenDrawerCommand { get; set; }
        public ICommand NavToDetailsCommand { get; set; }
        public ICommand ApplyFiltersChangeCommand { get; set; }

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

        private ObservableRangeCollection<POISlim> _pois;
        public ObservableRangeCollection<POISlim> POIS
        {
            get => _pois;
            set
            {
                SetAndRaise(ref _pois , value);
            }
        }


        private ObservableRangeCollection<POISlim> _filteredResults;
        public ObservableRangeCollection<POISlim> FilteredResults
        {
            get => _filteredResults;
            set
            {
                SetAndRaise(ref _filteredResults , value);
            }
        }

        int _itemsCount;
        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                SetAndRaise(ref _itemsCount , value);
                RaisePropertyChanged(nameof(IsEmptyList));
            }
        }
        private IGreekCitiesService _greekCitiesService { get; set; }

        private IUserLocationService _userLocationService;

        public bool IsEmptyList
        {
            get { return ItemsCount == 0; }
        }
        #endregion

        public TaskLoaderNotifier LoaderNotifier { get; set; } = new TaskLoaderNotifier();

        public int MaxPage { get; set; } = 0;
        #endregion

        public CollectionViewViewModel()
        {
            PropertiesInit();
        }

        public async Task InitializationTask()
        {
            if (Settings.Position is null)
            {
                await _userLocationService.GetUserLocationAsync(_ct).ConfigureAwait(false);
            }

            Categories categories = new Categories();
            Categories = new List<Category>(categories.CategoriesList);
            CanLoadMore = true;

            POIS = await _greekCitiesService.GetGreekCities();

            var intermediary = FilteredResults;
            FilteredResults.Clear();

            foreach (var item in POIS)
            {
                intermediary.Add(item);
            }

            FilteredResults = new ObservableRangeCollection<POISlim>(intermediary);
            ItemsCount = FilteredResults.Count;

            Page++;

            MainThread.BeginInvokeOnMainThread(() => IsRefreshing = false);

            await Task.Delay(2000);

            IsLoaded = true;
        }


        private Task ApplyFiltersChange()
        {
            CanLoadMore = true;

            SelectedCategories = Categories.Where(x => x.IsSelected).Select(x => x.Id).ToList();
            FilteredResults = new ObservableRangeCollection<POISlim>(POIS.Where(x => SelectedCategories.Contains(x.CategoryId)));
            ItemsCount = FilteredResults.Count;
            Page = 1;
            return Task.FromResult(Categories);
        }
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
            ApplyFiltersChangeCommand = new AsyncCommand(ApplyFiltersChange);
            FilteredResults = new ObservableRangeCollection<POISlim>();
            SelectedCategories = new List<int>();
        }

        private Task OpenDrawer()
        {
            IsDrawerOpen = true;
            return Task.FromResult(IsDrawerOpen);
        }

    }
}
