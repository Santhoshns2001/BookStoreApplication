													-- Cart Entity

create table Carts(
CartId int primary key identity,
UserId int foreign key references User_Profile(userId),
BookId int foreign key references Books(BookId),
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Image nvarchar(max) not null,
Quantity int default 1 check(Quantity between 1 and 5), 
OriginalBookPrice int not null,
FinalBookPrice int not null,
);

--drop table carts

select * from carts
--****************************************************************************************************************************
												--procedures
												--adding book to the cart
CREATE or alter PROCEDURE usp_AddBookToCart
    @UserId INT,
    @BookId INT
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Check if the book already exists in the cart for the given user
        IF EXISTS (SELECT 1 FROM Carts WHERE UserId = @UserId AND BookId = @BookId)
        BEGIN
            -- If the book is already in the cart, update the quantity by 1
            UPDATE Carts
            SET Quantity = Quantity + 1
            WHERE UserId = @UserId AND BookId = @BookId;
        END
        ELSE
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

            -- Insert a new row into the Carts table
            INSERT INTO Carts (UserId, BookId, Title, Author, Image, Quantity, OriginalBookPrice, FinalBookPrice)
            VALUES (@UserId, @BookId, @Title, @Author, @Image, 1, @OriginalBookPrice, @FinalBookPrice);

			declare @CartId int = SCOPE_IDENTITY();
			select * from Carts where CartId = @CartId;

        -- Commit the transaction
        COMMIT TRANSACTION;
		end
    END TRY
    BEGIN CATCH
        -- If there is an error, rollback the transaction
        ROLLBACK TRANSACTION;

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

exec usp_AddBookToCart 7,1

select * from Carts

select * from User_Profile
select * from Books
--*************************************************************************************************************************************
													--view cart by the user
create or alter proc usp_ViewCartsByUser
(@UserId int)
as
begin
if not exists (select 1 from Carts where UserId=@UserId)
begin
raiserror('cart is not present for the provided user id',16,1);
end
else
begin
select * from Carts where UserId=@UserId
end
end

exec usp_ViewCartsByUser 3

--*************************************************************************************************************************************
														-- update cart
create or alter proc usp_UpdateCart
(
@CartId int,
@Quantity int
)
as
begin
if not exists (select 1 from Carts where CartId=@CartId)
begin
raiserror('enterd cart id is incorrect',16,1)
end

else
begin

declare @BookId int;

select
@BookId=BookId
from Carts
where CartId=@CartId;

 DECLARE @OriginalBookPrice INT;
  DECLARE @FinalBookPrice INT;

  select 
  @OriginalBookPrice=OriginalPrice,
  @FinalBookPrice=Price
  from Books 
  where BookId=@BookId

		 update Carts set Quantity=@Quantity,
		 OriginalBookPrice=@OriginalBookPrice*@Quantity,
		 FinalBookPrice=@FinalBookPrice*@Quantity
		 where CartId=@CartId;

		 select * from Carts where CartId=@CartId
end
end

exec usp_UpdateCart 1, 3

select * from carts
select * from Books
--*************************************************************************************************************************************
													-- Romoving book from cart
create or alter proc usp_RemoveBookFromCart
(@CartId int)
as
begin
if exists(select 1 from Carts where CartId=@CartId)
begin
delete from Carts where CartId=@CartId;
end

else
begin
raiserror('cart id is not present to remove the cart',16,1);
end
end


--*************************************************************************************************************************************
														--fetch all carts
create or alter proc usp_ViewAllCarts
as
begin
select * from Carts
end

exec usp_ViewAllCarts

--*************************************************************************************************************************************
