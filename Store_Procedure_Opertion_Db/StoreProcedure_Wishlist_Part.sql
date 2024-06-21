													--Wishlist Entity
create table Wishlist(
WishlistId int primary key identity,
UserId int foreign key references User_Profile(userId),
BookId int foreign key references Books(BookId),
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Image nvarchar(max) not null,
OriginalBookPrice int not null,
FinalBookPrice int not null,
);

drop table Wishlist

--***********************************************************************************************************************************
													-- adding wishlist
CREATE OR ALTER PROCEDURE usp_AddToWishlist
    @UserId INT,
    @BookId INT
AS
BEGIN
    BEGIN TRY
        -- Check if the book already exists in the wishlist for the given user
        IF NOT EXISTS (SELECT 1 FROM Wishlist WHERE UserId = @UserId AND BookId = @BookId)
        BEGIN
            -- Retrieve the book details from the Books table
            DECLARE @Title NVARCHAR(MAX);
            DECLARE @Author NVARCHAR(MAX);
            DECLARE @Image NVARCHAR(MAX);
            DECLARE @OriginalBookPrice INT;
            DECLARE @FinalBookPrice INT;

            SELECT 
                @Title = Title,
                @Author = Author,
                @Image = Image,
                @OriginalBookPrice = OriginalPrice,  
                @FinalBookPrice = Price 
            FROM Books
            WHERE BookId = @BookId;

            -- Insert a new row into the Wishlist table
            INSERT INTO Wishlist (UserId, BookId, Title, Author, Image, OriginalBookPrice, FinalBookPrice)
            VALUES (@UserId, @BookId, @Title, @Author, @Image, @OriginalBookPrice, @FinalBookPrice);

            -- Retrieve the inserted record's ID
            DECLARE @WishlistId INT = SCOPE_IDENTITY();

            -- Return the inserted record
            SELECT * FROM Wishlist WHERE WishlistId = @WishlistId;
        END
        ELSE
        BEGIN
            -- If the record already exists, return a message indicating it
            SELECT 'Record already exists in the wishlist' AS Message;
        END
    END TRY
    BEGIN CATCH
        -- Return the error information
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Rethrow the error to the caller
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO


select * from wishlist
exec usp_AddToWishlist 1,6

--************************************************************************************************************************************
												-- view wishlist by the user
create or alter proc usp_ViewWishlistsByUser
(@UserId int)
as
begin
if not exists (select 1 from wishlist where UserId=@UserId)
begin
raiserror('wishlist is not present for the provided user id',16,1);
end
else
begin
select * from Wishlist where UserId=@UserId
end
end

exec usp_ViewWishlistsByUser 1

--************************************************************************************************************************************
													--remove wishlist 
create or alter proc usp_RemoveBookFromWishlist
(@WishlistId int)
as
begin
if exists(select 1 from Wishlist where WishlistId=@WishlistId)
begin
delete from wishlist where WishlistId=@WishlistId;
end

else
begin
raiserror('wishlistid is not present to remove the book',16,1);
end
end

exec usp_RemoveBookFromWishlist 3

select * from wishlist


--************************************************************************************************************************************
														--view all wishlist
create or alter proc usp_ViewAllWishlist
as
begin
select * from Wishlist
end


--************************************************************************************************************************************