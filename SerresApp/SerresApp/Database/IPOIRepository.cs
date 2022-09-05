﻿using SerresApp.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SerresApp.Database
{
    public interface IPOIRepository
    {
        Task<List<POIDatabaseItem>> GetFavoritesAsync();

        Task<POIDatabaseItem> GetItemAsync(int id);

        Task<int> SaveItemAsync(POIDatabaseItem item);

        Task DeleteItemAsync(int id);

        Task<int> GetItemIdAsync(POIDatabaseItem item);
    }
}
