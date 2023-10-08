CREATE TABLE [dbo].[Student]
(
  [StudentId]     INT              NOT NULL IDENTITY (1, 1)
  -- ,[UserName]  VARCHAR (64)  NOT NULL -- TRIM()
  ,[Email]         VARCHAR (320)    UNIQUE NOT NULL -- TRIM()
  -- ,[Password]  VARCHAR (14)  NOT NULL -- TRIM()
  ,[PasswordHash]  BINARY(64)       NULL
  ,[SecurityStamp] UNIQUEIDENTIFIER NULL

  ,CONSTRAINT [PK_Student] PRIMARY KEY ([StudentId])

  -- ,CONSTRAINT [CK_Student_UserName] CHECK (LEN ([UserName]) >= 1)
  ,CONSTRAINT [CK_Student_Email] CHECK (LEN ([Email]) >= 5)
  -- ,CONSTRAINT [CK_Student_Password] CHECK (LEN ([Password]) >= 6)
);
GO
