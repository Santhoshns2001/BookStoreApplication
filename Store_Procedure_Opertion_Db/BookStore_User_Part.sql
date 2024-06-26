													--User Entity

create table User_Profile
(
userId int primary key identity,
fullname varchar(100) not null,
email varchar(50) not null unique,
password varchar(50) not null,
mobile bigint not null unique check(mobile >=6000000000 AND mobile <= 9999999999)
);



**************************************************************************************************************************
												--User register
create or alter proc usp_UserRegister(
@fullname varchar(100),
@email varchar(50),
@password varchar(50),
@mobile bigint
)
as
begin

insert into User_Profile(fullName,email,password,mobile) 
values (@fullname,@email,@password,@mobile);

declare @userid int=scope_identity();

select * from user_profile where userid=@userid
end


exec usp_UserRegister 'aishwarya','aishu123@gmail.com','aishu@123',9632587412



select * from User_Profile

****************************************************************************************************************************
														--login user

CREATE PROCEDURE usp_UserLogin
(
    @Email VARCHAR(50),
    @Password VARCHAR(50)
)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM user_profile WHERE email = @Email AND password = @Password)
        BEGIN
            RAISERROR('No email and password exists or matched', 16, 1);
        END
        ELSE
        BEGIN
            SELECT * FROM user_profile WHERE email = @Email AND password = @Password;
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;

exec  usp_UserLogin 'santhosh@gmail.com','santhu2001' 




select * from user_profile

****************************************************************************************************************************
												--check email
create proc usp_CheckEmail
(@Email varchar(50)
)
as

begin
begin try
if not exists(select 1 from User_profile where email=@Email)
begin
RAISERROR('email not found',16,1);
end
else
select * from user_profile where email=@Email;
end try
begin catch
throw;
end catch
end

exec usp_CheckEmail 'santhosh@gmail.com'

***************************************************************************************************************************
														--fetch by user id
create proc usp_FetchByUserId
(@UserId int )
as

begin
begin try
if not exists(select 1 from User_profile where userId=@UserId)
begin
RAISERROR('user id  not found ',16,1);
end
else
select * from user_profile where userId=@UserId;
end try
begin catch
throw;
end catch
end

exec usp_FetchByUserId 1

*****************************************************************************************************************************
													--fetch all users
create proc usp_FetchAllUsers
as
begin
select * from user_profile 
end


exec usp_FetchAllUsers

*****************************************************************************************************************************
												-- update user
create or alter proc usp_UpdateUser(
@userId int,
@fullname varchar(100),
@email varchar(50),
@password varchar(50),
@mobile bigint
)
as
begin
if not exists (select 1 from user_profile where userId=@userId)
begin
raiserror('provided user id not exists',16,1)
end
else
begin
update user_profile set fullname=@fullname, email=@email,password=@password,mobile=@mobile where userId=@userId 
select * from user_profile where userId=@userId
end
end

exec usp_UpdateUser 1,'santhosh ','santhosh@gmail.com','santhu@2001',9110894393

select * from user_profile


****************************************************************************************************************************
														--forgot password

create proc usp_ForgotPassword
(@email varchar(50))
as
begin
if not exists(select 1 from user_profile where email=@email )
begin
raiserror('email not found',16,1)
end
else
begin
select * from user_profile where email=@email
end
end

exec usp_ForgotPassword 'scott123@gmail.com'


****************************************************************************************************************************
														--reset password  

create proc usp_ResetPassword
(
@Email varchar(50),
@Password varchar(50)
)
as
begin
update User_Profile set password=@Password where email=@Email;
end

exec usp_ResetPassword 'scott123@gmail.com','Scott@112233'

select * from user_profile


****************************************************************************************************************************
****************************************************************************************************************************
																					
