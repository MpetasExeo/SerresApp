using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SerresApp.Droid;
using SerresApp.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Xamarin.Forms;

[assembly: Dependency(typeof(ImageChecker))]
namespace SerresApp.Droid
{
    public class ImageChecker : IImageChecker
    {
        public bool DoesImageExist(string image)
        {
            var context = Android.App.Application.Context;
            var resources = context.Resources;
            var name = Path.GetFileNameWithoutExtension(image);
            int resourceId = resources.GetIdentifier(name.ToLower() , "drawable" , context.PackageName);
            return resourceId != 0;
        }
    }
}