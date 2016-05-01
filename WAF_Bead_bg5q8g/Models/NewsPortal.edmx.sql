
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/01/2016 22:38:39
-- Generated from EDMX file: D:\shared\waf\WAF_Bead_bg5q8g\WAF_Bead_bg5q8g\Models\NewsPortal.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [News_Portal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ArticleImage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Images] DROP CONSTRAINT [FK_ArticleImage];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Articles];
GO
IF OBJECT_ID(N'[dbo].[Images]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Images];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Articles'
CREATE TABLE [dbo].[Articles] (
    [Id] uniqueidentifier  NOT NULL,
    [Title] nchar(20)  NULL,
    [Author] uniqueidentifier  NULL,
    [Date] binary(8)  NULL,
    [Summary] nchar(30)  NULL,
    [Content] nchar(500)  NULL,
    [IsLead] bit  NULL,
    [UserId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Images'
CREATE TABLE [dbo].[Images] (
    [Id] uniqueidentifier  NOT NULL,
    [News_Id] uniqueidentifier  NOT NULL,
    [Image1] varbinary(max)  NOT NULL,
    [Article_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] uniqueidentifier  NOT NULL,
    [name] nchar(50)  NULL,
    [password] nchar(12)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [PK_Articles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [PK_Images]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Article_Id] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [FK_ArticleImage]
    FOREIGN KEY ([Article_Id])
    REFERENCES [dbo].[Articles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleImage'
CREATE INDEX [IX_FK_ArticleImage]
ON [dbo].[Images]
    ([Article_Id]);
GO

-- Creating foreign key on [UserId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_UserArticle]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserArticle'
CREATE INDEX [IX_FK_UserArticle]
ON [dbo].[Articles]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------