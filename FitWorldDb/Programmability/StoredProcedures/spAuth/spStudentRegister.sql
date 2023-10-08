CREATE PROCEDURE [dbo].[spStudentRegister]
	@Email VARCHAR(320)
	,@Password VARCHAR(14)
AS
BEGIN
	DECLARE @PasswordHash BINARY(64), @SecurityStamp UNIQUEIDENTIFIER;

	SET @SecurityStamp = NEWID()
	SET @PasswordHash = dbo.fHasher(TRIM(@Password), @SecurityStamp)

	INSERT INTO [Student]
		(Email, [PasswordHash], [SecurityStamp])
	OUTPUT
	inserted.StudentId,
	inserted.Email
	VALUES
		(TRIM(@Email) ,@PasswordHash ,@SecurityStamp)
END
RETURN 0
