namespace WebServer.Services;

/// <summary> 
/// Author:    Tyler DeBruin
/// Partner:   Rayyan Hamid
/// Date:      4-27-2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
///
/// Lists the Database Procedures - In a one stop file. This includes the creation of the database tables.
/// </summary>
public static class DatabaseConstants
{
    public static readonly string InsertPlayer = "INSERT INTO dbo.Players (PlayerName, InsertDate) Values(@PlayerName, @InsertDate); SELECT @@IDENTITY AS [PlayerId] ";
    public static readonly string SelectPlayer = "SELECT PlayerId, PlayerName, InsertDate FROM dbo.Players Where PlayerName = @PlayerName ";
    public static readonly string SelectGame = "SELECT g.[GameId],g.[PlayerId],g.[BornTime],g.[DeathTime],g.[MaxSize],g.[MaxSizeTime],g.[InsertDate], p.[PlayerName] FROM [dbo].[Game] g JOIN dbo.Players p on g.PlayerId = p.PlayerId ";
    public static readonly string InsertGame = "INSERT INTO [dbo].[Game](PlayerID, BornTime, DeathTime, MaxSize, MaxSizeTime, InsertDate) VALUES(@PlayerID, @BornTime, @DeathTime, @MaxSize, @MaxSizeTime, @InsertDate); SELECT @@IDENTITY AS [GameId] ";
    public static readonly string UpdateGame = "UPDATE [dbo].[Game] SET MaxSize = @MaxSize, MaxSizeTime = @MaxSizeTime, DeathTime = @DeathTime WHERE GameId = @GameId ";
    public static readonly string InsertPlayerSize = "INSERT INTO dbo.PlayerSize (GameId, Size, InsertDate) VALUES (@GameId, @Size, @InsertDate); SELECT @@IDENTITY AS [PlayerSizeId] ";
    public static readonly string SelectPlayerSize = "SELECT PlayerSizeId, GameId, Size, InsertDate FROM dbo.PlayerSize WHERE PlayerSizeId = @PlayerSizeId ";

    public static readonly string CreatePlayersTable = "SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE [dbo].[Players] ([PlayerID] [INT] IDENTITY(1,1) NOT NULL PRIMARY KEY,[PlayerName] [VARCHAR](50) NOT NULL,[InsertDate] [DATETIME] NOT NULL) ";
    public static readonly string CreateGameTable = "SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE [dbo].[Game] ([GameId] [INT] IDENTITY(1,1) NOT NULL PRIMARY KEY, [PlayerId] [INT] NOT NULL,[BornTime] [DATETIME] NOT NULL,[DeathTime] [DATETIME],[MaxSize] [DECIMAL](9, 2),[MaxSizeTime] [DATETIME],[InsertDate] [DATETIME] NOT NULL) ";
    public static readonly string CreatePlayerSizeTable = "SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE [dbo].[PlayerSize]([PlayerSizeId] [INT] IDENTITY(1,1) NOT NULL PRIMARY KEY,[GameId] [INT] NOT NULL,[Size] [DECIMAL](9, 2) NOT NULL, [InsertDate] [DATETIME] NOT NULL) ";
}