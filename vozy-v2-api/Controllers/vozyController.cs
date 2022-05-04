using Microsoft.AspNetCore.Mvc;
using vozy_v2_api.core;
using vozy_v2_api.models;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Text;

namespace vozy_v2_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class vozyController : ControllerBase
  {
    private string connection = "Data Source=62.171.184.240;Initial Catalog=Vozy;User ID=SA;Password=8e3lR5Nz3ago";
    public vozyController() { }

    // POST api/vozy/token
    [Route("token")]
    [HttpPost]
    public IActionResult Post([FromHeader] headers headers)
    {
      if (headers.authorization == null)
      {
        return BadRequest(new { message = "No se encontraron las credenciales" });
      }
      else
      {
        string[] token = headers.authorization.Split(" ");
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
      jsonwebtoken jwt = new();
      if (headers.authorization != null)
      {
        string[] tokenArr = headers.authorization.Split(" ");
        string token = tokenArr[tokenArr.Length-1];
        if (jwt.validarToken(token))
        {
          if (value != null)
          {
            using (SqlConnection db = new(connection))
            {
              string validacion = contentObj.agent_name.ToString().ToLower();
              string query = $"INSERT INTO VozyEndpoint (jsonData,campaignId,contactId,sessionId,agentName) output INSERTED.ID VALUES(@jsondata, @campaign, @contact, @session, @agent)";
              object parametros = new
              {
                jsondata = contentStr,
                contact = contentObj.contact_id.ToString(),
                campaign = contentObj.campaign_id.ToString(),
                session = contentObj.session_id.ToString(),
                agent = contentObj.agent_name.ToString()
              };
              try
              {
                var newid = db.ExecuteScalar(query, parametros);
                if (validacion == "lili_recagua_collections")
                {
                  HttpClient client = new HttpClient();
                  HttpResponseMessage response = await client.PostAsJsonAsync("http://62.171.184.240:8092/api/VozyAutomatizations", postObj);
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
          return Unauthorized(new { message = "Token no valido" });
        }
      }
      else
      {
        logs.logError("Sin Token");
        return Unauthorized(new { message = "Sin Token" });
      }
    }
  }

}
