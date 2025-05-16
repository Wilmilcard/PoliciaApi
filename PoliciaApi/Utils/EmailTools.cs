using Oracle.ManagedDataAccess.Client;
using System.Net;
using System.Net.Mail;

namespace PoliciaApi.Utils
{
    public class EmailTools
    {
        public EmailTools(){ }

        public async Task SendEmailAsync(OracleDataReader reader, string emailTo, string subject)
        {
            var email_sender = "TU CORREO DE ENVIO"; //<----- Ajustar Aqui

            MailAddress addresFrom = new MailAddress(email_sender, "Prueba PoliciaApi");
            MailAddress addresTo = new MailAddress(emailTo);
            var message = new MailMessage(addresFrom, addresTo);
            message.Subject = subject;
            message.IsBodyHtml = true;
            

            var client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(email_sender, "TU CONTRASEÑA DE ENVIO"); //<----- Ajustar Aqui

            var path = Globals.PathSystem(["PoliciaApi", "Assets"]);

            string emailBody = File.ReadAllText($"{path}\\confirmacion_pago.html")
                .Replace("{NOMBRE_CLIENTE}", $"{reader["NOMBRE_COMPLETO"]}")
                .Replace("{NUMERO_ORDEN}", $"{reader["NUMERO_PAGO"]}")
                .Replace("{CINE}", $"{reader["CINE"]}")
                .Replace("{SEDE}", $"Plaza Mayor")
                .Replace("{SALA}", $"{reader["SALA"]}")
                .Replace("{PELICULA}", $"{reader["PELICULA"]}")
                .Replace("{FECHA_FUNCION}", $"{Convert.ToDateTime(reader["HORA_FUNCION"]).ToString("dd/MM/yyyy HH:mm:ss")}")
                .Replace("{MONTO}", $"{reader["VALOR_PAGO"]}")
                .Replace("{MONEDA}", "COP")
                .Replace("{FECHA_PAGO}", Globals.SystemDate().ToString("dd/MM/yyyy HH:mm:ss"))
                .Replace("{METODO_PAGO}", $"{reader["METODO_PAGO"]}")
                .Replace("{URL_SOPORTE}", "https://educacion.policia.edu.co/dinae/")
                .Replace("{AÑO}", DateTime.Now.Year.ToString());

            message.Body = emailBody;

            try
            {
                client.Send(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error enviando correo: {ex.Message}");
            }
        }
    }
}
