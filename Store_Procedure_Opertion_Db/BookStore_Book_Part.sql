
													--Book Entity

create table Books(
BookId int primary key identity,
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Description nvarchar(max) not null,
Rating decimal(2,1) default 0,
RatingCount int default 0,
OriginalPrice int not null,
DiscountPercentage int not null,
Price as cast(OriginalPrice*(1-DiscountPercentage/100.0) as int) persisted not null, 
Quantity int not null,
Image nvarchar(max),
constraint chk_rating check(Rating between 0 and 5),
constraint chk_discount check(DiscountPercentage between 0 and 100),
constraint chk_price check(Price <= OriginalPrice),
)
select * from books

--*****************************************************************************************************************************
														--Add Book
alter PROCEDURE usp_AddBook
(
    @Title NVARCHAR(MAX),
    @Author NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @OriginalPrice INT,
    @DiscountPercentage INT,
    @Quantity INT,
    @Image NVARCHAR(MAX)
)
AS
BEGIN
    -- Declare variables
    DECLARE @NewBookID INT;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert the new book record
        INSERT INTO Books (Title, Author, Description, OriginalPrice, DiscountPercentage, Quantity, Image)
        VALUES (@Title, @Author, @Description, @OriginalPrice, @DiscountPercentage, @Quantity, @Image);

        -- Retrieve the ID of the newly inserted book
        SET @NewBookID = SCOPE_IDENTITY();

        -- Commit the transaction
        COMMIT TRANSACTION;

        -- Return the ID of the newly inserted book
        --SELECT @NewBookID AS NewBookID;
		select * from Books where BookId = @NewBookID

    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;

        -- Retrieve error information
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Raise the error to the caller
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
					
exec usp_AddBook 't1sd','a1','d1',150,20,25,'asdfgb'


--****************************************************************************************************************************
													--Fetch All books
create proc usp_GetAllBooks
as
begin
select * from Books
end

exec usp_GetAllBooks 

--****************************************************************************************************************************
														--Fetch by id 

create proc usp_GetByBookId
(@bookId int)
as
begin
if not exists (select 1 from Books where BookId=@bookId)
begin
raiserror('book id is incorrect',16,1);
end
else
begin
select * from Books where BookId=@bookId
end
end


exec usp_GetByBookId 5

--****************************************************************************************************************************
													-- Delete book by Id
create proc usp_DeleteBookById
(@bookId int)
as
begin
if not exists (select 1 from Books where BookId=@bookId)
begin
raiserror('book id is incorrect',16,1);
end
else
begin
delete from Books where BookId=@bookId
end
end

exec usp_DeleteBookById 3

select * from Books

--****************************************************************************************************************************
													--Update book

create or alter proc usp_UpdateBook
(
	@bookId int,
    @Title NVARCHAR(MAX),
    @Author NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @OriginalPrice INT,
    @DiscountPercentage INT,
    @Quantity INT,
    @Image NVARCHAR(MAX)
)
as
begin
update Books set Title=@Title,Author=@Author,Description=@Description,OriginalPrice=@OriginalPrice,
DiscountPercentage=@DiscountPercentage,Quantity=@Quantity,Image=@Image where BookId=@bookId

select * from books where BookId=@bookId
end

exec usp_UpdateBook 1,'title1','author1','desc1',299,15,100,'img1'

select * from Books

--*************************************************************************************************************************************
													-- fetch book by title or author


CREATE PROCEDURE usp_FetchByTitle_Author
    @Author NVARCHAR(max) = NULL,
    @Title NVARCHAR(max) = NULL
AS
BEGIN
    -- Declare variables to hold error information
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    -- Set NOCOUNT ON to prevent extra result sets from interfering with SELECT statements
    SET NOCOUNT ON;

    BEGIN TRY
        -- Select books by author or title, depending on the parameters provided
        SELECT 
           *
        FROM 
            Books
        WHERE
            (@Author IS NULL OR Author LIKE '%' + @Author + '%')
            AND (@Title IS NULL OR Title LIKE '%' + @Title + '%');
    END TRY
    BEGIN CATCH
        -- Capture error information
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Rethrow the error to the calling environment
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;


exec usp_FetchByTitle_Author '','t1'

select * from Books

--*************************************************************************************************************************************
														-- find by book id if not exists insert the book

		--2)Find the data using bookid, if it exst update the data else insert the new book record.

create or alter proc usp_FindByBookId
(
    @bookId int,
    @Title NVARCHAR(MAX),
    @Author NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @OriginalPrice INT,
    @DiscountPercentage INT,
    @Quantity INT,
    @Image NVARCHAR(MAX)
)
as
begin
begin try
  IF @bookId IS NULL OR @Title IS NULL or @Description is null or @OriginalPrice is null or @Quantity is null or @Image is null
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('values cannot be NULL.', 16, 1);
            RETURN;
        END
   if not exists (select 1 from Books where BookId=@bookId)
      begin
        INSERT INTO Books (Title, Author, Description, OriginalPrice, DiscountPercentage, Quantity, Image)
        VALUES (@Title, @Author, @Description, @OriginalPrice, @DiscountPercentage, @Quantity, @Image);

		declare @BookNo int=Scope_Identity();
		select * from Books where BookId=@BookNo;
      end
else 
begin
update Books set Title=@Title,Author=@Author,Description=@Description,OriginalPrice=@OriginalPrice,
DiscountPercentage=@DiscountPercentage,Quantity=@Quantity,Image=@Image where BookId=@bookId;

select * from books where BookId=@bookId

end
end try
begin catch
throw;
end catch
end

exec usp_FindByBookId 8,'title10','author10','disc10',250,25,20,'image10'

--*************************************************************************************************************************************
