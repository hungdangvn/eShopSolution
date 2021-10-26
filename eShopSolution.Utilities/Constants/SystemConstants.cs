using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "eShopSolutionDb";

        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSetting
        {
            public const int NumberOfFeaturedProducts = 8;
            public const int NumberOfLastestProducts = 6;
        }
    }
}