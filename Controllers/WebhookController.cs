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
