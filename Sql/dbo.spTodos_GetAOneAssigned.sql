USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_GetAOneAssigned] Script Date: 29/03/2023 14:33:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_GetAOneAssigned];


GO
CREATE PROCEDURE [dbo].[spTodos_GetAOneAssigned]
	@AssignedTo int,
	@TodoId int
AS
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo 
		and Id = @TodoId;
end
