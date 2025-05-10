// Archivo: Converters/IsNotNullOrEmptyStringConverter.cs
// Ubicación: Proyecto NutriFitApp.Mobile (o donde lo tengas)
using System.Globalization;
using Microsoft.Maui.Controls; // Para IValueConverter

namespace NutriFitApp.Mobile.Converters
{
    public class IsNotNullOrEmptyStringConverter : IValueConverter
    {
        // La firma del método ahora usa 'object?' para los parámetros para coincidir con IValueConverter
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            // Devuelve true si el string no es nulo ni vacío, false en caso contrario.
            return !string.IsNullOrEmpty(value as string);
        }

        // La firma del método ahora usa 'object?' para los parámetros
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            // No se necesita para este conversor en la mayoría de los casos.
            throw new NotImplementedException();
        }
    }
}
