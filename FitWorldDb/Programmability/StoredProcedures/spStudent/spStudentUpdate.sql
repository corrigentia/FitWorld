-- Create a new stored procedure called 'spStudentUpdate' in schema 'dbo'
-- Create the stored procedure in the specified schema
CREATE PROCEDURE [dbo].[spStudentUpdate]
	-- ,@param1 /*parameter name*/ INT /*datatype_for_param1*/ = 0
	/*default_value_for_param1*/
	-- ,@param2 /*parameter name*/ INT /*datatype_for_param1*/ = 0
	/*default_value_for_param2*/

	@StudentId INT
	,@Email VARCHAR(320)
	,@Password VARCHAR(14)
-- add more stored procedure parameters here
AS
BEGIN
	-- body of the stored procedure
	DECLARE @PasswordHash BINARY(64), @SecurityStamp UNIQUEIDENTIFIER;

	SET @SecurityStamp = NEWID()
	SET @PasswordHash = dbo.fHasher(TRIM(@Password), @SecurityStamp)

	-- Update rows in table '[Student]' in schema '[dbo]'
	UPDATE [dbo].[Student]
	SET
		[Email] = TRIM(@Email),
		[PasswordHash] = @PasswordHash,
		[SecurityStamp] = @SecurityStamp
		-- Add more columns and values here
	WHERE [StudentId] = @StudentId
/* add search conditions here */
END
-- GO
-- example to execute the stored procedure we just created
-- EXECUTE dbo.spStudentUpdate 1 /*value_for_param1*/, 2 /*value_for_param2*/
-- GO

RETURN 0
