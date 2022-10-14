using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoList.Common.Attributes;
using ToDoList.Core.Managers.ToDo;
using ToDoList.ModelViews.ModelViews.ToDo;
using ToDoList.ModelViews.ModelViews.User;
using ToDoList.Attributes;

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
            if(LoggedInUser.Id!= addToDoMV.AssignedTo)
            {
                var res=AddTaskByAdmin(addToDoMV);
                return Ok(res);
            }else
            {
                var res = _toDoManager.AddTask(addToDoMV, addToDoMV.AssignedTo);
                return Ok(res);
            }          
        }

        [Route("api/toDo/update")]
        [HttpPut]
        public IActionResult Update([FromBody]UpdateToDoMV updateToDoMV)
        {
            var res=_toDoManager.UpdateTask(updateToDoMV,LoggedInUser.Id);
            return Ok(res);
        }


        [Route("api/toDo/delete/{id}")]
        [HttpPatch]
        public IActionResult Delete(int id)
        {
            _toDoManager.DeleteTask(id,LoggedInUser.Id);
            return Ok();
        }

        [Route("api/toDo/hasRead/{id}")]
        [HttpPatch]
        public IActionResult HasRead(int id)
        {
            _toDoManager.HasRead(id, LoggedInUser.Id);
            return Ok();
        }

        [HttpGet]
        [Route("api/toDo/archivedTasks")]
        [ToDoListAuthorize()]
        public IActionResult GetArchived()
        {
            var res=_toDoManager.GetArchivedTasks();
            return Ok(res);
        }

        #endregion Public

        #region Private

        [Route("api/toDo/addTaskByAdmin")]
        [HttpPost]
        [ToDoListAuthorize]
        private ToDoMV AddTaskByAdmin(AddToDoMV addToDoMV)
                                    => _toDoManager.AddTask(addToDoMV,LoggedInUser.Id);

        #endregion Private

    }
}
