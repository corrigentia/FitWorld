CREATE TABLE [dbo].[Instructor]
(
  [InstructorId] INT            NOT NULL IDENTITY (1, 1)
  -- ,[UserName]     VARCHAR (64)   NOT NULL
  -- ,[Email]        VARCHAR (320)  NOT NULL
  -- ,[Password]     VARCHAR (14)   NOT NULL
  ,[FirstName]    NVARCHAR (255) NOT NULL
  ,[LastName]     NVARCHAR (255)

  ,CONSTRAINT [PK_Instructor] PRIMARY KEY ([InstructorId])  
  ,CONSTRAINT [AK_Instructor_FirstName_LastName] UNIQUE ([FirstName], [LastName])

  ,CONSTRAINT [CK_Instructor_FirstName] CHECK (LEN ([FirstName]) >= 1)
  -- ,CONSTRAINT [CK_Instructor_UserName] CHECK (LEN ([UserName]) >= 1)
  -- ,CONSTRAINT [CK_Instructor_Email] CHECK (LEN ([Email]) >= 5)
  -- ,CONSTRAINT [CK_Instructor_Password] CHECK (LEN ([Password]) >= 6)
);
GO
