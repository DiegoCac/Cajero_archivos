CREATE DATABASE CajeroATM;
GO
USE CajeroATM;
GO

INSERT INTO Usuarios (Nombre, NumeroTarjeta, PIN, TokenRFID, SaldoActual, LimiteDiario, EsAdmin)
VALUES ('Karyn Prueba', '1234567890123456', '1234', 'Hola desde Arduino', 1000.00, 500.00, 0);

-- Usuarios (Administrador y Clientes)
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    NumeroTarjeta VARCHAR(16) UNIQUE NOT NULL, -- 16 dígitos
    PIN VARCHAR(4) NOT NULL,                    -- 4 dígitos
    TokenRFID VARCHAR(50) NULL,                -- Guardará el UID del Arduino
    SaldoActual DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    LimiteDiario DECIMAL(10,2) NOT NULL,        -- Límite de retiro por día
    EsAdmin BIT DEFAULT 0,                     -- 1 = Admin, 0 = Usuario común[cite: 1]
    UltimoAcceso DATETIME NULL,
    CambioPIN BIT DEFAULT 0                    -- Para métricas del Admin[cite: 1]
);

-- Denominaciones del Cajero (Caja de billetes)
CREATE TABLE BilletesCajero (
    Denominacion INT PRIMARY KEY,               -- 200, 100, 50, 20, 10, 5, 1[cite: 1]
    Cantidad INT NOT NULL DEFAULT 0
);

-- Transacciones (Depósitos y Retiros)[cite: 1]
CREATE TABLE Transacciones (
    TransaccionID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Tipo VARCHAR(10) CHECK (Tipo IN ('RETIRO', 'DEPOSITO')), --[cite: 1]
    Monto DECIMAL(10,2) NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE()
);