CREATE PROCEDURE AddCurrency
    @Currency NVARCHAR(50),
    @Symbol NVARCHAR(10),
    @Rate NVARCHAR(10),
    @Description NVARCHAR(100),
    @Rate_float FLOAT,
    @Chinese NVARCHAR(50),
    @Japanese NVARCHAR(50),
    @English NVARCHAR(50),
	@CreateDateTime Datetime,
	@UpdateTime Datetime
AS
BEGIN
    INSERT INTO CurrencyTable (Currency, Symbol, Rate, Description, Rate_float, Chinese, Japanese, English, CreateDateTime, UpdateTime)
    VALUES (@Currency, @Symbol, @Rate, @Description, @Rate_float, @Chinese, @Japanese, @English, @CreateDateTime, @UpdateTime);
END