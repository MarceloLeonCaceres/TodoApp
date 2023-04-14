using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private readonly ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        this._sql = sql;
    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {

        return _sql.LoadData<TodoModel, dynamic>("spTodos_GetAllAssigned",
            new { AssignedTo = assignedTo }, "Default");

        // desde sql server llamaríamos al stored procedure así:
        // exec dbo.spTodos_GetAllAssigned @AssignedTo = 3, @TodoId = 1
    }

    public async Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId)
    {

        var result = await _sql.LoadData<TodoModel, dynamic>("spTodos_GetOneAssigned",
            new { AssignedTo = assignedTo, TodoId = todoId }, "Default");

        return result.FirstOrDefault();

    }
    public async Task<TodoModel?> Create(int assignedTo, string task)
    {

        var result = await _sql.LoadData<TodoModel, dynamic>("spTodos_Create",
            new { AssignedTo = assignedTo, Task = task }, "Default");

        return result.FirstOrDefault();

    }

    public async Task<TodoModel?> Create_2(int assignedTo, string task, bool completado)
    {
        var result = await _sql.LoadData<TodoModel, dynamic>("spTodos_Create_2",
            new { AssignedTo = assignedTo, Task = task, Completado = completado }, "Default");
        return result.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {

        return _sql.SaveData<dynamic>("spTodos_UpdateTask",
            new { AssignedTo = assignedTo, TodoId = todoId, Task = task }, "Default");

    }
    public Task CompleteTodo(int assignedTo, int todoId)
    {

        return _sql.SaveData<dynamic>("spTodos_CompleteTodo",
            new { AssignedTo = assignedTo, TodoId = todoId }, "Default");

    }
    public Task Delete(int assignedTo, int todoId)
    {

        return _sql.SaveData<dynamic>("spTodos_Delete",
            new { AssignedTo = assignedTo, TodoId = todoId }, "Default");

    }
}
