﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SerresApp.UIControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionViewCard : ContentView
    {
        #region Bindable Properties
        public static readonly BindableProperty POINameProperty = BindableProperty.Create(nameof(POINameProperty) , typeof(string) , typeof(CollectionViewCard) , string.Empty , propertyChanged: POINamePropertyChanged);
        public static readonly BindableProperty POIRegionProperty = BindableProperty.Create(nameof(POIRegionProperty) , typeof(string) , typeof(CollectionViewCard) , string.Empty , propertyChanged: POIRegionPropertyChanged);
        public static readonly BindableProperty POIDistanceProperty = BindableProperty.Create(nameof(POIDistanceProperty) , typeof(string) , typeof(CollectionViewCard) , string.Empty , propertyChanged: POIDistancePropertyChanged);
        public static readonly BindableProperty POIImageProperty = BindableProperty.Create(nameof(POIImageProperty) , typeof(string) , typeof(CollectionViewCard) , null , propertyChanged: POIImagePropertyChanged);
        public static readonly BindableProperty POITapCommandProperty = BindableProperty.Create(nameof(POITapCommandProperty) , typeof(Command) , typeof(CollectionViewCard) , null , propertyChanged: POITapCommandPropertyChanged);
        public static readonly BindableProperty POIShowDistanceProperty = BindableProperty.Create(nameof(POIShowDistanceProperty) , typeof(bool) , typeof(CollectionViewCard) , true , propertyChanged: POIShowDistancePropertyPropertyChanged);
        #endregion

        #region Get Set Properties

        public string POIName
        {
            get => (string)GetValue(POINameProperty);
            set => SetValue(POINameProperty , value);
        }

        public bool POIShowDistance
        {
            get => (bool)GetValue(POIShowDistanceProperty);
            set => SetValue(POIShowDistanceProperty , value);
        }

        public string POIImage
        {
            get => (string)GetValue(POIImageProperty);
            set => SetValue(POIImageProperty , value);
        }


        public string POIRegion
        {
            get => (string)GetValue(POIRegionProperty);
            set => SetValue(POIRegionProperty , value);
        }

        public string POIDistance
        {
            get => (string)GetValue(POIDistanceProperty);
            set => SetValue(POIDistanceProperty , value);
        }

        public Command POITapCommand
        {
            get => (Command)GetValue(POITapCommandProperty);
            set => SetValue(POITapCommandProperty , value);
        }

        #endregion

        #region PropertyChanged

        private static void POINamePropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((string)newValue != string.Empty)
            {
                card.POINameLabel.Text = (string)newValue;
            }
            else
            {
                card.POINameLabel.Text = (string)oldValue;
            }
        }

        private static void POIShowDistancePropertyPropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((bool)newValue != (bool)oldValue)
            {
                card.POIDistanceIndicatorLabel.IsVisible = (bool)newValue;
                card.POIDistanceFromUserLabel.IsVisible = (bool)newValue;
            }
            else
            {
                card.POIDistanceIndicatorLabel.IsVisible = (bool)oldValue;
                card.POIDistanceFromUserLabel.IsVisible = (bool)oldValue;
            }
        }

        private static void POIImagePropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((string)newValue != null)
            {
                card.POIImageUri.Source = (string)newValue;
            }
        }


        private static void POIRegionPropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((string)newValue != string.Empty)
            {
                card.POIRegionLabel.Text = (string)newValue;
            }
            else
            {
                card.POIRegionLabel.Text = (string)oldValue;
            }
        }

        private static void POIDistancePropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((string)newValue != string.Empty)
            {
                card.POIDistanceFromUserLabel.Text = (string)newValue;
            }
            else { } /*{ card.PlaceCardTitle.Text = ((double)oldValue.ToString()); }*/
        }

        private static void POITapCommandPropertyChanged(BindableObject bindable , object oldValue , object newValue)
        {
            CollectionViewCard card = (CollectionViewCard)bindable;
            if ((Command)newValue != null)
            {
                //card.POITapAction.Command .Command = ( (double)newValue.ToString() );
            }
            else { } /*{ card.PlaceCardTitle.Text = ((double)oldValue.ToString()); }*/
        }


        #endregion

        #region UI Interaction


        #endregion


        public CollectionViewCard()
        {
            InitializeComponent();
        }
    }
}