CREATE PROCEDURE DeleteCurrency
    @Id INT
AS
BEGIN
    DELETE FROM CurrencyTable WHERE Id = @Id;
END