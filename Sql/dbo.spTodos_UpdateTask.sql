USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_UpdateTask] Script Date: 29/03/2023 14:33:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_UpdateTask];


GO
CREATE PROCEDURE [dbo].[spTodos_UpdateTask]
	@Task nvarchar(50),
	@AssignedTo int,
	@TodoId int
AS
begin
	update dbo.Todos
	set Task = @Task
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
