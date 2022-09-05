
using SerresApp.Resources;

using System.Collections.Generic;

namespace SerresApp.Models
{
    public static class Categories
    {
        private static readonly List<Category> _categoriesList = new List<Category>
        {
            new Category {Id=0 ,Color = "#f8c823", Text =AppResources.NatureAndWildlife, Image = "culture.png" },
            new Category {Id=1 ,Color = "#b83737", Text =AppResources.TangibleCulturalHeritage, Image = "culture.png" },
            new Category {Id=2 ,Color = "#138bcf", Text =AppResources.InangibleCulturalHeritage, Image = "culture.png" }
        };

        public static List<Category> CategoriesList => new List<Category>(_categoriesList);
    }
}
