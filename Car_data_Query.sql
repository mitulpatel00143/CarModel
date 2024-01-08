
CREATE or ALTER PROCEDURE [dbo].[stp_car_GetCarCount]
 AS
BEGIN
	SET NOCOUNT ON;

    SELECT count(1) AS CarCount FROM [dbo].[CarModel] 
END
GO


Create or Alter PROCEDURE [dbo].[stp_car_GetAllCars]
	@SearchTerm NVARCHAR(255) = NULL,
    @SortColumn NVARCHAR(50) = 'DateOfManufacturing',
    @SortDirection NVARCHAR(4) = 'ASC',
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    SELECT Id, Brand, Class, ModelName, ModelCode, Description
			, Feature, Price, DateOfManufacturing
			,CASE WHEN IsActive=1 THEN 'True'
					ELSE 'False' END AS IsActive
			, CarImage
		FROM [dbo].[CarModel]
    
    WHERE (@SearchTerm IS NULL OR ModelName LIKE '%' + @SearchTerm + '%' OR ModelCode LIKE '%' + @SearchTerm + '%')
    ORDER BY
        CASE WHEN @SortDirection = 'ASC' THEN
            CASE WHEN @SortColumn = 'DateOfManufacturing' THEN DateOfManufacturing
				WHEN @SortColumn = 'Brand' THEN Brand
				WHEN @SortColumn = 'ModelName' THEN ModelName
			END
        END ASC,
        CASE WHEN @SortDirection = 'DESC' THEN
            CASE WHEN @SortColumn = 'DateOfManufacturing' THEN DateOfManufacturing
				WHEN @SortColumn = 'Brand' THEN Brand
				WHEN @SortColumn = 'ModelName' THEN ModelName
			END
        END DESC
    OFFSET @StartRow - 1 ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO


create or alter proc [dbo].[stp_car_GetCarById]
(
	@Id int
)
as
begin
Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.CarModel with (NOLOCK)
			where Id = @Id)

	If(@RowCount > 0)
		Begin
			SELECT Id, Brand, Class, ModelName, ModelCode, Description
					, Feature, Price, DateOfManufacturing
					, CASE WHEN IsActive=1 THEN 'True'
								ELSE 'False' END AS IsActive
					, CarImage
				FROM [dbo].[CarModel]
					where Id = @Id
		End

end
GO


CREATE or ALTER PROCEDURE [dbo].[stp_car_InsertCar]
	@Brand VARCHAR(50),
	@Class VARCHAR(50),
	@ModelName VARCHAR(50),
	@ModelCode VARCHAR(50),
	@Description VARCHAR(MAX),
	@Feature VARCHAR(MAX),
	@Price Decimal(14,2),
	@DateOfManufacturing DATETIME,
	@IsActive BIT,
	@CarImage VARCHAR(MAX)
AS
BEGIN

		SET NOCOUNT ON;

		INSERT INTO [dbo].[CarModel]
			   ( Brand, Class, ModelName, ModelCode, Description, Feature, Price,
					DateOfManufacturing, IsActive, CarImage)
		 VALUES
			   (@Brand, @Class, @ModelName, @ModelCode, @Description, @Feature, @Price,
					@DateOfManufacturing, @IsActive, @CarImage)

END
GO


CREATE or ALTER PROCEDURE [dbo].[stp_car_UpdateCar]
	@Id Int,
	@Brand VARCHAR(50),
	@Class VARCHAR(50),
	@ModelName VARCHAR(50),
	@ModelCode VARCHAR(50),
	@Description VARCHAR(MAX),
	@Feature VARCHAR(MAX),
	@Price Decimal(14,2),
	@DateOfManufacturing DATETIME,
	@IsActive BIT,
	@CarImage VARCHAR(MAX)
AS
BEGIN
	Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.CarModel with (NOLOCK)
						where Id = @Id)

	If(@RowCount > 0)
		Begin
				
			SET NOCOUNT ON;

			UPDATE [dbo].[CarModel]
				SET		Brand =@Brand ,
						Class = @Class ,
						ModelName = @ModelName ,
						ModelCode = @ModelCode ,
						Description = @Description ,
						Feature = @Feature ,
						Price = @Price ,
						DateOfManufacturing = @DateOfManufacturing ,
						IsActive = @IsActive ,
						CarImage = @CarImage
				WHERE  Id=@Id
		End

END
GO


CREATE or ALTER PROCEDURE  [dbo].[stp_car_DeleteCar]
	@Id Int
AS
BEGIN	
	Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.CarModel with (NOLOCK)
						where Id = @Id)

	If(@RowCount > 0)
		Begin
			SET NOCOUNT ON;
	
			Delete from [dbo].[CarModel]
					WHERE [Id] = @Id

		End
	
END
GO




