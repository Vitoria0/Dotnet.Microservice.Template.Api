# Microservice Template - .NET Core

Template base de projeto .NET Core seguindo arquitetura DDD (Domain-Driven Design) e Clean Architecture, preparado para microserviços escaláveis.

## Tecnologias

- **.NET 8.0**
- **Entity Framework Core** - ORM para acesso a dados
- **MediatR** - Implementação do padrão CQRS
- **FluentValidation** - Validação de comandos e queries
- **Serilog** - Logging estruturado
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Framework de testes
- **Moq** - Framework de mocking para testes
- **Docker** - Containerização

## Estrutura do Projeto

```
MicroserviceTemplate/
├── src/
│   ├── MicroserviceTemplate.API/          # Camada de apresentação (Controllers, Program.cs)
│   ├── MicroserviceTemplate.Application/ # Camada de aplicação (Commands, Queries, DTOs)
│   ├── MicroserviceTemplate.Domain/      # Camada de domínio (Entities, Interfaces)
│   └── MicroserviceTemplate.Infrastructure/ # Camada de infraestrutura (EF Core, Repositories)
├── tests/
│   └── MicroserviceTemplate.Tests/       # Testes unitários
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Arquitetura

O projeto segue os princípios de **DDD** e **Clean Architecture**:

- **Domain**: Entidades de negócio e interfaces de repositórios
- **Application**: Casos de uso, comandos, queries e DTOs
- **Infrastructure**: Implementações de repositórios, EF Core, UnitOfWork
- **API**: Controllers, configuração da aplicação

### Padrões Implementados

- **CQRS** (Command Query Responsibility Segregation) via MediatR
- **Repository Pattern**
- **Unit of Work Pattern**
- **Dependency Injection**
- **Validation Pipeline** com FluentValidation

## Pré-requisitos

- .NET 8.0 SDK
- Docker Desktop (para executar com Docker)
- SQL Server (ou usar o container Docker)

## Como Executar

### Opção 1: Executar com Docker Compose

```bash
docker-compose up -d
```

Isso irá:

- Subir um container SQL Server
- Construir e executar a API
- A API estará disponível em `http://localhost:5000`
- Swagger UI em `http://localhost:5000/swagger`

### Opção 2: Executar Localmente

1. Configure a connection string no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MicroserviceTemplateDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

2. Execute as migrations (se necessário):

```bash
cd src/MicroserviceTemplate.API
dotnet ef migrations add InitialCreate --project ../MicroserviceTemplate.Infrastructure
dotnet ef database update --project ../MicroserviceTemplate.Infrastructure
```

3. Execute a aplicação:

```bash
cd src/MicroserviceTemplate.API
dotnet run
```

## 📝 Exemplo de CRUD - Products

O projeto inclui um exemplo completo de CRUD para a entidade `Product`:

### Endpoints

- `GET /api/products` - Lista todos os produtos
- `GET /api/products/{id}` - Obtém um produto por ID
- `POST /api/products` - Cria um novo produto
- `PUT /api/products/{id}` - Atualiza um produto
- `DELETE /api/products/{id}` - Remove um produto (soft delete)

### Exemplo de Request (POST)

```json
{
  "name": "Notebook",
  "description": "Notebook Dell Inspiron",
  "price": 3500.0,
  "stock": 10
}
```

## Testes

Execute os testes com:

```bash
dotnet test
```

Os testes incluem:

- Testes de handlers de comandos
- Testes de validadores
- Testes de queries

## Docker

### Build da imagem

```bash
docker build -t microservice-template-api .
```

### Executar container

```bash
docker run -p 5000:80 microservice-template-api
```

## Configurações

### Logging

O Serilog está configurado para:

- Console output
- File output (logs/log-.txt)
- Rolling interval diário

### Database

O projeto usa SQL Server. A connection string pode ser configurada via:

- `appsettings.json`
- Variáveis de ambiente
- Docker Compose

## Próximos Passos

Para expandir este template:

1. Adicione novas entidades no `Domain`
2. Crie Commands/Queries no `Application`
3. Implemente repositórios no `Infrastructure`
4. Adicione controllers na `API`
5. Crie testes unitários

## Contribuindo

Este é um template base. Sinta-se livre para adaptar conforme suas necessidades.
