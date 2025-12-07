# ProcurEasy API

## 📋 Sobre o Projeto

O **ProcurEasy** é uma plataforma de leilões reversos desenvolvida como Trabalho de Conclusão de Curso. O sistema permite que consumidores criem leilões para produtos e serviços, enquanto fornecedores competem oferecendo os melhores preços, promovendo transparência e economia nas transações comerciais.

### 🎯 Objetivos

- Facilitar a conexão entre consumidores e fornecedores através de leilões reversos
- Promover competitividade saudável e preços justos
- Garantir transparência em processos de aquisição
- Otimizar a gestão de produtos, leilões e lances

---

## 🚀 Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **BCrypt/PBKDF2** - Criptografia de senhas

### Arquitetura
- **RESTful API** - Padrão de comunicação
- **Repository Pattern** - Abstração de acesso a dados
- **Service Layer** - Lógica de negócio
- **DTOs (Data Transfer Objects)** - Transferência de dados

---

## 📁 Estrutura do Projeto

```
Procureasy.API/
├── Controllers/          # Endpoints da API
│   ├── AuthController.cs
│   ├── UsuariosController.cs
│   ├── ProdutosController.cs
│   ├── LeiloesController.cs
│   └── LancesController.cs
├── Services/            # Lógica de negócio
│   ├── Interfaces/
│   ├── UsuarioService.cs
│   ├── ProdutoService.cs
│   ├── LeilaoService.cs
│   ├── LanceService.cs
│   └── TokenService.cs
├── Data/               # Contexto do banco de dados
│   └── ProcurEasyContext.cs
├── Models/             # Entidades do domínio
│   ├── Usuario.cs
│   ├── Produto.cs
│   ├── Leilao.cs
│   ├── Lance.cs
│   ├── LeilaoUsuario.cs
│   └── Enums/
├── Dtos/              # Objetos de transferência
│   ├── Usuario/
│   ├── Produto/
│   ├── Leilao/
│   └── Lance/
└── Helpers/           # Utilitários
    ├── PasswordHelper.cs
    ├── EmailValidator.cs
    ├── PasswordValidator.cs
    └── DocumentNormalizer.cs
```

---

## 🗄️ Modelo de Dados

### Entidades Principais

#### Usuários
- Suporta três tipos: **CONSUMIDOR**, **FORNECEDOR**, **ADMINISTRADOR**
- Autenticação segura com senha criptografada
- Validação de CPF (consumidor) e CNPJ (fornecedor)

#### Produtos
- Catálogo com código único
- Classificação por área (MANUTENÇÃO, REPARO, OPERAÇÃO)
- Controle de estoque e valores

#### Leilões
- Status: ABERTO, ENCERRADO, CANCELADO
- Validação de datas (início, término, entrega)
- Vinculação de múltiplos fornecedores
- Preço inicial e final registrados

#### Lances
- Validação de valor máximo (não pode exceder preço inicial)
- Marcação de lance vencedor
- Rastreabilidade completa

---

## ⚙️ Configuração e Instalação

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (2019 ou superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### Passo a Passo

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/procureasy-api.git
cd procureasy-api
```

2. **Configure a string de conexão**

Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=bdprocureasy;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
  }
}
```

3. **Crie o banco de dados**

Execute o script SQL fornecido em `TCC.c` (seção scriptBD) no SQL Server Management Studio ou utilize:

```bash
dotnet ef database update
```

4. **Restaure as dependências**
```bash
dotnet restore
```

5. **Execute o projeto**
```bash
dotnet run
```

A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

---

## 🔐 Autenticação

### Login
```http
POST /api/auth
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "senha": "SenhaForte@123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Uso do Token

Inclua o token no header das requisições protegidas:
```http
Authorization: Bearer {seu-token-jwt}
```

---

## 📚 Endpoints da API

### Usuários

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/usuarios` | Lista todos os usuários | Sim |
| GET | `/api/usuarios/{id}` | Busca usuário por ID | Sim |
| POST | `/api/usuarios` | Cria novo usuário | Não |
| PUT | `/api/usuarios/{id}` | Atualiza usuário | Sim |
| DELETE | `/api/usuarios/{id}` | Remove usuário | Sim |
| PATCH | `/api/usuarios/{id}/ativar` | Ativa usuário | Sim |
| PATCH | `/api/usuarios/{id}/desativar` | Desativa usuário | Sim |

### Produtos

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/produtos` | Lista todos os produtos | Não |
| GET | `/api/produtos/{id}` | Busca produto por ID | Não |
| POST | `/api/produtos` | Cria novo produto | Sim |
| PUT | `/api/produtos/{codigoProduto}` | Atualiza produto | Sim |
| DELETE | `/api/produtos/{id}` | Inativa produto | Sim |

### Leilões

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/leiloes` | Lista todos os leilões | Sim |
| GET | `/api/leiloes/{id}` | Busca leilão por ID | Sim |
| POST | `/api/leiloes` | Cria novo leilão | Sim |
| PATCH | `/api/leiloes/{leilaoId}/fornecedores` | Adiciona fornecedores | Sim |
| GET | `/api/leiloes/fornecedor/{fornecedorId}` | Lista leilões do fornecedor | Sim |
| PATCH | `/api/leiloes/{id}/cancelar` | Cancela leilão | Sim |
| DELETE | `/api/leiloes/{id}` | Remove leilão | Sim |

### Lances

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/lances` | Lista todos os lances | Sim |
| GET | `/api/lances/{id}` | Busca lance por ID | Sim |
| POST | `/api/lances` | Cria novo lance | Sim |
| PATCH | `/api/lances/{id}/vencedor` | Define lance vencedor | Sim |

---

## 💡 Exemplos de Uso

### Criar Usuário Consumidor
```json
POST /api/usuarios
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "Senha@Forte123",
  "cpf": "123.456.789-00",
  "tipoUsuario": "CONSUMIDOR"
}
```

### Criar Leilão
```json
POST /api/leiloes
{
  "titulo": "Leilão de Ferramentas",
  "descricao": "Compra de ferramentas para manutenção",
  "precoInicial": 5000.00,
  "dataInicio": "2024-01-15T08:00:00",
  "dataTermino": "2024-01-20T18:00:00",
  "dataEntrega": "2024-01-30T18:00:00",
  "produtoId": 1,
  "usuarioId": 1
}
```

### Fazer Lance
```json
POST /api/lances
{
  "valor": 4500.00,
  "observacao": "Entrega em 15 dias",
  "usuarioId": 2,
  "leilaoId": 1
}
```

---

## 🔒 Segurança

### Validações Implementadas

- ✅ Senhas criptografadas com PBKDF2 + SHA256
- ✅ Validação de força de senha (mínimo 8 caracteres, maiúsculas, minúsculas, números e símbolos)
- ✅ Validação de formato de email
- ✅ Normalização de documentos (CPF/CNPJ)
- ✅ Proteção contra SQL Injection via Entity Framework
- ✅ Autenticação JWT com expiração de 30 minutos
- ✅ Validação de regras de negócio

### Regras de Lances

- Lance não pode exceder o preço inicial do leilão
- Apenas fornecedores vinculados podem dar lances
- Lance vencedor encerra automaticamente o leilão

---

## 📊 Diagrama de Relacionamentos

```
Usuarios (1) ──────── (N) Leiloes
    │                      │
    │                      │
    │(N)                (N)│
    │                      │
    └────── (N) Lances (N)─┘
                │
                │(N)
                │
            Produtos
```

---

## 🧪 Testes

Para executar os testes (quando implementados):

```bash
dotnet test
```

---

## 📝 Próximas Melhorias

- [ ] Implementação de notificações em tempo real (SignalR)
- [ ] Sistema de avaliações de fornecedores
- [ ] Dashboard administrativo
- [ ] Relatórios de leilões e estatísticas
- [ ] Integração com sistemas de pagamento
- [ ] API de upload de documentos
- [ ] Filtros avançados e busca por localização
- [ ] Testes unitários e de integração

---

## 📄 Licença

Este projeto foi desenvolvido como Trabalho de Conclusão de Curso e está disponível para fins educacionais.
