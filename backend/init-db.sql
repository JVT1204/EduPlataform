-- Script de inicialização do banco para Docker
-- Este script será executado automaticamente quando o container SQL Server subir

-- Aguardar o SQL Server estar pronto
WAITFOR DELAY '00:00:10'

-- Verificar se o banco já existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EduPlatform')
BEGIN
    -- Criar banco de dados
    CREATE DATABASE EduPlatform;
    PRINT 'Banco de dados EduPlatform criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco de dados EduPlatform já existe.';
END

GO

USE EduPlatform;
GO

-- Verificar se as tabelas já existem
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
BEGIN
    -- Executar o script completo do banco
    PRINT 'Criando estrutura do banco de dados...';
    
    -- TABELAS PRINCIPAIS
    
    -- 1. USUÁRIOS
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        LastLogin DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Role INT NOT NULL DEFAULT 0 -- 0=Student, 1=Teacher, 2=Admin
    );

    -- 2. CATEGORIAS
    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );

    -- 3. CURSOS
    CREATE TABLE Courses (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        ImageUrl NVARCHAR(500) NULL,
        StartDate DATETIME2 NOT NULL,
        EndDate DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Status INT NOT NULL DEFAULT 0, -- 0=Draft, 1=Active, 2=Archived
        CategoryId INT NOT NULL,
        TeacherId INT NOT NULL,
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
        FOREIGN KEY (TeacherId) REFERENCES Users(Id)
    );

    -- 4. MATRÍCULAS
    CREATE TABLE Enrollments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EnrollmentDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Status INT NOT NULL DEFAULT 0, -- 0=Active, 1=Locked, 2=Completed
        CompletedAt DATETIME2 NULL,
        UserId INT NOT NULL,
        CourseId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id),
        FOREIGN KEY (CourseId) REFERENCES Courses(Id),
        UNIQUE(UserId, CourseId) -- Evita matrículas duplicadas
    );

    -- 5. MÓDULOS
    CREATE TABLE Modules (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        [Order] INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CourseId INT NOT NULL,
        FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
    );

    -- 6. AULAS
    CREATE TABLE Lessons (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        [Order] INT NOT NULL,
        Type INT NOT NULL DEFAULT 0, -- 0=Video, 1=PDF, 2=Quiz, 3=Link, 4=PracticalActivity, 5=Text
        FileUrl NVARCHAR(500) NULL,
        Content NVARCHAR(MAX) NULL,
        DurationMinutes INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        ModuleId INT NOT NULL,
        FOREIGN KEY (ModuleId) REFERENCES Modules(Id) ON DELETE CASCADE
    );

    -- 7. AVALIAÇÕES
    CREATE TABLE Assessments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        Type INT NOT NULL DEFAULT 0, -- 0=Quiz, 1=Exam, 2=Assignment
        AvailableDate DATETIME2 NOT NULL,
        DueDate DATETIME2 NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CourseId INT NOT NULL,
        ModuleId INT NULL,
        CreatedById INT NOT NULL,
        FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE,
        FOREIGN KEY (ModuleId) REFERENCES Modules(Id) ON DELETE SET NULL,
        FOREIGN KEY (CreatedById) REFERENCES Users(Id)
    );

    -- 8. QUESTÕES
    CREATE TABLE Questions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Statement NVARCHAR(1000) NOT NULL,
        Type INT NOT NULL DEFAULT 0, -- 0=MultipleChoice, 1=Essay, 2=TrueFalse
        Weight DECIMAL(5,2) NOT NULL DEFAULT 1.00,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        AssessmentId INT NOT NULL,
        FOREIGN KEY (AssessmentId) REFERENCES Assessments(Id) ON DELETE CASCADE
    );

    -- 9. ALTERNATIVAS
    CREATE TABLE Alternatives (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Text NVARCHAR(500) NOT NULL,
        IsCorrect BIT NOT NULL DEFAULT 0,
        [Order] INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        QuestionId INT NOT NULL,
        FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
    );

    -- 10. RESPOSTAS
    CREATE TABLE Responses (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TextResponse NVARCHAR(2000) NULL,
        AssignedGrade DECIMAL(5,2) NULL,
        Feedback NVARCHAR(1000) NULL,
        SubmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        GradedAt DATETIME2 NULL,
        GradedById INT NULL,
        QuestionId INT NOT NULL,
        UserId INT NOT NULL,
        AssessmentId INT NOT NULL,
        AlternativeId INT NULL,
        FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE,
        FOREIGN KEY (UserId) REFERENCES Users(Id),
        FOREIGN KEY (AssessmentId) REFERENCES Assessments(Id) ON DELETE CASCADE,
        FOREIGN KEY (AlternativeId) REFERENCES Alternatives(Id) ON DELETE SET NULL,
        FOREIGN KEY (GradedById) REFERENCES Users(Id)
    );

    -- 11. PROGRESSO
    CREATE TABLE Progress (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Status INT NOT NULL DEFAULT 0, -- 0=NotStarted, 1=InProgress, 2=Completed
        Percentage INT NOT NULL DEFAULT 0,
        LastUpdated DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CompletedAt DATETIME2 NULL,
        UserId INT NOT NULL,
        LessonId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        FOREIGN KEY (LessonId) REFERENCES Lessons(Id) ON DELETE CASCADE,
        UNIQUE(UserId, LessonId) -- Evita progresso duplicado
    );

    -- 12. CERTIFICADOS
    CREATE TABLE Certificates (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        VerificationCode NVARCHAR(50) NOT NULL UNIQUE,
        IssuedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsValid BIT NOT NULL DEFAULT 1,
        UserId INT NOT NULL,
        CourseId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
    );

    -- 13. FÓRUNS
    CREATE TABLE Forums (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        CourseId INT NOT NULL,
        FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
    );

    -- 14. TÓPICOS DO FÓRUM
    CREATE TABLE ForumTopics (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Message NVARCHAR(2000) NOT NULL,
        PostedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        IsPinned BIT NOT NULL DEFAULT 0,
        IsLocked BIT NOT NULL DEFAULT 0,
        ForumId INT NOT NULL,
        UserId INT NOT NULL,
        FOREIGN KEY (ForumId) REFERENCES Forums(Id) ON DELETE CASCADE,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );

    -- 15. MENSAGENS DO FÓRUM
    CREATE TABLE ForumMessages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Message NVARCHAR(2000) NOT NULL,
        PostedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        TopicId INT NOT NULL,
        UserId INT NOT NULL,
        FOREIGN KEY (TopicId) REFERENCES ForumTopics(Id) ON DELETE CASCADE,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );

    -- 16. NOTIFICAÇÕES
    CREATE TABLE Notifications (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Message NVARCHAR(1000) NOT NULL,
        IsRead BIT NOT NULL DEFAULT 0,
        SentAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ReadAt DATETIME2 NULL,
        Type INT NOT NULL DEFAULT 0, -- 0=General, 1=Course, 2=Assessment, 3=Forum, 4=System
        UserId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );

    -- ÍNDICES PARA PERFORMANCE
    CREATE INDEX IX_Users_Email ON Users(Email);
    CREATE INDEX IX_Users_Role ON Users(Role);
    CREATE INDEX IX_Courses_Status ON Courses(Status);
    CREATE INDEX IX_Courses_TeacherId ON Courses(TeacherId);
    CREATE INDEX IX_Courses_CategoryId ON Courses(CategoryId);
    CREATE INDEX IX_Enrollments_UserId ON Enrollments(UserId);
    CREATE INDEX IX_Enrollments_CourseId ON Enrollments(CourseId);
    CREATE INDEX IX_Enrollments_Status ON Enrollments(Status);
    CREATE INDEX IX_Lessons_ModuleId ON Lessons(ModuleId);
    CREATE INDEX IX_Progress_UserId ON Progress(UserId);
    CREATE INDEX IX_Progress_LessonId ON Progress(LessonId);
    CREATE INDEX IX_Assessments_CourseId ON Assessments(CourseId);
    CREATE INDEX IX_Questions_AssessmentId ON Questions(AssessmentId);
    CREATE INDEX IX_Responses_UserId ON Responses(UserId);
    CREATE INDEX IX_Responses_AssessmentId ON Responses(AssessmentId);
    CREATE INDEX IX_ForumTopics_ForumId ON ForumTopics(ForumId);
    CREATE INDEX IX_ForumMessages_TopicId ON ForumMessages(TopicId);
    CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
    CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);

    -- DADOS INICIAIS
    INSERT INTO Categories (Name, Description) VALUES
    ('Tecnologia', 'Cursos relacionados à tecnologia e programação'),
    ('Língua Estrangeira', 'Cursos relacionados à língua estrangeira');

    INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES
    ('Administrador', 'admin@eduplatform.com', 'admin123', 2),
    ('João Silva', 'joao.silva@eduplatform.com', 'prof123', 1),
    ('Maria Santos', 'maria.santos@eduplatform.com', 'prof123', 1),
    ('Pedro Costa', 'pedro.costa@eduplatform.com', 'aluno123', 0),
    ('Ana Oliveira', 'ana.oliveira@eduplatform.com', 'aluno123', 0);

    PRINT 'Estrutura do banco criada com sucesso!';
    PRINT 'Dados iniciais inseridos!';
END
ELSE
BEGIN
    PRINT 'Tabelas já existem no banco de dados.';
END
