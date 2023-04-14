USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_CompleteTodo] Script Date: 29/03/2023 14:32:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_CompleteTodo];


GO
CREATE PROCEDURE [dbo].[spTodos_CompleteTodo]
	@AssignedTo int,
	@TodoId int
AS
begin
	update dbo.Todos
	set IsComplete = 1
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
