CREATE TABLE Solicitante (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Nome VARCHAR(255) NOT NULL,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    DataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);