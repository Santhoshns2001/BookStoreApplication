													--Order Entity
create table Orders(
    OrderId int primary key identity,
    UserId int foreign key references User_profile(UserId),
	AddressId int foreign key references Addresses(AddressId),
    BookId int foreign key references Books(BookId),
    Title nvarchar(max) not null,
    Author nvarchar(max) not null,
    Image nvarchar(max) not null,
    Quantity int check(Quantity >= 1),  
    TotalOriginalBookPrice int not null,
    TotalFinalBookPrice int not null,
    OrderDateTime datetime default getdate(),
    IsDeleted bit default 0
)

select * from orders

--*************************************************************************************************************************************
														-- Add or place order
CREATE OR ALTER PROCEDURE usp_PlaceOrder
(
    @UserId INT,
    @CartId INT,
    @AddressId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validate UserId, CartId, and AddressId
        IF @UserId IS NULL OR @CartId IS NULL OR @AddressId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('UserId, CartId, and AddressId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Get cart details
        DECLARE @BookId INT, @CartQuantity INT;
        SELECT @BookId = BookId, @CartQuantity = Quantity
        FROM Carts
        WHERE CartId = @CartId AND UserId = @UserId;
		
        -- Validate cart details
        IF @BookId IS NULL OR @CartQuantity IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Invalid CartId or UserId.', 16, 1);
            RETURN;
        END

        -- Get book details
        DECLARE @Title NVARCHAR(MAX),
		        @Author NVARCHAR(MAX),
		        @Image NVARCHAR(MAX),
		        @BookQuantity INT,
		        @OriginalBookPrice INT,
		        @TotalFinalBookPrice INT;

        SELECT @Title = Title, 
               @Author = Author, 
               @Image = Image, 
               @BookQuantity = Quantity,
               @OriginalBookPrice = OriginalPrice,
               @TotalFinalBookPrice = Price
        FROM Books
        WHERE BookId = @BookId;

        -- Validate book details
        IF @Title IS NULL OR @Author IS NULL OR @Image IS NULL OR @BookQuantity IS NULL OR @OriginalBookPrice IS NULL OR @TotalFinalBookPrice IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Invalid BookId.', 16, 1);
            RETURN;
        END

        -- Check if enough books are available
        IF @BookQuantity < @CartQuantity
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Not enough books available.', 16, 1);
            RETURN;
        END

        -- Insert into Orders table
        INSERT INTO Orders (UserId, AddressId, BookId, Title, Author, Image, Quantity, TotalOriginalBookPrice, TotalFinalBookPrice, OrderDateTime, IsDeleted)
        VALUES (@UserId, @AddressId, @BookId, @Title, @Author, @Image, @CartQuantity, @OriginalBookPrice * @CartQuantity, @TotalFinalBookPrice * @CartQuantity, GETDATE(), 0);

        -- Get the new OrderId
        DECLARE @OrderId INT = SCOPE_IDENTITY();

        -- Update book quantity in Books table
        UPDATE Books
        SET Quantity = Quantity - @CartQuantity
        WHERE BookId = @BookId;

        -- Remove the book from the cart
        DELETE FROM Carts
        WHERE CartId = @CartId AND UserId = @UserId;

        -- Commit the transaction
        COMMIT TRANSACTION;

        PRINT 'Order placed successfully.';

        -- Return the new order details
        SELECT * FROM Orders WHERE OrderId = @OrderId;

    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of error
        ROLLBACK TRANSACTION;
        
        -- Return the error information
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

select * from Addresses

exec usp_PlaceOrder 13,2,7
select * from carts

--*************************************************************************************************************************************
													-- view All orders
CREATE OR ALTER PROCEDURE usp_ViewAllOrders
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Select all orders directly from the Orders table
        SELECT 
            *
        FROM 
            Orders
        ORDER BY 
            OrderDateTime DESC;  -- Orders displayed by most recent first

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

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;


exec usp_ViewAllOrders
--*************************************************************************************************************************************
													-- view orders by user id
CREATE OR ALTER PROCEDURE usp_ViewOrdersByUserId
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate UserId
        IF @UserId IS NULL
        BEGIN
            RAISERROR('UserId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Select orders for the specified UserId
        SELECT 
           *
        FROM 
            Orders
        WHERE 
            UserId = @UserId
        ORDER BY 
            OrderDateTime DESC;  -- Orders displayed by most recent first

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

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

exec usp_ViewOrdersByUserId 2

--*************************************************************************************************************************************
													-- view orders based on order id 
CREATE OR ALTER PROCEDURE usp_ViewOrdersByOrderId
(
    @OrderId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate OrderId
        IF @OrderId IS NULL
        BEGIN
            RAISERROR('OrderId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Select order for the specified OrderId
        SELECT 
           *
        FROM 
            Orders
        WHERE 
            OrderId = @OrderId;
        
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

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

exec usp_ViewOrdersByOrderId 2

--*************************************************************************************************************************************
														-- cancel order
CREATE OR ALTER PROCEDURE usp_CancelOrder
(
    @UserId INT,
    @OrderId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validate UserId and OrderId
        IF @UserId IS NULL OR @OrderId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('UserId and OrderId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Get order details
        DECLARE @BookId INT, @Quantity INT;
        SELECT @BookId = BookId, @Quantity = Quantity
        FROM Orders
        WHERE OrderId = @OrderId AND UserId = @UserId;

        -- Validate order details
        IF @BookId IS NULL OR @Quantity IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Invalid OrderId or UserId.', 16, 1);
            RETURN;
        END

        -- Restore book quantity in Books table
        UPDATE Books
        SET Quantity = Quantity + @Quantity
        WHERE BookId = @BookId;

        -- Remove the order from Orders table
        DELETE FROM Orders
        WHERE OrderId = @OrderId AND UserId = @UserId;

        -- Check if the book is already in the cart
        IF EXISTS (SELECT 1 FROM Carts WHERE UserId = @UserId AND BookId = @BookId)
        BEGIN
            -- Update the quantity in the cart
            UPDATE Carts
            SET Quantity = Quantity + @Quantity
            WHERE UserId = @UserId AND BookId = @BookId;
        END
        ELSE
        BEGIN
		 DECLARE @Title NVARCHAR(MAX),
		        @Author NVARCHAR(MAX),
		        @Image NVARCHAR(MAX),
		        @OriginalBookPrice INT,
		        @TotalFinalBookPrice INT;

			 SELECT @Title = Title, 
               @Author = Author, 
               @Image = Image, 
               @OriginalBookPrice = OriginalPrice,
               @TotalFinalBookPrice = Price
        FROM Books
        WHERE BookId = @BookId;

            -- Insert the book into the cart
            INSERT INTO Carts (UserId, BookId,Title,Author,Image, Quantity,OriginalBookPrice,FinalBookPrice)
            VALUES (@UserId, @BookId,@Title,@Author,@Image, @Quantity,@OriginalBookPrice,@TotalFinalBookPrice);
        END

        -- Commit the transaction
        COMMIT TRANSACTION;

        PRINT 'Order cancelled successfully.';

    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of error
        ROLLBACK TRANSACTION;
        
        -- Return the error information
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;


select * from orders

exec usp_CancelOrder 1,2

select * from carts

select * from User_Profile

--************************************************************************************************************************************