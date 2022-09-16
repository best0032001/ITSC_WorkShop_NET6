using Demo.Model.Entitys;
using Demo.Model.Interface;
using Demo.Model.Repository;
using ITSC_API_GATEWAY_LIB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserPortalController : ITSCController
    {
        private IUserRepository _userRepository;
        public UserPortalController(IHttpClientFactory clientFactory, ITSCServer iTSCServer, ILogger<ITSCController> logger, IUserRepository userRepository)
        {
            this.loadConfig(logger, iTSCServer, clientFactory);
            _userRepository = userRepository;
        }

        private async Task<IActionResult> checkUser(String action)
        {
            if (!await this.checkApp())
            {
                return this.UnauthorizedITSC(action);
            }
            return Ok();
        }
        [HttpGet("v1/User")]
        [ProducesResponseType(typeof(UserEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getUser()
        {
            String action = "UserPortalController.getUser";
            this.beginActionITSC(action);
            try
            {
                IActionResult actionResult = await checkUser(action);
                if ((int)(HttpStatusCode)actionResult.GetType()
                .GetProperty("StatusCode")
                .GetValue(actionResult, null) != 200)
                {
                    return actionResult;
                }

                UserEntity userEntity=await _userRepository.getUserEntity(this._accesstoken);
                if (userEntity == null) { return StatusCodeITSC(action, 204);  }
                APIModel aPIModel = new APIModel();
                aPIModel.data = userEntity;
                aPIModel.message = "Success";
                return OkITSC(aPIModel, action);

            }
            catch (Exception ex)
            {
                return this.StatusErrorITSC(action, ex);
            }
        }
    }
}
