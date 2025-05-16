using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using PoliciaApi.Models;


namespace PoliciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[Action]")]
        public IActionResult GetHolaMundo()
        {
            return Ok("Hola Mundo");
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetABaseDatos()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConexionLaura");

                using var conn = new OracleConnection(connectionString);
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
                return new BadRequestObjectResult(new { response = ex.Message });
            }
        }

        [HttpGet("[Action]/{numero1}/{numero2}")]
        public IActionResult Calculadora(int numero1, int numero2)
        {
            return Ok($"la suma es {numero1 + numero2}");
        }

        [HttpGet("[Action]/{nombre}")]
        public IActionResult Mensaje(string nombre)
        {
            return Ok($"Hola {nombre} parametro eviado desde url");
        }

        private static List<Producto> productos = new List<Producto>();

        [HttpPost("[Action]")]
        public IActionResult CrearProducto(Producto productoNuevo)
        {
            productoNuevo.Id = productos.Count + 1;
            productos.Add(productoNuevo);
            return Ok($"Producto {productoNuevo.Nombre} creado con exito");
        }

        [HttpPost("[Action]")]
        public IActionResult Crear([FromBody] List<Producto> nuevoProducto)
        {
            foreach(var item in nuevoProducto)
            {
                item.Id = productos.Count + 1;
                productos.Add(item);
            }
            
            return Ok($"Su lista de de productos ha sio cargada");
        }

        [HttpGet("[Action]")]
        public IActionResult TraerLista()
        {
            return Ok(productos);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Producto productoActualizado)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) 
                return NotFound();

            producto.Nombre = productoActualizado.Nombre;
            producto.Precio = productoActualizado.Precio;

            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) 
                return NotFound();

            productos.Remove(producto);
            return NoContent();
        }
    }
}









