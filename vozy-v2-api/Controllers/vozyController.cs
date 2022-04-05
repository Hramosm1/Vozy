using Microsoft.AspNetCore.Mvc;
using vozy_v2_api.core;
using vozy_v2_api.models;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vozy_v2_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vozyController : ControllerBase
    {   
        private string connection = @"Data Source=VMI662633\SQLEXPRESS;Initial Catalog=Vozy;User ID=ddonis;Password=0TkZDbcSPpn8";
        //private string connection = "Server=192.168.0.100;Database=Vozy;User ID=ddonis;Password=0TkZDbcSPpn8";
        public vozyController()
        {

        }
        [Route("test")]
        [HttpPost]
        public IActionResult testPost([FromBody] body json)
        {
            string jsonstring = json.content.ToString();

            dynamic objeto = JObject.Parse(jsonstring);
            var str = objeto.fecha.ToString();
            return Ok(str);
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
                    return Ok(new { token = token });
                }
                else
                {
                    logs.logError("El password es incorrecto");
                    return BadRequest(new { message = "El password es incorrecto" });
                }
            }
            else
            {
                logs.logError("El usuario es incorrecto");
                return BadRequest(new { message = "El usuario es incorrecto" });
            }
        }

        [Route("campaign/content")]
        [HttpPost]
        public async Task<IActionResult> PostSqlAsync([FromBody] body value, [FromHeader] headers headers)
        {
            string contentStr = value.content.ToString();
            dynamic contentObj = JObject.Parse(contentStr);
            object postObj = value.content;
            jsonwebtoken jwt = new();
            string token = headers.authorization;
            if (token != null)
            {
                if (jwt.validarToken(token))
                {

                    if (value != null)
                    {
                        using (SqlConnection db = new(connection))
                        {
                            string validacion = contentObj.agent_name.ToString().ToLower();
                            string query = $"INSERT INTO VozyEndpoint (jsonData, campaignId,contactId,sessionId) output INSERTED.ID VALUES(@jsondata, @campaign, @contact, @session)";
                            object parametros = new
                            {
                                jsondata = contentStr,
                                contact = contentObj.contact_id.ToString(),
                                campaign = contentObj.campaign_id.ToString(),
                                session = contentObj.session_id.ToString()
                            };
                            try
                            {
                                var newid = db.ExecuteScalar(query, parametros);
                                if (validacion == "lili_recagua_collections")
                                {
                                    HttpClient client = new HttpClient();
                                    //*************************************DESCOMENTAR PARA PRUEBAS*******************************
                                    //HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:44364/api/VozyAutomatizations", postObj);
                                    
                                    //*************************************COMENTAR PARA PRUEBAS**********************************
                                    HttpResponseMessage response = await client.PostAsJsonAsync("http://164.68.125.229:8061/api/VozyAutomatizations", postObj);
                                    if ((int)response.StatusCode == 200)
                                    {
                                        string newquery = $"UPDATE VozyEndpoint SET sic = 1 WHERE id = @newid";
                                        db.Execute(newquery, new { newid = newid });
                                    }
                                    return Ok(new { message = "El registro ha sido guardado exitosamente" });
                                }
                                else
                                {
                                    return Ok(new { message = "Se ingreso una llamada con otro agente" });
                                }
                            }
                            catch (Exception err)
                            {
                                logs.logError("No se pudo guardar - " + err.Message);
                                return BadRequest(new { message = "No se pudo guardar" });
                            }

                        }

                    }
                    else
                    {
                        logs.logError("falta body en la solicitud");
                        return BadRequest(new { message = "falta body" });
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
