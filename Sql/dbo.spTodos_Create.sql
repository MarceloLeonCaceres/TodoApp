USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_Create] Script Date: 29/03/2023 14:33:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_Create];


GO
CREATE PROCEDURE [dbo].[spTodos_Create]
	@Task nvarchar(50),
	@AssignedTo int
AS
begin
	insert into dbo.Todos (Task, AssignedTo)
	values (@Task, @AssignedTo);

	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where Id = SCOPE_IDENTITY();
end
