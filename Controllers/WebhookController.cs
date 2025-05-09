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
        private static readonly string accessToken = "EAASvRXXnsUQBO2ivjtSAdI9SZAWMO6h4hp9NCOuwf6E005CtbRHoTdyx1AssAEedTYlnNgz8d8eOL5ZCLwqQ4ZCz1JQuR54QTrS7WEXVatpnT9ksj6AdfqPptRkkDsAsp67SGJxuRJHH1ErGyL65wTDRXC9N1CNJwYxvRNmPhuZCNnIpZA42x4S6R5DxGwaO5v3sZD";
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
                    string respuesta = payloadId == "btn_saludo" ? "Queja"
                                    : payloadId == "btn_foto" ? "Sugerencia"
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