﻿using TodoLibrary.Models;

namespace TodoLibrary.DataAccess
{
    public interface ITodoData
    {
        Task CompleteTodo(int assignedTo, int todoId);
        Task<TodoModel?> Create(int assignedTo, string task);
        Task<TodoModel?> Create_2(int assignedTo, TodoModel todoModel);
        Task Delete(int assignedTo, int todoId);
        Task<List<TodoModel>> GetAllAssigned(int assignedTo);
        Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId);
        Task UpdateTask(int assignedTo, int todoId, string task);
    }
}