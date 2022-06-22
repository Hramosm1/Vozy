using Microsoft.AspNetCore.Mvc;
using vozy_v2_api.core;
using vozy_v2_api.models;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Text;
using MySqlConnector;

namespace vozy_v2_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class vozyController : ControllerBase
  {
    private MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
    {
      Server = "62.171.184.240",
      UserID = "ddonis",
      Password = "d6!k7I4^MK08",
      Database = "Vozy"
    };
    public vozyController() { }

    // POST api/vozy/token
    [Route("token")]
    [HttpPost]
    public IActionResult Post([FromHeader] headers headers)
    {
      if (headers.Authorization == null)
      {
        return BadRequest(new { message = "No se encontraron las credenciales" });
      }
      else
      {
        string[] token = headers.Authorization.Split(" ");
        if (token[1] == "dm96eTpVT2dWYWZveUpQNU4xMWw=")
        {
          jsonwebtoken jwt = new();
          string[] credenciales = Encoding.UTF8.GetString(Convert.FromBase64String(token[1])).Split(":");
          string jwtToken = jwt.crearToken(credenciales[0], credenciales[1]);
          return Ok(new { token = jwtToken, type = "JWT" });
        }
        else
        {
          return Unauthorized(new { message = "El usuario o la contraseña no son correctas" });
        }
      }
    }

    [Route("campaign/content")]
    [HttpPost]
    public async Task<IActionResult> PostSqlAsync([FromBody] body value, [FromHeader] headers headers)
    {
      string contentStr = value.content.ToString();
      dynamic contentObj = JObject.Parse(contentStr);
      object postObj = value.content;
      string newId;
      jsonwebtoken jwt = new();
      if (headers.Authorization != null)
      {
        string[] tokenArr = headers.Authorization.Split(" ");
        string token = tokenArr[tokenArr.Length - 1];
        if (jwt.validarToken(token))
        {
          if (value != null)
          {
            using (MySqlConnection conn = new(builder.ConnectionString))
            {
              string validacion = contentObj.agent_name.ToString().ToLower();
              try
              {
                await conn.OpenAsync();
                using (var cmd = new MySqlCommand())
                {
                  cmd.Connection = conn;
                  cmd.CommandText = "CALL SP_InsertGestion(@jsondata, @campaign, @contact, @session, @agent)";
                  cmd.Parameters.AddWithValue("jsondata", contentStr);
                  cmd.Parameters.AddWithValue("contact", contentObj.contact_id.ToString());
                  cmd.Parameters.AddWithValue("campaign", contentObj.campaign_id.ToString());
                  cmd.Parameters.AddWithValue("session", contentObj.session_id.ToString());
                  cmd.Parameters.AddWithValue("agent", contentObj.agent_name.ToString());
                  using (var reader = await cmd.ExecuteReaderAsync())
                  {
                    await reader.ReadAsync();
                    newId = reader.GetString(0);
                  }

                }
                if (validacion == "lili_recagua_collections")
                {
                  HttpClient client = new HttpClient();
                  HttpResponseMessage response = await client.PostAsJsonAsync("http://62.171.184.240:8092/api/VozyAutomatizations", postObj);
                  if ((int)response.StatusCode == 200)
                  {
                    using (var cmd = new MySqlCommand())
                    {
                      cmd.Connection = conn;
                      cmd.CommandText = "UPDATE VozyEndpoint SET sic = 1 WHERE id = @newid";
                      cmd.Parameters.AddWithValue("id", newId);
                      await cmd.ExecuteNonQueryAsync();
                    }
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
          return Unauthorized(new { message = "Token no valido" });
        }
      }
      else
      {
        logs.logError("Sin Token");
        return Unauthorized(new { message = headers });
      }
    }
  }

}
