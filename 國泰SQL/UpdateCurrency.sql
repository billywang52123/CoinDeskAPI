CREATE PROCEDURE UpdateCurrency
    @Id INT,
    @Currency NVARCHAR(50),
    @Symbol NVARCHAR(10),
    @Rate NVARCHAR(10),
    @Description NVARCHAR(100),
    @Rate_float FLOAT,
    @Chinese NVARCHAR(50),
    @Japanese NVARCHAR(50),
    @English NVARCHAR(50),
	@UpdateTime datetime
AS
BEGIN
    UPDATE CurrencyTable
    SET Currency = @Currency,
        Symbol = @Symbol,
        Rate = @Rate,
        Description = @Description,
        Rate_float = @Rate_float,
        Chinese = @Chinese,
        Japanese = @Japanese,
        English = @English,
		UpdateTime = @UpdateTime
    WHERE Id = @Id;
END