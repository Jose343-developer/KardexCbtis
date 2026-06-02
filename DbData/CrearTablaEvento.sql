USE CbtisKardex;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Evento')
BEGIN
    CREATE TABLE Evento (
        IdEvento INT IDENTITY(1,1) PRIMARY KEY,
        Titulo VARCHAR(100) NOT NULL,
        Descripcion VARCHAR(500) NULL,
        FechaInicio DATETIME NOT NULL,
        FechaFin DATETIME NOT NULL,
        Estatus VARCHAR(20) NOT NULL DEFAULT 'confirmed',
        Ubicacion VARCHAR(200) NULL
    );
END
GO
