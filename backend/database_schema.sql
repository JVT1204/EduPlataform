-- =====================================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS EDUPLATFORM
-- Sistema de LMS (Learning Management System)
-- =====================================================

-- Criar banco de dados
CREATE DATABASE EduPlatform;
GO

USE EduPlatform;
GO

-- =====================================================
-- TABELAS PRINCIPAIS
-- =====================================================

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

-- =====================================================
-- ÍNDICES PARA PERFORMANCE
-- =====================================================

-- Índices para consultas frequentes
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

-- =====================================================
-- DADOS INICIAIS
-- =====================================================

-- Inserir categorias padrão
INSERT INTO Categories (Name, Description) VALUES
('Tecnologia', 'Cursos relacionados à tecnologia e programação'),
('Língua Estrangeira', 'Cursos relacionados à língua estrangeira');

-- Inserir usuário administrador padrão
INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES
('Administrador', 'admin@eduplatform.com', 'admin123', 2);

-- Inserir alguns professores de exemplo
INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES
('João Silva', 'joao.silva@eduplatform.com', 'prof123', 1),
('Maria Santos', 'maria.santos@eduplatform.com', 'prof123', 1);

-- Inserir alguns alunos de exemplo
INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES
('Pedro Costa', 'pedro.costa@eduplatform.com', 'aluno123', 0),
('Ana Oliveira', 'ana.oliveira@eduplatform.com', 'aluno123', 0);

-- =====================================================
-- VIEWS ÚTEIS
-- =====================================================

-- View para cursos com informações do professor e categoria
CREATE VIEW vw_CoursesWithDetails AS
SELECT 
    c.Id,
    c.Name,
    c.Description,
    c.ImageUrl,
    c.StartDate,
    c.EndDate,
    c.Status,
    c.IsActive,
    cat.Name AS CategoryName,
    u.Name AS TeacherName,
    u.Email AS TeacherEmail,
    (SELECT COUNT(*) FROM Enrollments e WHERE e.CourseId = c.Id AND e.Status = 0) AS EnrolledStudentsCount,
    (SELECT COUNT(*) FROM Modules m WHERE m.CourseId = c.Id AND m.IsActive = 1) AS ModulesCount
FROM Courses c
INNER JOIN Categories cat ON c.CategoryId = cat.Id
INNER JOIN Users u ON c.TeacherId = u.Id;

-- View para progresso dos alunos
CREATE VIEW vw_StudentProgress AS
SELECT 
    u.Id AS UserId,
    u.Name AS StudentName,
    c.Id AS CourseId,
    c.Name AS CourseName,
    m.Id AS ModuleId,
    m.Title AS ModuleTitle,
    l.Id AS LessonId,
    l.Title AS LessonTitle,
    p.Status,
    p.Percentage,
    p.LastUpdated
FROM Users u
INNER JOIN Enrollments e ON u.Id = e.UserId
INNER JOIN Courses c ON e.CourseId = c.Id
INNER JOIN Modules m ON c.Id = m.CourseId
INNER JOIN Lessons l ON m.Id = l.ModuleId
LEFT JOIN Progress p ON u.Id = p.UserId AND l.Id = p.LessonId
WHERE u.Role = 0 AND e.Status = 0;

-- =====================================================
-- PROCEDURES ÚTEIS
-- =====================================================

-- Procedure para matricular aluno em um curso
CREATE PROCEDURE sp_EnrollStudent
    @UserId INT,
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar se o usuário já está matriculado
    IF EXISTS (SELECT 1 FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId)
    BEGIN
        RAISERROR ('Aluno já está matriculado neste curso', 16, 1);
        RETURN;
    END
    
    -- Verificar se o curso existe e está ativo
    IF NOT EXISTS (SELECT 1 FROM Courses WHERE Id = @CourseId AND IsActive = 1)
    BEGIN
        RAISERROR ('Curso não encontrado ou inativo', 16, 1);
        RETURN;
    END
    
    -- Inserir matrícula
    INSERT INTO Enrollments (UserId, CourseId)
    VALUES (@UserId, @CourseId);
    
    -- Criar notificação
    INSERT INTO Notifications (UserId, Title, Message, Type)
    SELECT @UserId, 'Matrícula Confirmada', 
           'Sua matrícula no curso foi confirmada com sucesso!', 1;
END

-- Procedure para calcular progresso do aluno
CREATE PROCEDURE sp_CalculateStudentProgress
    @UserId INT,
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TotalLessons INT, @CompletedLessons INT;
    
    -- Contar total de aulas no curso
    SELECT @TotalLessons = COUNT(*)
    FROM Lessons l
    INNER JOIN Modules m ON l.ModuleId = m.Id
    WHERE m.CourseId = @CourseId AND l.IsActive = 1;
    
    -- Contar aulas completadas
    SELECT @CompletedLessons = COUNT(*)
    FROM Progress p
    INNER JOIN Lessons l ON p.LessonId = l.Id
    INNER JOIN Modules m ON l.ModuleId = m.Id
    WHERE p.UserId = @UserId AND m.CourseId = @CourseId AND p.Status = 2;
    
    -- Retornar progresso
    SELECT 
        @UserId AS UserId,
        @CourseId AS CourseId,
        @TotalLessons AS TotalLessons,
        @CompletedLessons AS CompletedLessons,
        CASE 
            WHEN @TotalLessons = 0 THEN 0
            ELSE (@CompletedLessons * 100.0) / @TotalLessons
        END AS ProgressPercentage;
END

-- =====================================================
-- TRIGGERS ÚTEIS
-- =====================================================

-- Trigger para atualizar UpdatedAt automaticamente
CREATE TRIGGER tr_Users_UpdateTimestamp
ON Users
AFTER UPDATE
AS
BEGIN
    UPDATE Users
    SET UpdatedAt = GETUTCDATE()
    FROM Users u
    INNER JOIN inserted i ON u.Id = i.Id;
END

-- Trigger para atualizar UpdatedAt em cursos
CREATE TRIGGER tr_Courses_UpdateTimestamp
ON Courses
AFTER UPDATE
AS
BEGIN
    UPDATE Courses
    SET UpdatedAt = GETUTCDATE()
    FROM Courses c
    INNER JOIN inserted i ON c.Id = i.Id;
END

-- Trigger para marcar notificação como lida quando visualizada
CREATE TRIGGER tr_Notifications_MarkAsRead
ON Notifications
AFTER UPDATE
AS
BEGIN
    IF UPDATE(IsRead)
    BEGIN
        UPDATE Notifications
        SET ReadAt = GETUTCDATE()
        FROM Notifications n
        INNER JOIN inserted i ON n.Id = i.Id
        WHERE i.IsRead = 1 AND n.ReadAt IS NULL;
    END
END

PRINT 'Banco de dados EduPlatform criado com sucesso!';
PRINT 'Todas as tabelas, índices, views, procedures e triggers foram criados.';
PRINT 'Dados iniciais foram inseridos.';
