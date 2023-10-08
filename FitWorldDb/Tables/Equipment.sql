CREATE TABLE [dbo].[Equipment]
(
  [EquipmentId] INT           NOT NULL IDENTITY(1, 1)
  ,[Name]        VARCHAR (42)  NOT NULL
  ,[Price]       DECIMAL(19,4) NOT NULL

  ,CONSTRAINT [PK_Equipment] PRIMARY KEY ([EquipmentId])
  ,CONSTRAINT [AK_Equipment_Name_Price] UNIQUE ([Name], [Price])

  ,CONSTRAINT [CK_Equipment_Name] CHECK (LEN ([Name]) >= 1)
  ,CONSTRAINT [CK_Equipment_Price] CHECK ([Price] >= 0.01)
);
GO
