using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoList.Common.Attributes;
using ToDoList.Core.Managers.ToDo;
using ToDoList.ModelViews.ModelViews.ToDo;
using ToDoList.ModelViews.ModelViews.User;
using ToDoList.Attributes;
using System.IO;

namespace ToDoList.Controllers
{
    [ApiController]
    [Authorize]
    public class ToDoController : ApiBaseController
    {

        #region Dependency Injection
        private readonly IToDoManager _toDoManager;
        private readonly ILogger<ToDoController> _logger;

        public ToDoController(ILogger<ToDoController> logger,
                              IToDoManager toDoManager)
        {
            _logger = logger;
            _toDoManager = toDoManager;
        }
        #endregion Dependency Injection

        #region Public
        [HttpGet]
        [Route("api/toDo/allTasks")]
        [ToDoListAuthorize()]
        public IActionResult GetAll()
        {
            var res = _toDoManager.GetAllTasks();
            return Ok(res);
        }

        [HttpGet]
        [Route("api/toDo/myTasks")]
        public IActionResult Get()
        {
            var res = _toDoManager.GetMyTasks(LoggedInUser.Id);
            return Ok(res);
        }

        [Route("api/toDo/add")]
        [HttpPost]
        public IActionResult Add([FromBody] AddToDoMV addToDoMV)
        {
            var res = _toDoManager.AddTask(LoggedInUser,addToDoMV);
            return Ok(res);
        }

        [Route("api/toDo/update")]
        [HttpPut]
        public IActionResult Update([FromBody] UpdateToDoMV updateToDoMV)
        {
            var res = _toDoManager.UpdateTask(LoggedInUser.Id, updateToDoMV);
            return Ok(res);
        }

        [Route("api/toDo/retrieve/{fileName}")]
        [HttpGet]
        public IActionResult Retrieve(string fileName)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{fileName}";
            var byteArray = System.IO.File.ReadAllBytes(folderPath);
            return File(byteArray, "image/jpeg", fileName);
        }

        [Route("api/toDo/delete/{id}")]
        [HttpPatch]
        public IActionResult Delete(int id)
        {
            _toDoManager.DeleteTask(LoggedInUser.Id, id);
            return Ok();
        }

        [Route("api/toDo/hasRead/{id}")]
        [HttpPatch]
        public IActionResult HasRead(int id)
        {
            _toDoManager.HasRead(LoggedInUser.Id, id);
            return Ok();
        }

        [HttpGet]
        [Route("api/toDo/archivedTasks")]
        [ToDoListAuthorize()]
        public IActionResult GetArchived()
        {
            var res = _toDoManager.GetArchivedTasks();
            return Ok(res);
        }

        #endregion Public

    }
}
