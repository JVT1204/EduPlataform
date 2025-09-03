# EduPlatform Backend

Backend do sistema LMS (Learning Management System) desenvolvido em ASP.NET Core 8 com Entity Framework Core.

## Estrutura do Projeto

```
backend/
├── EduPlatform.API/           # Projeto principal da API
├── EduPlatform.Core/          # Entidades e interfaces
├── EduPlatform.Infrastructure/ # Contexto do banco e repositórios
├── EduPlatform.Application/   # Casos de uso e DTOs
└── EduPlatform.Tests/         # Testes unitários
```

## Tecnologias Utilizadas

- **ASP.NET Core 8**
- **Entity Framework Core**
- **SQL Server** (ou PostgreSQL)
- **C# 12**

## Pré-requisitos

- .NET 8 SDK
- SQL Server (ou PostgreSQL)
- Visual Studio 2022 ou VS Code

## Configuração do Banco de Dados

### SQL Server

1. Instale o SQL Server (pode ser SQL Server Express)
2. Configure a string de conexão no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EduPlatform;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### PostgreSQL (Alternativa)

1. Instale o PostgreSQL
2. Adicione o pacote: `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
3. Configure a string de conexão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EduPlatform;Username=seu_usuario;Password=sua_senha"
  }
}
```

## Como Executar

1. **Clone o repositório e navegue até a pasta backend:**
   ```bash
   cd backend
   ```

2. **Restaure os pacotes:**
   ```bash
   dotnet restore
   ```

3. **Crie as migrações do banco de dados:**
   ```bash
   cd EduPlatform.API
   dotnet ef migrations add InitialCreate
   ```

4. **Aplique as migrações:**
   ```bash
   dotnet ef database update
   ```

5. **Execute a API:**
   ```bash
   dotnet run
   ```

6. **Acesse a documentação da API:**
   - Swagger UI: `https://localhost:7001/swagger`
   - API: `https://localhost:7001/api`

## Endpoints Principais

### Usuários
- `GET /api/users` - Listar todos os usuários
- `GET /api/users/{id}` - Obter usuário por ID
- `GET /api/users/students` - Listar estudantes
- `GET /api/users/teachers` - Listar professores
- `POST /api/users` - Criar novo usuário
- `PUT /api/users/{id}` - Atualizar usuário
- `DELETE /api/users/{id}` - Deletar usuário

### Cursos
- `GET /api/courses` - Listar todos os cursos
- `GET /api/courses/{id}` - Obter curso por ID
- `POST /api/courses` - Criar novo curso
- `PUT /api/courses/{id}` - Atualizar curso
- `DELETE /api/courses/{id}` - Deletar curso

## Próximos Passos

1. **Autenticação e Autorização**
   - Implementar JWT
   - Adicionar roles e claims

2. **Validação**
   - Adicionar FluentValidation
   - Implementar validações customizadas

3. **Logging**
   - Configurar Serilog
   - Implementar logs estruturados

4. **Testes**
   - Testes unitários
   - Testes de integração
   - Testes de API

5. **Documentação**
   - Melhorar documentação da API
   - Adicionar exemplos de uso

6. **Segurança**
   - Hash de senhas com BCrypt
   - Rate limiting
   - CORS configurado adequadamente

## Estrutura de Entidades

### User
- Id, Name, Email, PasswordHash, Phone
- Role (Student, Teacher, Admin)
- Relacionamentos com cursos

### Course
- Id, Title, Description, ImageUrl
- Teacher (relacionamento)
- EnrolledStudents (relacionamento many-to-many)
- Modules (relacionamento one-to-many)

### Module
- Id, Title, Description, Order
- Course (relacionamento)
- Lessons (relacionamento one-to-many)

### Lesson
- Id, Title, Description, ContentUrl, Content
- Type (Video, Text, Quiz, Assignment)
- Module (relacionamento)

### Assignment
- Id, Title, Description, DueDate, MaxScore
- Course (relacionamento)
- Submissions (relacionamento one-to-many)

## Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT.


