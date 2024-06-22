													--Feedback Entity

create table Feedback(
FeedbackId int primary key identity,
UserId int foreign key references User_profile(UserId),
UserName nvarchar(100) not null,
BookId int foreign key references Books(BookId),
Rating int not null,
constraint chk_feedback_rating check(Rating between 1 and 5),
Review nvarchar(max) not null,
CreatedAt datetime default getdate(),
UpdatedAt datetime default getdate()
)

select * from Feedback

--**********************************************************************************************************************************
														-- Add feedback

CREATE or alter PROCEDURE usp_AddFeedback
    @UserId INT,
    @BookId INT,
    @Rating INT,
    @Review NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate the rating
    IF @Rating < 1 OR @Rating > 5
    BEGIN
        RAISERROR('Rating must be between 1 and 5.', 16, 1);
        RETURN;
    END

    -- Check if the book exists
    IF NOT EXISTS (SELECT 1 FROM Books WHERE BookId = @BookId)
    BEGIN
        RAISERROR('Book does not exist.', 16, 1);
        RETURN;
    END

    -- Check if the user exists
    IF NOT EXISTS (SELECT 1 FROM User_profile WHERE UserId = @UserId)
    BEGIN
        RAISERROR('User does not exist.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    DECLARE @FeedbackId INT;

    -- Add feedback
    INSERT INTO Feedback (UserId, UserName, BookId, Rating, Review, CreatedAt, UpdatedAt)
    VALUES (
        @UserId,
        (SELECT fullname FROM User_profile WHERE UserId = @UserId),
        @BookId,
        @Rating,
        @Review,
        GETDATE(),
        GETDATE()
    );

    -- Get the inserted FeedbackId
    SET @FeedbackId = SCOPE_IDENTITY();

    -- Update book ratings and rating count
    UPDATE Books
    SET
        RatingCount = RatingCount + 1,
        Rating = CAST(((Rating * RatingCount + @Rating) / (RatingCount + 1.0)) AS DECIMAL(2, 1))
    WHERE BookId = @BookId;

    COMMIT TRANSACTION;

    -- Return the inserted feedback
    SELECT *
    FROM Feedback
    WHERE FeedbackId = @FeedbackId;
END;

exec usp_AddFeedback 2,6,3,'osm'

select * from Orders
select * from books

--************************************************************************************************************************************
														-- update feedback


CREATE or alter PROCEDURE usp_UpdateFeedback
    @FeedbackId INT,
    @UserId INT,
    @Review NVARCHAR(MAX),
    @Rating INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate the rating
    IF @Rating < 1 OR @Rating > 5
    BEGIN
        RAISERROR('Rating must be between 1 and 5.', 16, 1);
        RETURN;
    END

    -- Check if the feedback exists and belongs to the user
    IF NOT EXISTS (SELECT 1 FROM Feedback WHERE FeedbackId = @FeedbackId AND UserId = @UserId)
    BEGIN
        RAISERROR('Feedback does not exist or does not belong to the user.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    DECLARE @OldRating INT, @BookId INT;

    -- Get the old rating and book ID for recalculating the book rating
    SELECT @OldRating = Rating, @BookId = BookId
    FROM Feedback
    WHERE FeedbackId = @FeedbackId;

    -- Update the feedback
    UPDATE Feedback
    SET
        Rating = @Rating,
        Review = @Review,
        UpdatedAt = GETDATE()
    WHERE FeedbackId = @FeedbackId;

    -- Recalculate the book rating
    DECLARE @TotalRatings INT, @NewRatingSum DECIMAL(10, 1);

    -- Calculate the new sum of ratings
    SELECT
        @TotalRatings = RatingCount,
        @NewRatingSum = (Rating * RatingCount - @OldRating + @Rating)
    FROM Books
    WHERE BookId = @BookId;

    -- Update the book rating
    UPDATE Books
    SET
        Rating = CAST(@NewRatingSum / @TotalRatings AS DECIMAL(2, 1))
    WHERE BookId = @BookId;

    COMMIT TRANSACTION;

    -- Return the updated feedback
    SELECT *
    FROM Feedback
    WHERE FeedbackId = @FeedbackId;
END;

exec usp_UpdateFeedback 1,2,'good book',4

select * from Feedback

select * from books

--***********************************************************************************************************************************
															-- remove feedback

CREATE or alter PROCEDURE usp_RemoveFeedback
    @FeedbackId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the feedback exists and belongs to the user
    IF NOT EXISTS (SELECT 1 FROM Feedback WHERE FeedbackId = @FeedbackId AND UserId = @UserId)
    BEGIN
        RAISERROR('Feedback does not exist or does not belong to the user.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    DECLARE @DeletedRating INT, @BookId INT;

    -- Get the rating to be deleted and the corresponding book ID
    SELECT @DeletedRating = Rating, @BookId = BookId
    FROM Feedback
    WHERE FeedbackId = @FeedbackId;

    -- Delete the feedback
    DELETE FROM Feedback
    WHERE FeedbackId = @FeedbackId;

    -- Recalculate the book rating after deleting the feedback
    DECLARE @TotalRatings INT, @NewRatingSum DECIMAL(10, 1);

    -- Calculate the new sum of ratings
    SELECT
        @TotalRatings = RatingCount,
        @NewRatingSum = (Rating * RatingCount - @DeletedRating)
    FROM Books
    WHERE BookId = @BookId;

    -- Update the book rating
    IF @TotalRatings > 1
    BEGIN
        UPDATE Books
        SET
            Rating = CAST(@NewRatingSum / (@TotalRatings - 1) AS DECIMAL(2, 1))
        WHERE BookId = @BookId;
    END
    ELSE
    BEGIN
        -- If no more ratings, set rating and rating count to default values
        UPDATE Books
        SET
            Rating = 0,
            RatingCount = 0
        WHERE BookId = @BookId;
    END

    COMMIT TRANSACTION;

    -- Return success message or confirmation if needed
    SELECT 'Feedback deleted successfully.' AS Message;
END;

exec usp_RemoveFeedback 1,2

--**********************************************************************************************************************************
														-- view all feedbacks

CREATE or alter proc usp_ViewAllFeedbacks
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Select all feedbacks
        SELECT
           *
        FROM
            Feedback
        ORDER BY
            CreatedAt DESC;  -- Optionally order by creation date descending
    END TRY
    BEGIN CATCH
        -- Handle any errors that occur
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Raise the error with the message, severity, and state
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;


exec usp_ViewAllFeedbacks

--***********************************************************************************************************************************
														-- view feedback by id 
CREATE or alter PROCEDURE usp_ViewFeedbacksByBookId
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate that the BookId exists
        IF NOT EXISTS (SELECT 1 FROM Books WHERE BookId = @BookId)
        BEGIN
            RAISERROR('Book does not exist.', 16, 1);
            RETURN;
        END

        -- Select all feedbacks for the specified book
        SELECT
            FeedbackId,
            UserId,
            (
                SELECT fullname 
                FROM User_profile 
                WHERE UserId = f.UserId
            ) AS UserName,
            BookId,
            Rating,
            Review,
            CreatedAt,
            UpdatedAt
        FROM
            Feedback f
        WHERE
            BookId = @BookId
        ORDER BY
            CreatedAt DESC;  -- Optionally order by creation date descending
    END TRY
    BEGIN CATCH
        -- Handle any errors that occur
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Raise the error with the message, severity, and state
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

exec usp_ViewFeedbacksByBookId 1

--***********************************************************************************************************************************
															


