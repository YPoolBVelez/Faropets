CREATE TABLE [dbo].[TPayments_clients] (
    [IdPayments]  INT             IDENTITY (1, 1) NOT NULL,
    [Debt]        DECIMAL (18, 2) NOT NULL,
    [Change]      DECIMAL (18, 2) NOT NULL,
    [Payment]     DECIMAL (18, 2) NOT NULL,
    [Date]        DATETIME2 (7)   NOT NULL,
    [CurrentDebt] DECIMAL (18, 2) NOT NULL,
    [Deadline]    DATETIME2 (7)) NOT NULL,
    [Ticket]      NVARCHAR (MAX)  NULL,
    [IdUser]      NVARCHAR (MAX)  NULL,
    [User]        NVARCHAR (MAX)  NULL,
    [IdCliente]   NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_TPayments_clients] PRIMARY KEY CLUSTERED ([IdPayments] ASC)
);

