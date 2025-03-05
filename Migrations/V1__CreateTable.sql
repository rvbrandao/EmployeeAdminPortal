CREATE TABLE [Employees] (
          [Id] uniqueidentifier NOT NULL,
          [Name] nvarchar(max) NOT NULL,
          [Email] nvarchar(max) NOT NULL,
          [Phone] nvarchar(max) NULL,
          [Salary] decimal(18,2) NOT NULL,
          CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
      );