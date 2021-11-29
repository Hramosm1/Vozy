using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using vozy_v2_api.models;
using System.Data.SqlClient;
using Dapper;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vozy_v2_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vozyController : ControllerBase
    {
        private string connection = "Data Source=192.168.8.6;Initial Catalog=WS_InteligenciaDB_Fase2;User ID=ddonis;Password=0TkZDbcSPpn8";
        public vozyController()
        {
           
        }

        // GET api/vozy/token
        [Route("token")]
        [HttpPost]
        public IActionResult Post([FromBody] User credenciales)
        {
            jsonwebtoken jwt = new();
            if (credenciales.user == "vozy")
            {
                if (credenciales.password == "UOgVafoyJP5N11l")
                {
                    string token = jwt.crearToken(credenciales.user, credenciales.password);
                    return Ok(new {token = token});
                }
                else
                {
                     return BadRequest(new {message = "El password es incorrecto" });
                }
            }
            else
            {
                return BadRequest(new {message = "El usuario es incorrecto" });
            }
        }

        [Route("campaign/content")]
        [HttpPost]
        public async Task<IActionResult> PostSqlAsync([FromBody] body value, [FromHeader] headers headers)
        {
            jsonwebtoken jwt = new();
            string token = headers.authorization;
            if (token != null)
            {

                if (jwt.validarToken(token))
                {
                    try
                    {
                        using (SqlConnection db = new(connection))
                        {
                            string ob = Newtonsoft.Json.JsonConvert.SerializeObject(value.content);
                            string query = $"INSERT INTO vozy (jsonData) values ('{ob}')";
                            db.Execute(query, new { json = value.ToString() });
                            return Ok(new { message = "El registro ha sido guardado exitosamente"});
                        }
                    }
                    catch (Exception)
                    {
                        return BadRequest(new { message = "No se pudo guardar" });
                    }
                   
                }
                else
                {
                    return StatusCode(401, new { message = "Token no valido" });
                }
            }
            else
            {
                return StatusCode(401, new { message = "Sin Token" });
            }
        }
    }

}
