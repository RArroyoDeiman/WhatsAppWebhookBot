using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WhatsAppMetaWebhook.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        private static readonly string accessToken = "EAAI3Qe5HIJYBO5GNUGYyM8KZC1GD65qZCZBP8JXFzWZAE8EApcgwN3c19Y6dVkGBiDTmv9fsmn1SRJZBuWkolZCaZBhK0BDD3jcWDuBbmxZBhkrvmfwxFvlntZCATPaFLELHyBfzWaARhp0NBj44y9aoZA1GZBB2zU23iJMhNXJCkV5sq68zLCWAjqeJXsHUdBTvPJVFSUZD";
        private static readonly string phoneNumberId = "636315776233974";

        [HttpGet]
public IActionResult Get(
    [FromQuery(Name = "hub.mode")] string mode,
    [FromQuery(Name = "hub.challenge")] string challenge,
    [FromQuery(Name = "hub.verify_token")] string verifyToken)
{
    const string VERIFY_TOKEN = "midemo123"; // usa el mismo que pusiste en Meta

    if (mode == "subscribe" && verifyToken == VERIFY_TOKEN)
    {
        Console.WriteLine("Webhook verificado correctamente.");
        return Ok(challenge); // responde con el challenge
    }

    return BadRequest("Error de verificación");
}

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] dynamic body)
        {
            try
            {
                var message = body.entry[0].changes[0].value.messages[0];
                string from = message.from;
                string type = message.type;

                if (type == "button")
                {
                    string payloadId = message.button.payload;
                    string respuesta = payloadId == "btn_saludo" ? "Queja"
                                    : payloadId == "btn_foto" ? "Sugerencia"
                                    : "Opción no reconocida.";
                    await EnviarMensaje(from, respuesta);
                }
            }
            catch { }

            return Ok();
        }
        [HttpGet("probar-envio")]
public async Task<IActionResult> ProbarEnvio()
{
// Reemplaza con tus datos reales
string numeroDestino = "5215545549982"; // Tu número verificado
string tokenTemporal = "EAAI3Qe5HIJYBO5GNUGYyM8KZC1GD65qZCZBP8JXFzWZAE8EApcgwN3c19Y6dVkGBiDTmv9fsmn1SRJZBuWkolZCaZBhK0BDD3jcWDuBbmxZBhkrvmfwxFvlntZCATPaFLELHyBfzWaARhp0NBj44y9aoZA1GZBB2zU23iJMhNXJCkV5sq68zLCWAjqeJXsHUdBTvPJVFSUZD"; // Tu token de acceso temporal
string idTelefono = "636315776233974"; // El ID del número de teléfono que te dio Meta

await EnviarMensajeWhatsAppMeta(numeroDestino, tokenTemporal, idTelefono);

return Ok("Mensaje enviado (revisa tu WhatsApp)");
}

private async Task EnviarMensajeWhatsAppMeta(string numeroDestino, string tokenTemporal, string idTelefono)
{
var payload = new
{
messaging_product = "whatsapp",
to = numeroDestino,
type = "text",
text = new
{
body = "Hola desde la API de WhatsApp con C# y Meta!"
}
};

var client = new HttpClient();
client.DefaultRequestHeaders.Authorization =
new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenTemporal);

var contenido = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

var respuesta = await client.PostAsync($"https://graph.facebook.com/v19.0/{idTelefono}/messages", contenido);
var resultado = await respuesta.Content.ReadAsStringAsync();

Console.WriteLine($"Respuesta de Meta: {respuesta.StatusCode} - {resultado}");
}

        private async Task EnviarMensaje(string to, string texto)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var msg = new
            {
                messaging_product = "whatsapp",
                to = to,
                type = "text",
                text = new { body = texto }
            };

            var content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
            await client.PostAsync($"https://graph.facebook.com/v19.0/{phoneNumberId}/messages", content);
        }
    }
}
