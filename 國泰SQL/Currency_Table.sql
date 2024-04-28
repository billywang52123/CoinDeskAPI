CREATE TABLE [dbo].[CurrencyTable] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Currency]    NVARCHAR (50)  NOT NULL,
    [Symbol]      NVARCHAR (10)  NOT NULL,
    [Rate]        NVARCHAR (10)  NOT NULL,
    [Description] NVARCHAR (100) NOT NULL,
    [Rate_float]  FLOAT (53)     NOT NULL,
    [Chinese]     NVARCHAR (50)  NULL,
    [Japanese]    NVARCHAR (50)  NULL,
    [English]     NVARCHAR (50)  NULL,
    [CreateDateTime] DATETIME NULL, 
    [UpdateTime] DATETIME NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

