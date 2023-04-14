USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_GetAllAssigned] Script Date: 29/03/2023 14:33:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_GetAllAssigned];


GO
CREATE PROCEDURE [dbo].[spTodos_GetAllAssigned]
	@AssignedTo int
AS
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo;
end
