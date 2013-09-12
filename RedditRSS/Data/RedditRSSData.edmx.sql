
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/12/2013 10:13:21
-- Generated from EDMX file: C:\Users\Devon\SkyDrive\Projects\reddit-rss-bot\RedditRSS\Data\RedditRSSData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [RedditRSS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__RedditRSS__Reddi__164452B1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RedditRSSBotData] DROP CONSTRAINT [FK__RedditRSS__Reddi__164452B1];
GO
IF OBJECT_ID(N'[dbo].[FK__RedditUse__AppUs__1273C1CD]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RedditUser] DROP CONSTRAINT [FK__RedditUse__AppUs__1273C1CD];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AppUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppUser];
GO
IF OBJECT_ID(N'[dbo].[RedditRSSBotData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RedditRSSBotData];
GO
IF OBJECT_ID(N'[dbo].[RedditUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RedditUser];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AppUsers'
CREATE TABLE [dbo].[AppUsers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(100)  NOT NULL,
    [HashedPassword] varchar(max)  NOT NULL
);
GO

-- Creating table 'RedditRSSBotDatas'
CREATE TABLE [dbo].[RedditRSSBotDatas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FeedUrl] varchar(500)  NOT NULL,
    [Subreddit] varchar(100)  NOT NULL,
    [Interval] int  NOT NULL,
    [RedditUserID] int  NULL
);
GO

-- Creating table 'RedditUsers'
CREATE TABLE [dbo].[RedditUsers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(100)  NOT NULL,
    [HashedPassword] varchar(255)  NOT NULL,
    [AppUserID] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [PK_AppUsers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RedditRSSBotDatas'
ALTER TABLE [dbo].[RedditRSSBotDatas]
ADD CONSTRAINT [PK_RedditRSSBotDatas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RedditUsers'
ALTER TABLE [dbo].[RedditUsers]
ADD CONSTRAINT [PK_RedditUsers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AppUserID] in table 'RedditUsers'
ALTER TABLE [dbo].[RedditUsers]
ADD CONSTRAINT [FK__RedditUse__AppUs__1273C1CD]
    FOREIGN KEY ([AppUserID])
    REFERENCES [dbo].[AppUsers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__RedditUse__AppUs__1273C1CD'
CREATE INDEX [IX_FK__RedditUse__AppUs__1273C1CD]
ON [dbo].[RedditUsers]
    ([AppUserID]);
GO

-- Creating foreign key on [RedditUserID] in table 'RedditRSSBotDatas'
ALTER TABLE [dbo].[RedditRSSBotDatas]
ADD CONSTRAINT [FK__RedditRSS__Reddi__164452B1]
    FOREIGN KEY ([RedditUserID])
    REFERENCES [dbo].[RedditUsers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__RedditRSS__Reddi__164452B1'
CREATE INDEX [IX_FK__RedditRSS__Reddi__164452B1]
ON [dbo].[RedditRSSBotDatas]
    ([RedditUserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------