using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoData _data;

    public readonly ILogger<TodosController> _logger;

    public TodosController(ITodoData data, ILogger<TodosController> logger)
    {
        this._data = data;
        _logger = logger;
    }

    private int GetUserId()
    {
        var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdText);
    }

    // GET: api/todos
    /// <summary>
    /// Trae todas las tareas del usuario ingresado
    /// </summary>
    /// <returns>Una lista de todas las tareas del usuario</returns>
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpGet(Name = "GetAllTodos")]
    public async Task<ActionResult<List<TodoModel>>> Get()
    {
        // En esta linea se incluye pagination
        // public async Task<ActionResult<List<TodoModel>>> Get(int pageNumber, int pageSize)
        _logger.LogInformation("GET: api/Todos/");
        try
        {
            var output = await _data.GetAllAssigned(GetUserId());
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Todos failed" );
            return BadRequest();
        }
    }

    // GET api/todos/5
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpGet("{todoId}", Name = "GetOneTodo")]
    public async Task<ActionResult<TodoModel>> Get(int todoId)
    {
        _logger.LogInformation("GET: api/Todos/{TodoId}", todoId);
        try
        {
            var output = await _data.GetOneAssigned(GetUserId(), todoId);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to {ApiPath} failed. the Id was {TodoId}",
                $"api/Todos/Id", todoId);
            return BadRequest();
        }
    }

    // POST api/todos
    /// <summary>
    /// Crea una nueva tarea solo ingresando el string "task"
    /// </summary>
    /// <param name="task"></param>
    /// <returns>La Tarea creada</returns>
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpPost(Name = "CreateTodo")]
    public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
    {
        
        _logger.LogInformation("POST: api/Todos (Task: {Task})", task);
        try
        {
            var output = await _data.Create(GetUserId(), task);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to {ApiPath} failed. The Task was {Task}", task);
            return BadRequest();
        }
    }


    // POST api/todos/c
    /// <summary>
    /// Crea una nueva tarea ingresando el string "task", y el boolean "isComplete"
    /// </summary>
    /// <param name="todoModel"></param>
    /// <returns>La Tarea creada</returns>
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpPost("2", Name = "CreateTodo_2")]
    public async Task<ActionResult<TodoModel>> Post_2([FromBody] TodoModel todoModel)
    {
        _logger.LogInformation(@"POST: api/todos/2 con task = {task}", todoModel.Task);
        try
        {
            var output = await _data.Create_2(GetUserId(), todoModel);
            _logger.LogInformation("POST: api/todos/2  ya pasó la parte crítica");
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST: api/todos/2 call failed. La tarea fue {Task}", todoModel.Task);
            return BadRequest();
        }
    }

    // PUT api/todos/5
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpPut("{todoId}", Name = "UpdateTodoTask")]
    public async Task<ActionResult> Put(int todoId, [FromBody] string task)
    {
        _logger.LogInformation("PUT: api/Todos/{TodoId} (Task: {Task})", todoId, task);
        try
        {
            await _data.UpdateTask(GetUserId(), todoId, task);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The PUT call to api/Todos/{TodoId} failed. The Task was {Task}", todoId, task);
            return BadRequest();
        }
    }

    // PUT api/todos/5/Complete
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpPut("{todoId}/Complete", Name = "CompleteTodo")]
    public async Task<IActionResult> Complete(int todoId)
    {
        _logger.LogInformation("PUT: api/Todos/{TodoId}/Complete ", todoId);
        try
        {
            await _data.CompleteTodo(GetUserId(), todoId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The PUT call to api/Todos/{TodoId}/Complete failed. The Task was {Task}", todoId);
            return BadRequest();
        }
    }

    // DELETE api/todos/5
    //[SwaggerOperation(Summary = "some description here", Tags = new[] { "GetAllTodos, GetOneTodo, CreateTodo, CreateTodo_2", "UpdateTodoTask", "CompleteTodo", "DeleteTodo" })]
    [HttpDelete("{todoId}", Name = "DeleteTodo")]
    public async Task<IActionResult> Delete(int todoId)
    {
        _logger.LogInformation("DELETE: api/Todos/{TodoId}/Complete ", todoId);
        try
        {
            await _data.Delete(GetUserId(), todoId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The DELETE call to api/Todos/{TodoId} failed. The Id was {TodoId}", todoId);
            return BadRequest();
        }
    }
}
