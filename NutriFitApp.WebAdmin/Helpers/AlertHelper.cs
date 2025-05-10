using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NutriFitApp.WebAdmin.Helpers
{
    public static class AlertHelper
    {
        public static void Success(ITempDataDictionary tempData, string message)
        {
            tempData["Success"] = message;
        }

        public static void Error(ITempDataDictionary tempData, string message)
        {
            tempData["Error"] = message;
        }

        public static void Warning(ITempDataDictionary tempData, string message)
        {
            tempData["Warning"] = message;
        }
    }
}
