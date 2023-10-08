CREATE TABLE [dbo].[ClassStudent]
(
  [ClassId]   INT NOT NULL
  ,[StudentId] INT NOT NULL

  ,CONSTRAINT [PK_ClassStudent] PRIMARY KEY (StudentId, ClassId)

  ,CONSTRAINT [FK_ClassStudent_Class] FOREIGN KEY ([ClassId]) REFERENCES [Class] ([ClassId])
  ,CONSTRAINT [FK_ClassStudent_Student] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId])
);
GO

ALTER TABLE ClassStudent ADD FOREIGN KEY (StudentId) REFERENCES Student (StudentId);
GO
ALTER TABLE ClassStudent ADD FOREIGN KEY (ClassId) REFERENCES Class (ClassId);
GO
