CREATE PROCEDURE UpdateCurrency_ByCurrency
    @Currency NVARCHAR(50),
    @Symbol NVARCHAR(10),
    @Rate NVARCHAR(10),
    @Rate_float FLOAT,
	@UpdateTime Datetime
AS
BEGIN
    UPDATE CurrencyTable
    SET Symbol = @Symbol,
        Rate = @Rate,
        Rate_float = @Rate_float,
		UpdateTime = @UpdateTime
    WHERE Currency = @Currency;
END