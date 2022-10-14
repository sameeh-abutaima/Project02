using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using ToDoList.Common.Attributes;
using ToDoList.Core.Managers.User;
using ToDoList.ModelViews.ModelViews.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ApiBaseController
    {
        #region Dependency Injection
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        #endregion Dependency Injection

        #region End Points

        [HttpGet]
        [Route("api/user/users")]
        [ToDoListAuthorize()]
        public IActionResult Get()
        {
            var res = _userManager.GetUsers();
            return Ok(res);
        }

        [HttpGet]
        [Route("api/user/archivedUsers")]
        [ToDoListAuthorize()]
        public IActionResult GetArchived()
        {
            var res = _userManager.GetArchivedUsers();
            return Ok(res);
        }

        [Route("api/user/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] SignUpUserMV signUpUserMV)
        {
            var res = _userManager.SignUp(signUpUserMV);
            return Ok(res);
        }

        [Route("api/user/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserMV loginUserMV)
        {
            var res = _userManager.Login(loginUserMV);
            return Ok(res);
        }

        [Route("api/user/updateMyProfile")]
        [HttpPut]
        public IActionResult UpdateMyProfile(UserMV userMV)
        {
            var user = _userManager.UpdateProfile(LoggedInUser, userMV);
            return Ok(user);
        }

        [Route("api/user/retrieve/{fileName}")]
        [HttpGet]
        public IActionResult Retrieve(string fileName)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{fileName}";
            var byteArray = System.IO.File.ReadAllBytes(folderPath);
            return File(byteArray, "image/jpeg", fileName);
        }

        [HttpPatch]
        [Route("api/user/delete/{id}")]
        [ToDoListAuthorize()]
        public IActionResult Delete(int id)
        {
            _userManager.DeleteUser(LoggedInUser, id);
            return Ok();
        }

        #endregion End Points
    }
}
