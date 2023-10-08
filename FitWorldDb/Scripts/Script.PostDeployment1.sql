/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:      :setvar TableName MyTable
               SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/

USE [FitWorld];
GO
SET IDENTITY_INSERT [dbo].[Equipment] ON

INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (1 ,N'Bo stick' ,129.9000)
INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (2 ,N'Bokken' ,22.0000)
INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (3 ,N'Hanbo' ,16.9000)
INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (4 ,N'Katana' ,169.0000)
INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (5 ,N'Tanto' ,90.0000)
INSERT [dbo].[Equipment]
    ([EquipmentId], [Name], [Price])
VALUES
    (6 ,N'Uniform' ,117.5000)
SET IDENTITY_INSERT [dbo].[Equipment] OFF
GO

USE [FitWorld]
GO
SET IDENTITY_INSERT [dbo].[MartialArt] ON

INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (1 ,N'Aikido')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (2 ,N'Baguazhang')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (3 ,N'Boxing')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (4 ,N'Brazilian JuJutsu')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (5 ,N'JKD')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (6 ,N'Judo')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (7 ,N'Karate')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (8 ,N'Kendo')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (9 ,N'Kickboxing')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (10 ,N'MMA')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (11 ,N'Muay Thai')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (12 ,N'Ninjutsu')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (13 ,N'Shaolin Kung Fu')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (14 ,N'Tai chi')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (15 ,N'TKD')
INSERT [dbo].[MartialArt]
    ([MartialArtId], [Name])
VALUES
    (16 ,N'Wing Chun')
SET IDENTITY_INSERT [dbo].[MartialArt] OFF
GO


USE [FitWorld]
GO
SET IDENTITY_INSERT [dbo].[Instructor] ON

INSERT [dbo].[Instructor]
    ([InstructorId], [FirstName], [LastName])
VALUES
    (1 ,N'Bruce' ,N'Lee')
INSERT [dbo].[Instructor]
    ([InstructorId], [FirstName], [LastName])
VALUES
    (2 ,N'Jet' ,N'Li')
INSERT [dbo].[Instructor]
    ([InstructorId], [FirstName], [LastName])
VALUES
    (3 ,N'Ip' ,N'Man')
INSERT [dbo].[Instructor]
    ([InstructorId], [FirstName], [LastName])
VALUES
    (4 ,N'Mike' ,N'Tyson')
INSERT [dbo].[Instructor]
    ([InstructorId], [FirstName], [LastName])
VALUES
    (5 ,N'Muhammad' ,N'Ali')
SET IDENTITY_INSERT [dbo].[Instructor] OFF
GO

