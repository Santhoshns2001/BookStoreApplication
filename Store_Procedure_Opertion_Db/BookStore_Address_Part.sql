													--Address Entity
create table Addresses(
AddressId int primary key identity,
UserId int foreign key references user_profile(UserId),
FullName nvarchar(100) not null,
Mobile bigint not null,
Address nvarchar(max) not null,
City nvarchar(100) not null,
State nvarchar(100) not null,
Type nvarchar(50) not null,
constraint chk_addressmobile check(Mobile >=6000000000 and Mobile<= 9999999999),
constraint chk_addresstype check(Type in ('Home', 'Work', 'Other'))
);

select * from addresses

--************************************************************************************************************************************
															--Add Address


CREATE OR ALTER PROCEDURE usp_AddAddress
(
    @UserId INT,
    @FullName NVARCHAR(100),
    @Mobile BIGINT,
    @Address NVARCHAR(MAX),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @Type NVARCHAR(50)
)
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validate UserId
        IF @UserId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('UserId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Validate FullName
        IF @FullName IS NULL OR LTRIM(RTRIM(@FullName)) = ''
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('FullName cannot be empty.', 16, 1);
            RETURN;
        END
        -- Validate Address
        IF @Address IS NULL OR LTRIM(RTRIM(@Address)) = ''
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Address cannot be empty.', 16, 1);
            RETURN;
        END

        -- Validate City
        IF @City IS NULL OR LTRIM(RTRIM(@City)) = ''
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('City cannot be empty.', 16, 1);
            RETURN;
        END

        -- Validate State
        IF @State IS NULL OR LTRIM(RTRIM(@State)) = ''
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('State cannot be empty.', 16, 1);
            RETURN;
        END

        -- Validate Type
        IF @Type NOT IN ('Home', 'Work', 'Other')
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Type must be either "Home", "Work", or "Other".', 16, 1);
            RETURN;
        END

        -- Insert Address into the database
        INSERT INTO Addresses (UserId, FullName, Mobile, Address, City, State, Type)
        VALUES (@UserId, @FullName, @Mobile, @Address, @City, @State, @Type);

        -- Commit the transaction
        COMMIT TRANSACTION;
        PRINT 'Address added successfully.';
		declare @AddressId int=SCOPE_IDENTITY();
		select * from Addresses where AddressId=@AddressId;
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


exec usp_AddAddress 12,'santhosh kumar',9110894393,'hsr layout','bengaluru','karnataka','home'

select * from Addresses

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
														--Get All Addresses 
CREATE OR ALTER PROCEDURE usp_GetAllAddresses
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Select all addresses from the Addresses table
        SELECT 
            AddressId,
            UserId,
            FullName,
            Mobile,
            Address,
            City,
            State,
            Type
        FROM Addresses;
    END TRY
    BEGIN CATCH
        -- Return the error information in case of an error
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


exec usp_GetAllAddresses

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
															--Get Address by User id


CREATE OR ALTER PROCEDURE usp_GetAddressesByUserId
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

        -- Select addresses from the Addresses table where UserId matches the input parameter
        SELECT 
            AddressId,
            UserId,
            FullName,
            Mobile,
            Address,
            City,
            State,
            Type
        FROM Addresses
        WHERE UserId = @UserId;

        -- Check if no records are found
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('No addresses found for the provided UserId.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        -- Return the error information in case of an error
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

exec usp_GetAddressesByUserId 1

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
														--get address by user id and address id


CREATE OR ALTER PROCEDURE usp_GetAddressByUserIdAndAddressId
(
    @UserId INT,
    @AddressId INT
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

        -- Validate AddressId
        IF @AddressId IS NULL
        BEGIN
            RAISERROR('AddressId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Select address from the Addresses table where UserId and AddressId match the input parameters
        SELECT 
            AddressId,
            UserId,
            FullName,
            Mobile,
            Address,
            City,
            State,
            Type
        FROM Addresses
        WHERE UserId = @UserId AND AddressId = @AddressId;

        -- Check if no records are found
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('No address found for the provided UserId and AddressId.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        -- Return the error information in case of an error
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

exec usp_GetAddressByUserIdAndAddressId 1,2

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
															--update address
CREATE OR ALTER PROCEDURE usp_UpdateAddress
(
    @UserId INT,
    @AddressId INT,
    @FullName NVARCHAR(100),
    @Mobile BIGINT,
    @Address NVARCHAR(MAX),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @Type NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validate all inputs at once
        IF @UserId IS NULL OR
           @AddressId IS NULL OR
           @FullName IS NULL OR LTRIM(RTRIM(@FullName)) = '' OR
           @Mobile IS NULL OR @Mobile < 6000000000 OR @Mobile > 9999999999 OR
           @Address IS NULL OR LTRIM(RTRIM(@Address)) = '' OR
           @City IS NULL OR LTRIM(RTRIM(@City)) = '' OR
           @State IS NULL OR LTRIM(RTRIM(@State)) = '' OR
           @Type NOT IN ('Home', 'Work', 'Other')
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Invalid input parameters. Please check that all fields are provided and valid.', 16, 1);
            RETURN;
        END

        -- Update the address in the database
        UPDATE Addresses
        SET 
            FullName = @FullName,
            Mobile = @Mobile,
            Address = @Address,
            City = @City,
            State = @State,
            Type = @Type
        WHERE 
            UserId = @UserId AND 
            AddressId = @AddressId;

        -- Check if the update affected any rows
        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('No address found for the provided UserId and AddressId.', 16, 1);
            RETURN;
        END

        -- Commit the transaction
        COMMIT TRANSACTION;

        -- Return the updated address
        SELECT *
        FROM Addresses
        WHERE 
            UserId = @UserId AND 
            AddressId = @AddressId;
        
        PRINT 'Address updated successfully.';
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

exec usp_UpdateAddress 1,2,'santhosh ns',9110894394,'basavanagudi','bengaluru','karnataka','home'

	select * from Addresses

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
														-- delete address

CREATE OR ALTER PROCEDURE usp_DeleteAddress
(
    @UserId INT,
    @AddressId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validate UserId and AddressId
        IF @UserId IS NULL OR @AddressId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('UserId and AddressId cannot be NULL.', 16, 1);
            RETURN;
        END

        -- Delete the address from the database
        DELETE FROM Addresses
        WHERE UserId = @UserId AND AddressId = @AddressId;

        -- Check if the delete affected any rows
        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('No address found for the provided UserId and AddressId.', 16, 1);
            RETURN;
        END

        -- Commit the transaction
        COMMIT TRANSACTION;

        PRINT 'Address deleted successfully.';
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

exec usp_DeleteAddress 1,2
select * from Addresses

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

select * from User_Profile