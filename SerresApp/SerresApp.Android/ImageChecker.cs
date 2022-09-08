using SerresApp.Droid;
using SerresApp.Interfaces;

using System.IO;

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
            var resourceId = resources.GetIdentifier(name.ToLower() , "drawable" , context.PackageName);
            return resourceId != 0;
        }
    }
}