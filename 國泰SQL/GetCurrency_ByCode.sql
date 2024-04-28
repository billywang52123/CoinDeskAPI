CREATE PROCEDURE GetCurrency_ByCode
@Code INT
AS
BEGIN
	SELECT *
    FROM CurrencyTable
	WHERE Currency = @Code;
END