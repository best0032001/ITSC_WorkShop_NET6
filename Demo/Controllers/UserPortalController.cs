using Demo.Model.Entitys;
using Demo.Model.Interface;
using Demo.Model.Repository;
using EmailItscLib.ITSC.Interface;
using EmailItscLib.ITSC.Repository;
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
        private IEmailRepository _emailRepository;
        public UserPortalController(IHttpClientFactory clientFactory, ITSCServer iTSCServer, ILogger<ITSCController> logger, IUserRepository userRepository,IEmailRepository emailRepository)
        {
            this.loadConfig(logger, iTSCServer, clientFactory);
            _userRepository = userRepository;
            _emailRepository = emailRepository;
        }

        private async Task<IActionResult> checkUser(String action)
        {
            if (!await this.checkApp())
            {
                return this.UnauthorizedITSC(action);
            }
            return Ok();
        }
         /// <summary>
        /// API สำหรับ ************************************************************************
        /// </summary>
        /// <remarks>
        ///  demo [{OrganizationID:0,BudgetYear:0,BudgetName:0,BudgetCode:0,BudgetReferenceCode:0} , {OrganizationID:0,BudgetYear:0,BudgetName:0,BudgetCode:0,BudgetReferenceCode:0} ]
        /// </remarks>
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
                await _emailRepository.SendEmailAsync("test", userEntity.Email, "test", "  ", null);
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
