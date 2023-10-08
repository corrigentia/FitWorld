CREATE PROCEDURE [dbo].[spStudentLogIn]
	@Email VARCHAR(320)
	,@Password VARCHAR(14)
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @PasswordHash BINARY(64), @SecurityStamp UNIQUEIDENTIFIER

	SET @SecurityStamp = (SELECT
		[SecurityStamp]
	FROM
		[Student]
	WHERE Email = @Email)

	SET @PasswordHash = dbo.fHasher(@Password, @SecurityStamp)

	IF EXISTS
	(SELECT
		TOP 1
		*
	FROM
		[Student]
	WHERE Email = @Email AND [PasswordHash] = @PasswordHash)

	BEGIN
		SELECT
			*
		INTO #TempStudent
		FROM
			[Student]
		WHERE Email LIKE @Email

		ALTER TABLE #TempStudent
	DROP COLUMN PasswordHash, SecurityStamp
		SELECT
			*
		FROM
			#TempStudent
		DROP TABLE #TempStudent
	END
	RETURN 0
END
