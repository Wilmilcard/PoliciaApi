using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using PoliciaApi.Models;

namespace PoliciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OracleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public OracleController(IConfiguration configuration, AppDbContext context)
        {
            this._configuration = configuration;
            this._context = context;
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

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetCorreos()
        {
            var correos = await _context.Clientes
                .Select(c => c.Correo)
                .ToListAsync();

            return Ok(new { correos });
        }

        // GET: Todos los clientes
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        // GET: Un cliente por ID
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        // POST: Crear nuevo cliente
        [HttpPost("[Action]")]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cliente.ClienteId }, cliente);
        }

        // PUT: Actualizar cliente
        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.ClienteId)
                return BadRequest();

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: Eliminar cliente
        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
