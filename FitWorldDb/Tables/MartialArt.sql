CREATE TABLE [dbo].[MartialArt]
(
  [MartialArtId] INT          NOT NULL IDENTITY (1, 1)
  ,[Name]         VARCHAR (42) NOT NULL

  ,CONSTRAINT [PK_MartialArt] PRIMARY KEY ([MartialArtId])
  ,CONSTRAINT [AK_MartialArt_Name] UNIQUE ([Name])

  ,CONSTRAINT [CK_MartialArt_Name] CHECK (LEN ([Name]) >= 1)
);
GO
