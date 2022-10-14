--01-Create Database
CREATE DATABASE ToDoListDb;
GO
use ToDoListDb;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[Users](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FirstName] [nvarchar](50) NOT NULL,
		[LastName] [nvarchar](50) NOT NULL,
		[Password] [nvarchar](max) NOT NULL,
		[ConfirmPassword] [nvarchar](max) NOT NULL,
		[ImageUrl] [nvarchar](max) NOT NULL,
		Email nvarchar(256) NOT NULL,
		[CreatedAt] [datetime] NOT NULL DEFAULT getutcdate(),
		[UpdatedAt] [datetime] NOT NULL DEFAULT getutcdate(),
		[IsAdmin] [bit] NOT NULL DEFAULT (0),
		[Archived] [bit] NOT NULL DEFAULT (0),
		CONSTRAINT UK_email UNIQUE(Email),
		CONSTRAINT [PK_Users] PRIMARY KEY([ID])
	);
END;

GO

GO

IF OBJECT_ID(N'dbo.ToDoes', N'U') IS NULL 
BEGIN 
	CREATE TABLE [dbo].[ToDoes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
		[Content] TEXT NOT NULL DEFAULT '',
		[ImageUrl] [nvarchar](max) NOT NULL,
		[CreatedAt] [datetime] NOT NULL DEFAULT getutcdate(),
		[UpdatedAt] [datetime] NOT NULL DEFAULT getutcdate(),
		[AssignedTo] [int] NOT NULL,
		[AssignedBy] [int] NOT NULL,
		[IsRead] [bit] NOT NULL DEFAULT (0),
		[Archived] [bit] NOT NULL DEFAULT (0),
		CONSTRAINT [PK_ToDoes] PRIMARY KEY([ID]),
		CONSTRAINT [FK_ToDoes_Users_AssignedTo] FOREIGN KEY([AssignedTo]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
		CONSTRAINT [FK_ToDoes_Users_AssignedBy] FOREIGN KEY([AssignedBy]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	);
END;

GO
