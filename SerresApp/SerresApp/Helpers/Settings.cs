﻿
using System.Collections.Generic;

using Xamarin.Essentials;

namespace SerresApp.Helpers
{
    public static class Settings
    {

        // 0 = Αγγλικά, 1 = Ελληνικά, 2 = Βουλγάρικα
        private const int language = 0;

        // 0 = Προεπιλογή, 1 = Φωτεινή, 2 = Σκοτεινή
        private const int theme = 0;

        public static Dictionary<int , string> CategoriesId = new Dictionary<int , string>
        {
            [0] = "en" , //Αγγλικά
            [1] = "el" , //Ελληνικά
            [2] = "bg"  //Γερμανικά μη διαθέσιμα ==> TwoLetterLocaleCode[2] = "en"
        };

        //Settings.TwoLetterLocaleCode[Settings.Language] ==> δίνει τον κωδικό ISO για την UI γλώσσα
        public static Dictionary<int , string> TwoLetterLocaleCode = new Dictionary<int , string>
        {
            [0] = "en" , //Αγγλικά
            [1] = "el" , //Ελληνικά
            [2] = "bg"  //Γερμανικά μη διαθέσιμα ==> TwoLetterLocaleCode[2] = "en"
        };

        public static int Language
        {
            get => Preferences.Get(nameof(Language) , language);
            set => Preferences.Set(nameof(Language) , value);
        }

        public static Location Position { get; set; }
        public static int Theme
        {
            get => Preferences.Get(nameof(Theme) , theme);
            set => Preferences.Set(nameof(Theme) , value);
        }
    }
}
