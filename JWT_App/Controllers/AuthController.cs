using JWT_App.Models;
using JWT_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthservices _authservices;

        public AuthController(IAuthservices authservices)
        {
            _authservices = authservices;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authservices.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);


            return Ok(result);
        }

        [HttpPost("Token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authservices.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);


            return Ok(result);
        }


       
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authservices.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);


            return Ok(model);
        }




    }
}
