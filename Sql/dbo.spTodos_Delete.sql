USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_Delete] Script Date: 29/03/2023 14:33:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[spTodos_Delete];


GO
CREATE PROCEDURE [dbo].[spTodos_Delete]
	@AssignedTo int,
	@TodoId int
AS
begin
	delete from dbo.Todos	
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
