﻿using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;

using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

namespace SerresApp.Droid
{
    public sealed class CachingNativeBitmapDescriptorFactory : IBitmapDescriptorFactory
    {

        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            var defaultFactory = DefaultBitmapDescriptorFactory.Instance;

            if (!string.IsNullOrEmpty(descriptor.Id))
            {
                var iconId = 0;
                switch (descriptor.Id)
                {
                    case "1":
                        iconId = Resource.Drawable.top;
                        break;
                    case "2":
                        iconId = Resource.Drawable.monument;
                        break;
                    case "3":
                        iconId = Resource.Drawable.museum;
                        break;
                    case "4":
                        iconId = Resource.Drawable.beach;
                        break;
                    case "6":
                        iconId = Resource.Drawable.c0;
                        break;
                    case "7":
                        iconId = Resource.Drawable.gastronomy;
                        break;
                    case "8":
                        iconId = Resource.Drawable.@event;
                        break;
                    default:
                        iconId = Resource.Drawable.c0;
                        break;
                }

                return AndroidBitmapDescriptorFactory.FromResource(iconId);

                //var cacheEntry = _cache.GetOrAdd(descriptor.Id, _ => defaultFactory.ToNative(descriptor));
                //return AndroidBitmapDescriptor.FromResource(iconId);
            }

            return defaultFactory.ToNative(descriptor);
        }
    }
}