CREATE FUNCTION [dbo].[fHasher]
(
	@Password VARCHAR(128),
	@SecurityStamp UNIQUEIDENTIFIER
)
RETURNS BINARY(64)
AS
BEGIN
	DECLARE @hashedValue BINARY(64) = CONVERT(
		BINARY(64),
		CONCAT(
			HASHBYTES(
				'SHA2_512',
				CONCAT(@Password, CAST(@SecurityStamp AS VARCHAR(36)))),
				@SecurityStamp))
	RETURN @hashedValue
END
