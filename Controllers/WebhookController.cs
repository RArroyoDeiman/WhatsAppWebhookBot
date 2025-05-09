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
        private static readonly string accessToken = "EAASvRXXnsUQBO1jKiSKxWOkJM9W2x1ZC4gqgqaDu96L5Jkh78mJ9IEC9ZBZC2SfxZBRel6AFzjWfzonMK1TycYn6smBNkYd6ZBBZCBzHmsTXZCB2fOJwZCHYwZBV0ZAjVZByvtQMqkpw4dUzDYwCO4gd3mJm9HRRIZCGn3sTdYr2xMi4s1lINlv2bUZBAlDkp9foAVpABcm0ZD";
        private static readonly string phoneNumberId = "591999804005728";

        [HttpGet]
        public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
        {
            if (hub_mode == "subscribe" && hub_verify_token == "midemo123")
                return Ok(hub_challenge);
            return Unauthorized();
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
                    string respuesta = payloadId == "btn_saludo" ? "¡Hola! ¿En qué puedo ayudarte?"
                                    : payloadId == "btn_foto" ? "Te enviaré una imagen pronto."
                                    : "Opción no reconocida.";
                    await EnviarMensaje(from, respuesta);
                }
            }
            catch { }

            return Ok();
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
