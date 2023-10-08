CREATE TABLE [dbo].[Class]
(
  [ClassId]      INT           NOT NULL IDENTITY (1, 1)
  ,[MartialArtId] INT           NOT NULL
  ,[InstructorId] INT           NOT NULL
  ,[DateTime]     SMALLDATETIME NOT NULL
  ,[PricePerHour] DECIMAL(19,4) NOT NULL

  ,CONSTRAINT [PK_Class] PRIMARY KEY (ClassId)
  ,CONSTRAINT [AK_Class_InstructorId_DateTime] UNIQUE ([InstructorId], [DateTime])

  ,CONSTRAINT [CK_Class_PricePerHour] CHECK ([PricePerHour] >= 0.01)

  ,CONSTRAINT [FK_Class_MartialArt] FOREIGN KEY ([MartialArtId]) REFERENCES [MartialArt] ([MartialArtId])
  ,CONSTRAINT [FK_Class_Instructor] FOREIGN KEY ([InstructorId]) REFERENCES [Instructor] ([InstructorId])
);
GO
