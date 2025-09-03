# EduPlatform Backend

Backend do sistema LMS (Learning Management System) desenvolvido em ASP.NET Core 8 com Entity Framework Core.

## Estrutura do Projeto

```
backend/
├── EduPlatform.API/           # Projeto principal da API
├── EduPlatform.Core/          # Entidades e interfaces
├── EduPlatform.Infrastructure/ # Contexto do banco e repositórios
├── EduPlatform.Application/   # Casos de uso e DTOs
├── EduPlatform.Tests/         # Testes unitários
├── database_schema.sql        # Script completo do banco de dados
└── README.md                  # Documentação
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

### Opção 1: Usando o Script SQL (Recomendado)

1. **Execute o script SQL completo:**
   ```bash
   # Abra o SQL Server Management Studio ou Azure Data Studio
   # Execute o arquivo: backend/database_schema.sql
   ```

2. **Configure a string de conexão no `appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=EduPlatform;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

### Opção 2: Usando Entity Framework Migrations

1. **Configure a string de conexão no `appsettings.json`**
2. **Crie as migrações:**
   ```bash
   cd EduPlatform.API
   dotnet ef migrations add InitialCreate
   ```
3. **Aplique as migrações:**
   ```bash
   dotnet ef database update
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

## Estrutura do Banco de Dados

### 📊 **Tabelas Principais**

| Tabela | Descrição | Relacionamentos |
|--------|-----------|-----------------|
| **Users** | Usuários (alunos, professores, admins) | Matrículas, Cursos (professor), Progresso |
| **Categories** | Categorias de cursos | Cursos |
| **Courses** | Cursos | Categoria, Professor, Matrículas, Módulos |
| **Enrollments** | Matrículas de alunos | Usuário, Curso |
| **Modules** | Módulos/disciplinas | Curso, Aulas |
| **Lessons** | Aulas/conteúdo | Módulo, Progresso |
| **Assessments** | Avaliações/provas | Curso, Módulo, Questões |
| **Questions** | Questões das avaliações | Avaliação, Alternativas |
| **Alternatives** | Alternativas das questões | Questão |
| **Responses** | Respostas dos alunos | Questão, Usuário, Avaliação |
| **Progress** | Progresso dos alunos | Usuário, Aula |
| **Certificates** | Certificados | Usuário, Curso |
| **Forums** | Fóruns de discussão | Curso |
| **ForumTopics** | Tópicos do fórum | Fórum, Usuário |
| **ForumMessages** | Mensagens do fórum | Tópico, Usuário |
| **Notifications** | Notificações | Usuário |

### 🔗 **Principais Relacionamentos**

- **Usuário ↔ Curso** → através de **Matrículas**
- **Curso ↔ Módulo ↔ Aula** (hierarquia)
- **Curso ↔ Avaliação ↔ Questão ↔ Alternativa**
- **Usuário ↔ Progresso** (avalia o andamento do aluno em cada aula)
- **Usuário ↔ Certificado** (ao concluir curso)
- **Curso ↔ Fórum ↔ Tópicos ↔ Mensagens**

### 📈 **Índices de Performance**

O script inclui índices otimizados para:
- Consultas por email de usuário
- Filtros por status de curso
- Busca de matrículas por usuário/curso
- Consultas de progresso
- Notificações não lidas

### 🎯 **Views Úteis**

- `vw_CoursesWithDetails` - Cursos com detalhes do professor e categoria
- `vw_StudentProgress` - Progresso detalhado dos alunos

### ⚡ **Stored Procedures**

- `sp_EnrollStudent` - Matricular aluno em curso
- `sp_CalculateStudentProgress` - Calcular progresso do aluno

### 🔄 **Triggers**

- Atualização automática de timestamps
- Marcação automática de notificações como lidas

## Como Executar

1. **Clone o repositório e navegue até a pasta backend:**
   ```bash
   cd backend
   ```

2. **Restaure os pacotes:**
   ```bash
   dotnet restore
   ```

3. **Execute o script do banco de dados:**
   ```bash
   # Execute o arquivo database_schema.sql no SQL Server
   ```

4. **Execute a API:**
   ```bash
   cd EduPlatform.API
   dotnet run
   ```

5. **Acesse a documentação:**
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

## Dados Iniciais

O script SQL inclui dados iniciais:

### Categorias
- Tecnologia
- Gestão
- Marketing
- Design
- Saúde

### Usuários de Exemplo
- **Administrador**: admin@eduplatform.com
- **Professores**: joao.silva@eduplatform.com, maria.santos@eduplatform.com
- **Alunos**: pedro.costa@eduplatform.com, ana.oliveira@eduplatform.com

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

## Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT.
