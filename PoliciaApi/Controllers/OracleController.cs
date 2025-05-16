using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace PoliciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OracleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OracleController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet("api/conexiones/[Action]")]
        public async Task<IActionResult> ConsultarBD() 
        {
            try
            {
                var cadenaDeConexion = this._configuration.GetConnectionString("ConexionLaura");
                using var conn = new OracleConnection(cadenaDeConexion);
                conn.Open();
                using var cmd = new OracleCommand("SELECT * FROM system.cliente", conn);
                using var reader = cmd.ExecuteReader();
                var response = new List<string>();

                while (reader.Read())
                {
                    response.Add(reader["CORREO"].ToString());
                }

                return new OkObjectResult(new { correos = response });
            }
            catch (Exception ex) 
            {
                var response = new
                {
                    code = 400,
                    message = ex.Message
                };
                return new BadRequestObjectResult(response);
            }


        }

    }
}
