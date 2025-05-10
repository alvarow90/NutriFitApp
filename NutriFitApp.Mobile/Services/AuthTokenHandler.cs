// Archivo: Handlers/AuthTokenHandler.cs (o Services/AuthTokenHandler.cs)
// Namespace: NutriFitApp.Mobile.Handlers (o NutriFitApp.Mobile.Services si lo pones allí)

// Este DelegatingHandler se encarga de adjuntar automáticamente el token JWT
// a las cabeceras de las solicitudes HTTP salientes.
using System.Net.Http;
using System.Net.Http.Headers; // Para AuthenticationHeaderValue
using System.Threading;
using System.Threading.Tasks;
using NutriFitApp.Mobile.Services; // Para acceder a AuthService.GetAuthTokenAsync()
using System.Diagnostics;          // Para Debug.WriteLine

namespace NutriFitApp.Mobile.Handlers // O NutriFitApp.Mobile.Services, ¡asegúrate que coincida con donde lo guardas!
{
    public class AuthTokenHandler : DelegatingHandler
    {
        // Constructor. El InnerHandler se establecerá usualmente durante la configuración
        // del HttpClient en MauiProgram.cs.
        public AuthTokenHandler()
        {
            Debug.WriteLine("[AuthTokenHandler] Constructor ejecutado.");
        }

        // Este método se llama para cada solicitud HTTP que pasa a través de este manejador.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"[AuthTokenHandler] SendAsync llamado para: {request.RequestUri}");

            // Intentar obtener el token de autenticación guardado.
            // Usamos el método estático GetAuthTokenAsync de AuthService para simplicidad.
            var token = await AuthService.GetAuthTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                // Si se encontró un token, añadirlo a la cabecera 'Authorization'
                // como un "Bearer token".
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Debug.WriteLine($"[AuthTokenHandler] Token adjuntado a la solicitud para: {request.RequestUri} (Token: ...{token.Substring(Math.Max(0, token.Length - 6))})");
            }
            else
            {
                Debug.WriteLine($"[AuthTokenHandler] No se encontró token para adjuntar a la solicitud para: {request.RequestUri}");
            }

            // Continuar con el envío de la solicitud a través del siguiente manejador en la cadena
            // (que será el InnerHandler, usualmente HttpClientHandler que hace la llamada de red).
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
