﻿using Microsoft.AspNetCore.Mvc;
using vozy_v2_api.core;
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
        //private string connection = "Data Source=192.168.8.6;Initial Catalog=WS_InteligenciaDB_Fase2;User ID=ddonis;Password=0TkZDbcSPpn8";
        private string connection = "Server=192.168.8.8;Database=WS_InteligenciaDB_Fase2;Trusted_Connection=True;";
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
                    logs.logError("El password es incorrecto");
                     return BadRequest(new {message = "El password es incorrecto" });
                }
            }
            else
            {
                logs.logError("El usuario es incorrecto");
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
                        if (value!= null)
                        {
                        using (SqlConnection db = new(connection))
                        {
                            string ob = Newtonsoft.Json.JsonConvert.SerializeObject(value.content);
                            string query = $"INSERT INTO vozy (jsonData, contacId, campaignId, sessionId) values (@jsondata, @contact, @campaign, @session)";
                            db.Execute(query, new { jsondata = ob, contact = value.content.contact_id, campaign=value.content.campaign_id, session=value.content.session_id });
                            return Ok(new { message = "El registro ha sido guardado exitosamente"});
                        }
                        }
                        else
                        {
                            logs.logError("falta body en la solicitud");
                            return BadRequest(new {message = "falta body"});
                        }
                    }
                    catch (Exception err)
                    {
                        logs.logError("No se pudo guardar - "+err.Message);
                        return BadRequest(new { message = "No se pudo guardar" });
                    }
                   
                }
                else
                {
                    logs.logError("Token no valido");
                    return StatusCode(401, new { message = "Token no valido" });
                }
            }
            else
            {
                logs.logError("Sin Token");
                return StatusCode(401, new { message = "Sin Token" });
            }
        }
    }

}
