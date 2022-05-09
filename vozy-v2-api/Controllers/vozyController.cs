using Microsoft.AspNetCore.Mvc;
using vozy_v2_api.core;
using vozy_v2_api.models;
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vozy_v2_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class vozyController : ControllerBase
  {
    public vozyController()
    {

    }
    [Route("test")]
    [HttpPost]
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
            string validacion = contentObj.agent_name.ToString().ToLower();
            try
            {
              if (validacion == "lili_recagua_collections")
              {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync("http://164.68.125.229:8061/api/VozyAutomatizations", postObj);

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
