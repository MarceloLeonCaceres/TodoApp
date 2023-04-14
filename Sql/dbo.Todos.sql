USE [TodoDb]
GO

/****** Object: Table [dbo].[Todos] Script Date: 29/03/2023 14:34:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Todos];


GO
CREATE TABLE [dbo].[Todos] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Task]       NVARCHAR (50) NOT NULL,
    [AssignedTo] INT           NOT NULL,
    [IsComplete] BIT           NOT NULL
);


