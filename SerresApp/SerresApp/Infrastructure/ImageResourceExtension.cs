
using System;
using System.Collections.Generic;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SerresApp.Infrastructure
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        private static readonly Dictionary<string , ImageSource> Cache = new Dictionary<string , ImageSource>();

        public string Source { get; set; }

        public static ImageSource GetImageSource(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (Cache.TryGetValue(value , out var imageSource))
            {
                return imageSource;
            }

            // Do your translation lookup here, using whatever method you require
            ImageSource newImageSource = ImageSource.FromResource(value , typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            Cache.Add(value , newImageSource);
            return newImageSource;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            return GetImageSource(Source);
        }
    }
}
