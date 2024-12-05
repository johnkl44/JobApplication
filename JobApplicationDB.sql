CREATE DATABASE JobApplicationDB

USE JobApplicationDB

CREATE TABLE tblStudentApplications
(
    ApplicationID INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(15),
	Place NVARCHAR(50),
	DOB DATE,
    PhotoFile VARBINARY(MAX), -- Image
    ResumeFile VARBINARY(MAX) -- PDF
);

SELECT * FROM tblStudentApplications

-- Retrive Applications

ALTER PROCEDURE SPR_JobApplication
AS
BEGIN
	SELECT ApplicationID,FirstName,LastName,Email,Phone,Place,DOB,PhotoFile,ResumeFile FROM tblStudentApplications WITH(NOLOCK)
END

SELECT * FROM tblStudentApplications

--- Insert Data


CREATE PROCEDURE SPI_JobApplication
(
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(15),
	@Place NVARCHAR(50),
	@DOB DATE,
    @PhotoFile VARBINARY(MAX), -- Image
    @ResumeFile VARBINARY(MAX) -- PDF
)
AS
BEGIN

DECLARE @RowCount int = 0 	
SET @RowCount = (SELECT COUNT(1) FROM tblStudentApplications WHERE Email=@Email )


	BEGIN TRY
		BEGIN TRAN
		IF(@RowCount = 0)
			BEGIN
				INSERT INTO tblStudentApplications(FirstName,LastName,Email,Phone,Place,DOB,PhotoFile,ResumeFile) 
				VALUES (@FirstName,@LastName,@Email,@Phone,@Place,@DOB,@PhotoFile,@ResumeFile)
			END
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		SELECT ERROR_MESSAGE()
	END CATCH
END

EXEC SPI_JobApplication 'dev','bhaskar','dev@gmail.com','9658741230','Kochi','10/10/1997',NULL,NULL

EXEC SPR_JobApplication

--- Get appplication by id

CREATE PROCEDURE SP_GetById
(@ApplicationID INT )
AS
BEGIN
	SELECT [ApplicationID],[FirstName],[LastName],[Email],[Phone],[Place],[DOB],[PhotoFile],[ResumeFile]
	FROM tblStudentApplications
	WHERE ApplicationID = @ApplicationID
END

--- Update Appliction

CREATE PROCEDURE SPU_JobApplication
(
	@ApplicationID INT,  
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(15),
	@Place NVARCHAR(50),
	@DOB DATE,
    @PhotoFile VARBINARY(MAX),
    @ResumeFile VARBINARY(MAX) 
)
AS
BEGIN
DECLARE @RowCount int = 0 	
SET @RowCount = (SELECT COUNT(1) FROM tblStudentApplications WHERE Email=@Email and ApplicationID <> @ApplicationID)
	BEGIN TRY
		BEGIN TRAN
		IF(@RowCount = 0)
			BEGIN
				UPDATE tblStudentApplications
				SET 
						FirstName = @FirstName ,
						LastName =	@LastName,
						Email =		@Email,
						Phone =		@Phone,
						Place = 	@Place,
						DOB =       @DOB,
						PhotoFile = @PhotoFile,
						ResumeFile = @ResumeFile 
				WHERE APplicationID = @ApplicationID  	
			END
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		SELECT ERROR_MESSAGE()
	END CATCH
END

---Delete Application

CREATE PROCEDURE SPD_JobApplication
(
@ApplicationID INT,
@ReturnMessage NVARCHAR(100) OUTPUT
)
AS
BEGIN
	DECLARE @RowCount int = 0

	BEGIN TRY
	SET @RowCount = (SELECT (1)	FROM tblStudentApplications WHERE ApplicationID = @ApplicationID)
	IF (@RowCount > 0)
	BEGIN
			BEGIN TRAN
				DELETE FROM tblStudentApplications
				WHERE ApplicationID = @ApplicationID
				SET @ReturnMessage = 'Application Deleted Successfuly ...!'
			COMMIT TRAN
	END

	ELSE

	BEGIN
		SET @ReturnMessage = 'Application not avilable with ID : ' + CONVERT(VARCHAR,@ApplicationID)
	END

	END TRY
BEGIN CATCH
	ROLLBACK TRAN
	SET @ReturnMessage = ERROR_MESSAGE()
END CATCH
END


