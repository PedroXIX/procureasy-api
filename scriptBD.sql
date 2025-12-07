-- Criar o banco de dados
CREATE DATABASE bdprocureasy;
GO
USE bdprocureasy;
GO

-- Tabela USUÁRIOS
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Senha VARCHAR(255) NOT NULL,
    Cnpj VARCHAR(18),
    Cpf VARCHAR(14),
    TipoUsuario VARCHAR(20) NOT NULL CHECK (TipoUsuario IN ('CONSUMIDOR', 'FORNECEDOR', 'ADMINISTRADOR')),
    Ativo BIT NOT NULL DEFAULT 1,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Usuario_Email UNIQUE (Email),
    CONSTRAINT CK_Usuario_Identificacao CHECK (
        (TipoUsuario = 'CONSUMIDOR' AND Cpf IS NOT NULL) OR
        (TipoUsuario = 'FORNECEDOR' AND Cnpj IS NOT NULL) OR
        (TipoUsuario = 'ADMINISTRADOR')
    )
);
GO

-- Tabela PRODUTOS
CREATE TABLE Produtos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Quantidade INT NOT NULL CHECK (Quantidade >= 0),
    Valor DECIMAL(10,2) NOT NULL CHECK (Valor >= 0),
    Descricao VARCHAR(500) NOT NULL,
    Area VARCHAR(20) NOT NULL CHECK (Area IN ('MANUTENCAO', 'REPARO', 'OPERACAO')),
    Ativo BIT NOT NULL DEFAULT 1,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    DataAtualizacao DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Tabela LEILOES
CREATE TABLE Leiloes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Descricao VARCHAR(500) NOT NULL,
    PrecoInicial DECIMAL(10,2) NOT NULL CHECK (PrecoInicial >= 0),
    PrecoFinal DECIMAL(10,2) CHECK (PrecoFinal IS NULL OR PrecoFinal >= 0),
    DataInicio DATETIME NOT NULL,
    DataTermino DATETIME NOT NULL,
    DataEntrega DATETIME NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'ABERTO' CHECK (Status IN ('ABERTO', 'ENCERRADO', 'CANCELADO')),
    ProdutoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    DataAtualizacao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Leilao_Produto FOREIGN KEY (ProdutoId) REFERENCES Produtos (Id),
    CONSTRAINT FK_Leilao_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id),
    CONSTRAINT CK_Leilao_Datas CHECK (DataInicio < DataTermino AND DataTermino <= DataEntrega)
);
GO

-- Tabela LEILAOUSUARIOS (Tabela intermediária para associar múltiplos usuários a um leilão)
CREATE TABLE LeilaoUsuarios (
    LeilaoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    PRIMARY KEY (LeilaoId, UsuarioId),
    CONSTRAINT FK_LeilaoUsuarios_Leilao FOREIGN KEY (LeilaoId) REFERENCES Leiloes (Id),
    CONSTRAINT FK_LeilaoUsuarios_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id)
);
GO

-- Tabela LANCES
CREATE TABLE Lances (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Valor DECIMAL(10,2) NOT NULL CHECK (Valor > 0),
    Vencedor BIT NOT NULL DEFAULT 0,
    Observacao VARCHAR(255),
    UsuarioId INT NOT NULL,
    LeilaoId INT NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Lance_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id),
    CONSTRAINT FK_Lance_Leilao FOREIGN KEY (LeilaoId) REFERENCES Leiloes (Id)
);
GO

-- Índices
CREATE INDEX IX_Usuario_Email ON Usuarios (Email);
CREATE INDEX IX_Usuario_TipoUsuario ON Usuarios (TipoUsuario);
CREATE INDEX IX_Leilao_Status ON Leiloes (Status);
CREATE INDEX IX_Leilao_DataTermino ON Leiloes (DataTermino);
CREATE INDEX IX_Lance_LeilaoId ON Lances (LeilaoId);
CREATE INDEX IX_Lance_UsuarioId ON Lances (UsuarioId);
GO

-- Comando para limpar o banco
DROP DATABASE bdprocureasy;
