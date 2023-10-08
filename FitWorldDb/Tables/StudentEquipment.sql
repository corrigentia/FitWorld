CREATE TABLE [dbo].[StudentEquipment]
(
  [StudentId]   INT     NOT NULL
  ,[EquipmentId] INT     NOT NULL
  ,[Quantity]    TINYINT DEFAULT 1 NOT NULL

  ,CONSTRAINT [PK_StudentEquipment] PRIMARY KEY ([StudentId], [EquipmentId])

  ,CONSTRAINT [CK_StudentEquipment_Quantity] CHECK (LEN ([Quantity]) >= 1)

  ,CONSTRAINT [FK_StudentEquipment_Student] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId])
  ,CONSTRAINT [FK_StudentEquipment_Equipment] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipment] ([EquipmentId])
);
GO

ALTER TABLE StudentEquipment ADD FOREIGN KEY (EquipmentId) REFERENCES Equipment (EquipmentId);
GO
ALTER TABLE StudentEquipment ADD FOREIGN KEY (StudentId) REFERENCES Student (StudentId);
GO
