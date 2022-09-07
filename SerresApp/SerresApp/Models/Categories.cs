
using SerresApp.Resources;

using System.Collections.Generic;

namespace SerresApp.Models
{
    public class Categories
    {
        private readonly List<Category> _categoriesList = new List<Category>
        {
            new Category {Id=0 ,Color = "#315D26", Text =AppResources.NatureAndWildlife, Image = "culture.png" },
            new Category {Id=1 ,Color = "#9388D0", Text =AppResources.TangibleCulturalHeritage, Image = "culture.png" },
            new Category {Id=2 ,Color = "#005680", Text =AppResources.IntangibleCulturalHeritage, Image = "culture.png" }
        };

        public List<Category> CategoriesList => new List<Category>(_categoriesList);
    }
}
