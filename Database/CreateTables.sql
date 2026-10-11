IF DB_ID('CyberLearner') IS NULL
    CREATE DATABASE CyberLearner;
GO

USE CyberLearner;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Learner', 'Management')),
    AccountStatus NVARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (AccountStatus IN ('Active', 'Inactive')),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Topics (
    TopicID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TopicTitle NVARCHAR(150) NOT NULL,
    TopicContent NVARCHAR(MAX) NOT NULL,
    TopicOrder INT NOT NULL UNIQUE CHECK (TopicOrder > 0),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive'))
);

CREATE TABLE TopicQuizQuestions (
    QuestionID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TopicID INT NOT NULL FOREIGN KEY REFERENCES Topics(TopicID),
    QuestionText NVARCHAR(500) NOT NULL,
    OptionA NVARCHAR(300) NOT NULL,
    OptionB NVARCHAR(300) NOT NULL,
    OptionC NVARCHAR(300) NOT NULL,
    OptionD NVARCHAR(300) NOT NULL,
    CorrectAnswer CHAR(1) NOT NULL CHECK (CorrectAnswer IN ('A', 'B', 'C', 'D')),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive'))
);

CREATE TABLE TopicQuizAttempts (
    AttemptID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    TopicID INT NOT NULL FOREIGN KEY REFERENCES Topics(TopicID),
    Score DECIMAL(5,2) NOT NULL CHECK (Score BETWEEN 0 AND 100),
    PassStatus NVARCHAR(10) NOT NULL CHECK (PassStatus IN ('Pass', 'Fail')),
    AttemptDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ExamQuestions (
    ExamQuestionID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    QuestionText NVARCHAR(500) NOT NULL,
    OptionA NVARCHAR(300) NOT NULL,
    OptionB NVARCHAR(300) NOT NULL,
    OptionC NVARCHAR(300) NOT NULL,
    OptionD NVARCHAR(300) NOT NULL,
    CorrectAnswer CHAR(1) NOT NULL CHECK (CorrectAnswer IN ('A', 'B', 'C', 'D')),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive'))
);

CREATE TABLE ExamAttempts (
    ExamAttemptID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    AttemptNumber INT NOT NULL CHECK (AttemptNumber > 0),
    Score DECIMAL(5,2) NOT NULL CHECK (Score BETWEEN 0 AND 100),
    PassStatus NVARCHAR(10) NOT NULL CHECK (PassStatus IN ('Pass', 'Fail')),
    AttemptDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE DirectExamRequests (
    RequestID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    ReviewedBy INT NULL FOREIGN KEY REFERENCES Users(UserID),
    ReviewedDate DATETIME NULL
);

CREATE TABLE RetakeRequests (
    RetakeRequestID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    ExamAttemptID INT NOT NULL FOREIGN KEY REFERENCES ExamAttempts(ExamAttemptID),
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    ReviewedBy INT NULL FOREIGN KEY REFERENCES Users(UserID),
    ReviewedDate DATETIME NULL
);

CREATE TABLE Certificates (
    CertificateID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    ExamAttemptID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES ExamAttempts(ExamAttemptID),
    CertificateNumber NVARCHAR(50) NOT NULL UNIQUE,
    IssueDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO
