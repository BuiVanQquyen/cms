using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeeyPage.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [HttpPost]
        [Route("create-user")]
        public ActionResult<bool> InsertUserFromMeeyId()
        {
            return Ok(true);
        }
    }
}
