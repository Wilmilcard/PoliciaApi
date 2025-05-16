using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PoliciaApi.Utils;

namespace PoliciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MailController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet("[Action]/{ClienteId}")]
        public async Task<IActionResult> GenerarPago(string ClienteId)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConexionLaura");
                using var conn = new OracleConnection(connectionString);
                conn.Open();

                var query = @$"
                    SELECT 
                        u.ID AS USUARIO_ID,
                        u.NOMBRE || ' ' || u.APELLIDO AS NOMBRE_COMPLETO,
                        u.CORREO,
                        p.NUMERO_PAGO,
                        p.METODO_PAGO,
                        TO_CHAR(p.FECHA_PAGO, 'DD-MM-YYYY HH24:MI') AS FECHA_PAGO,
                        p.VALOR_PAGO,
                        s.CINE,
                        s.id AS SALA,
                        pe.nombre AS PELICULA,
                        TO_CHAR(s.HORA_FUNCION, 'DD-MM-YYYY HH24:MI') AS HORA_FUNCION
                    FROM 
                        SYSTEM.PAGO p 
                        INNER JOIN SYSTEM.USUARIO u ON p.USUARIO_ID = u.ID
                        INNER JOIN SYSTEM.SALA s ON p.SALA_ID = s.ID
                        INNER JOIN SYSTEM.Peliculas pe ON pe.id = s.pelicula_id
                    WHERE u.ID = {ClienteId}
                    ORDER BY 
                        p.FECHA_PAGO DESC";

                using var transaction = conn.BeginTransaction(); // Inicia la transacción
                using var cmd = new OracleCommand(query, conn)
                {
                    Transaction = transaction // Asocia la transacción al comando
                };

                using var reader = cmd.ExecuteReader();

                var email = new EmailTools();
                while (reader.Read())
                {
                    await email.SendEmailAsync(reader, reader["CORREO"].ToString(), "Confirmación Pago");
                }

                transaction.Commit(); // Finaliza correctamente

                var response = new
                {
                    status = true,
                    code = 200,
                    response = $"Correo enviado con exito al usuario { reader["NOMBRE_COMPLETO"].ToString() }"
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { response = ex.Message });
            }
        }
    }
}
