USE CourseReport;

-- 1
ALTER TABLE [dbo].[Enrollments]
ADD
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE()),
	CreateId VARCHAR(50) NOT NULL DEFAULT('System'),
	UpdateDate DATETIME NOT NULL DEFAULT(GETDATE()),
	UpdateId VARCHAR(50) NOT NULL DEFAULT('System');

-- 2
CREATE PROCEDURE [dbo].[Courses_Get] 
AS
	SELECT 
		CourseId,
		CourseCode,
		[Description]
	FROM
		[dbo].[Courses]

CREATE PROCEDURE [dbo].[Students_Get] 
AS
	SELECT 
		StudentId,
		FirstName,
		LastName
	FROM
		[dbo].[Students]

CREATE PROCEDURE [dbo].[Enrollments_Get] 
AS
	SELECT 
		EnrollmentId,
		StudentId,
		CourseId
	FROM
		[dbo].[Enrollments]

--3
CREATE TYPE [dbo].[EnrollmentType] AS TABLE
(
	[EnrollmentId] INT NOT NULL,
	[StudentId] INT NOT NULL,
	[CourseId] INT NOT NULL
);

-- 4
CREATE PROCEDURE [dbo].[Enrollments_Upsert]
	@EnrollmentType [dbo].[EnrollmentType] READONLY,
	@UserId VARCHAR(50)
AS
	MERGE INTO [dbo].[Enrollments] TARGET
	USING
	(
		SELECT 
			[EnrollmentId],
			[StudentId],
			[CourseId],
			@UserId [UpdateId],
			GETDATE() [UpdateDate],
			@UserId [CreateId],
			GETDATE() [CreateDate]
		FROM
			@EnrollmentType
	) AS SOURCE
	ON
	(
		TARGET.[EnrollmentId] = SOURCE.[EnrollmentId]
	)
	WHEN MATCHED THEN
		UPDATE SET
			TARGET.[StudentId] = SOURCE.[StudentId],
			TARGET.[CourseId] = SOURCE.[CourseId],
			TARGET.[UpdateId] = SOURCE.[UpdateId],
			TARGET.[UpdateDate] = SOURCE.[UpdateDate]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT 
		(
			[StudentId],
			[CourseId],
			[CreateDate],
			[CreateId],
			[UpdateDate],
			[UpdateId]
		)
		VALUES
		(
			SOURCE.[StudentId],
			SOURCE.[CourseId],
			SOURCE.[CreateDate],
			SOURCE.[CreateId],
			SOURCE.[UpdateDate],
			SOURCE.[UpdateId]
		);


