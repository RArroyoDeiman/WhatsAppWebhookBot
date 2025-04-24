using Microsoft.AspNetCore.Mvc;
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
                response.Message("¡Hola! ¿En qué puedo ayudarte?");
            }
            else if (Body?.ToLower().Contains("foto") == true)
            {
                var message = new Message();
                message.Body("Aquí tienes una imagen:");
                message.Media("https://via.placeholder.com/300");
                response.Append(message);
            }
            else
            {
                response.Message("Lo siento, no entendí eso. Escribe 'hola' o 'foto'.");
            }

            return Content(response.ToString(), "application/xml");
        }
    }
}