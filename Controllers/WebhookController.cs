/*using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace WhatsAppWebhookBot.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public IActionResult Receive([FromForm] string Body, [FromForm] string From)
        {
            Console.WriteLine($"Mensaje recibido de {From}: {Body}");
            var response = new MessagingResponse();

            if (Body?.ToLower().Contains("hola") == true)
            {
                response.Message("¡Hola! Gracias por ponerte en contacto con nosotros. Este es un canal exclusivo para la captación de quejas pero si tienes alguna duda o sugerencia que te gustaría compartir con nosotros, estaré encantado de ayudarte a resolverlo.");
            }
            

            else if (Body?.ToLower().Contains("duda") == true)
            {
                response.Message("Ingresa a esta liga o marca a este numero telefonico:https://deiman.com.mx/contacto México +52 (55) 5561-4200");
            }
            else if (Body?.ToLower().Contains("sugerencia") == true)
            {
                response.Message("Ingresa a esta liga o marca a este numero telefonico:https://deiman.com.mx/contacto México +52 (55) 5561-4200");
            }
            
            else if (Body?.ToLower().Contains("queja") == true)
            {
                response.Message("Por favor ingresa los datos de tu queja: Tipo de queja: -Fuga de producto por tapa (esencias y concentrados) -Derrame de producto (glucosa, cobertura y semipreparados) -Desviación en contenido neto -Producto sin etiqueta -Etiquetado incorrecto -Perfil del sabor -Cambio en apariencia");
            }//Etiquetado incorrecto
            
            else if (Body?.ToLower().Contains("etiquetado incorrecto") == true)
            {
                response.Message("Por favor, ingresa los siguientes datos: Nombre del producto Número de lote Datos de la persona (correo electrónico, número telefónico) En donde adquirió el producto (tienda en línea o distribuidor)");
            }//Etiquetado incorrecto
            else if (Body?.ToLower().Contains("foto") == true)
            {
                var message = new Message();
                message.Body("Aquí tienes una imagen:");
                //message.Media("https://via.placeholder.com/300");
                response.Append(message);
            }
            else
            {
                response.Message("Lo siento, no entendí eso. Escribe 'hola'.");
            }

            return Content(response.ToString(), "application/xml");
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Twilio;

namespace WhatsAppWebhookBot.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        private const string accountSid = "ACe63c715793aebd8c20243ae4a2e2ae6b";
        private const string authToken = "75352c20fd8086857ea3d3abfe6cc777";

        [HttpPost]
        public async Task<IActionResult> Receive([FromForm] string Body, [FromForm] string From)
        {
            Console.WriteLine($"Mensaje de {From}: {Body}");

            if (Body.ToLower().Contains("menu"))
            {
                await SendInteractiveButtons(From);
                return Ok();
            }

            return Content("Mensaje recibido");
        }

        private async Task SendInteractiveButtons(string to)
        {
            var payload = new
            {
                messaging_product = "whatsapp",
                to = to,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new { text = "¿Qué deseas hacer?" },
                    action = new
                    {
                        buttons = new[]
            {
new {
type = "reply",
reply = new {
id = "btn_saludar",
title = "Saludar"
}
},
new {
type = "reply",
reply = new {
id = "btn_foto",
title = "Enviar Foto"
}
}
}
                    }
                }
            };

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twilio.com/2010-04-01/Accounts/" + accountSid + "/Messages")
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };

            var byteArray = Encoding.ASCII.GetBytes($"{accountSid}:{authToken}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Twilio response: {result}");
        }
    }
}
