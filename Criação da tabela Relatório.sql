CREATE TABLE Relatorio (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Descricao VARCHAR(255) NOT NULL,
    DataSolicitacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DataAplicacao DATE NOT NULL,
    SolicitanteID INT NOT NULL,
	QuantidadeTotalVacinas INT NOT NULL DEFAULT 0,
    FOREIGN KEY (SolicitanteID) REFERENCES Solicitante(ID)
);