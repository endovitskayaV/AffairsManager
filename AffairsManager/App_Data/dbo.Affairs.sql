CREATE TABLE [dbo].[Affairs] (
    [Id]          INT            NOT NULL,
    [Importance]  NVARCHAR (50)  NOT NULL,
    [Name]        NVARCHAR (250) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Date] DATETIME NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

