# CyberLearner — Database Contract

## 1. Authority
This file is the fixed database contract for all four members. Read it together with `SYSTEM_SPECIFICATION.md` and `MEMBER_RESPONSIBILITIES.md`.

**Do not rename tables, columns, keys, fixed values, or relationships in your own branch. Do not add a table or column without group agreement and an update to this contract first.** C# models, SQL commands, services, pages, `CreateTables.sql`, and `SeedData.sql` must use these exact names.

Database technology: local SQL Server / LocalDB compatible with the ASP.NET Web Forms .NET Framework 4.7.2 project.

## 2. Naming rules
- Table names use the exact PascalCase plural names below.
- Column names use the exact PascalCase names below.
- Primary keys are integer identity columns.
- Foreign-key column names match the referenced key name where practical.
- SQL code must use parameterized commands.
- Date/time values use `DATETIME`.
- Boolean-like workflow results are stored using the fixed text values defined below, not random alternatives.
- Never use different spellings such as `user_id`, `QuizQuestions`, `Admin`, `Passed`, `Waiting`, etc.

## 3. Fixed values

### Role
Only:
- `Learner`
- `Management`

### AccountStatus
Only:
- `Active`
- `Inactive`

Default for a newly registered learner: `Active`.

### Content/question Status
For `Topics.Status`, `TopicQuizQuestions.Status`, and `ExamQuestions.Status`:
- `Active`
- `Inactive`

Default: `Active`.

### PassStatus
For `TopicQuizAttempts.PassStatus` and `ExamAttempts.PassStatus`:
- `Pass`
- `Fail`

Do not use `Passed`, `Failed`, `True`, `False`, or member-specific alternatives.

### Request Status
For `DirectExamRequests.Status` and `RetakeRequests.Status`:
- `Pending`
- `Approved`
- `Rejected`

New requests default to `Pending`.

## 4. Fixed schema

### 4.1 Users
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| UserID | INT IDENTITY(1,1) | No | Primary Key |
| FullName | NVARCHAR(100) | No | |
| Email | NVARCHAR(150) | No | Unique |
| Username | NVARCHAR(50) | No | Unique |
| PasswordHash | NVARCHAR(255) | No | Never plaintext |
| Role | NVARCHAR(20) | No | `Learner` or `Management` |
| AccountStatus | NVARCHAR(20) | No | Default `Active` |
| CreatedAt | DATETIME | No | Default current date/time |

Public registration must insert `Role='Learner'`.

### 4.2 Topics
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| TopicID | INT IDENTITY(1,1) | No | Primary Key |
| TopicTitle | NVARCHAR(150) | No | |
| TopicContent | NVARCHAR(MAX) | No | Learning content |
| TopicOrder | INT | No | Positive, unique ordering value |
| Status | NVARCHAR(20) | No | Default `Active` |

There is no CourseID because the system has exactly one course.

### 4.3 TopicQuizQuestions
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| QuestionID | INT IDENTITY(1,1) | No | Primary Key |
| TopicID | INT | No | FK → Topics.TopicID |
| QuestionText | NVARCHAR(500) | No | |
| OptionA | NVARCHAR(300) | No | |
| OptionB | NVARCHAR(300) | No | |
| OptionC | NVARCHAR(300) | No | |
| OptionD | NVARCHAR(300) | No | |
| CorrectAnswer | CHAR(1) | No | `A`, `B`, `C`, or `D` |
| Status | NVARCHAR(20) | No | Default `Active` |

Questions may be physically deleted for the assignment CRUD demonstration. Do not cascade that deletion into attempt history.

### 4.4 TopicQuizAttempts
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| AttemptID | INT IDENTITY(1,1) | No | Primary Key |
| UserID | INT | No | FK → Users.UserID |
| TopicID | INT | No | FK → Topics.TopicID |
| Score | DECIMAL(5,2) | No | Percentage 0–100 |
| PassStatus | NVARCHAR(10) | No | `Pass` or `Fail` |
| AttemptDate | DATETIME | No | Default current date/time |

Keep every required attempt. Progression is derived from passed attempts; do not create a Progress table.

### 4.5 ExamQuestions
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| ExamQuestionID | INT IDENTITY(1,1) | No | Primary Key |
| QuestionText | NVARCHAR(500) | No | |
| OptionA | NVARCHAR(300) | No | |
| OptionB | NVARCHAR(300) | No | |
| OptionC | NVARCHAR(300) | No | |
| OptionD | NVARCHAR(300) | No | |
| CorrectAnswer | CHAR(1) | No | `A`, `B`, `C`, or `D` |
| Status | NVARCHAR(20) | No | Default `Active` |

There is no ExamID because there is exactly one final certification exam. Questions may be physically deleted without deleting historical ExamAttempts.

### 4.6 ExamAttempts
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| ExamAttemptID | INT IDENTITY(1,1) | No | Primary Key |
| UserID | INT | No | FK → Users.UserID |
| AttemptNumber | INT | No | Starts at 1; positive |
| Score | DECIMAL(5,2) | No | Percentage 0–100 |
| PassStatus | NVARCHAR(10) | No | `Pass` or `Fail` |
| AttemptDate | DATETIME | No | Default current date/time |

AttemptNumber is per learner, not a global exam counter. Historical attempts must not be edited to manufacture a pass.

### 4.7 DirectExamRequests
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| RequestID | INT IDENTITY(1,1) | No | Primary Key |
| UserID | INT | No | FK → Users.UserID |
| RequestDate | DATETIME | No | Default current date/time |
| Status | NVARCHAR(20) | No | Default `Pending` |
| ReviewedBy | INT | Yes | FK → Users.UserID; Management reviewer |
| ReviewedDate | DATETIME | Yes | Set when reviewed |

While Pending, ReviewedBy and ReviewedDate remain NULL. Approval/rejection sets reviewer and review date.

### 4.8 RetakeRequests
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| RetakeRequestID | INT IDENTITY(1,1) | No | Primary Key |
| UserID | INT | No | FK → Users.UserID |
| ExamAttemptID | INT | No | FK → ExamAttempts.ExamAttemptID |
| RequestDate | DATETIME | No | Default current date/time |
| Status | NVARCHAR(20) | No | Default `Pending` |
| ReviewedBy | INT | Yes | FK → Users.UserID; Management reviewer |
| ReviewedDate | DATETIME | Yes | Set when reviewed |

The referenced ExamAttempt must belong to the same learner and be failed. The learner becomes eligible to submit only after 7 days from that failed attempt. An approved request permits one additional attempt.

### 4.9 Certificates
| Column | SQL type | Null | Key / rule |
|---|---|---|---|
| CertificateID | INT IDENTITY(1,1) | No | Primary Key |
| UserID | INT | No | FK → Users.UserID |
| ExamAttemptID | INT | No | FK → ExamAttempts.ExamAttemptID |
| CertificateNumber | NVARCHAR(50) | No | Unique |
| IssueDate | DATETIME | No | Default current date/time |

A certificate can only reference a passing ExamAttempt belonging to that UserID. One successful exam attempt must not generate duplicate certificate records.

## 5. Relationship contract
- `TopicQuizQuestions.TopicID` → `Topics.TopicID`
- `TopicQuizAttempts.UserID` → `Users.UserID`
- `TopicQuizAttempts.TopicID` → `Topics.TopicID`
- `ExamAttempts.UserID` → `Users.UserID`
- `DirectExamRequests.UserID` → `Users.UserID`
- `DirectExamRequests.ReviewedBy` → `Users.UserID`
- `RetakeRequests.UserID` → `Users.UserID`
- `RetakeRequests.ExamAttemptID` → `ExamAttempts.ExamAttemptID`
- `RetakeRequests.ReviewedBy` → `Users.UserID`
- `Certificates.UserID` → `Users.UserID`
- `Certificates.ExamAttemptID` → `ExamAttempts.ExamAttemptID`

Reviewer foreign keys point to Users, but application authorization must ensure the reviewer has Role `Management`.

## 6. Deletion and history rules
- Physical DELETE is expected for Management question management (`TopicQuizQuestions` and/or `ExamQuestions`).
- Do not delete quiz/exam attempt history when a question is deleted.
- Do not cascade-delete Users, Topics, ExamAttempts, TopicQuizAttempts, requests, or Certificates as a convenience.
- Use `Status='Inactive'` where an account/content item should be disabled without destroying important history.
- Foreign keys should protect historical integrity.

## 7. Data ownership by member
- Member 1: `Users`
- Member 2: `Topics`, `TopicQuizQuestions`, `TopicQuizAttempts`
- Member 3: `ExamQuestions`, `ExamAttempts`
- Member 4: `DirectExamRequests`, `RetakeRequests`, `Certificates`

Ownership means implementing the assigned features. It does **not** mean a member may independently rename or redesign the table.

## 8. Shared database files
- `Database/CreateTables.sql` — authoritative SQL schema implementing this contract.
- `Database/SeedData.sql` — agreed demonstration/sample data.
- `Data/DatabaseHelper.cs` — common connection/command helper.
- `Web.config` — shared connection string/configuration.

The local `.mdf`/database data file should not be used as the team synchronization mechanism. The SQL scripts are what allow every member to create the same schema locally.

## 9. Seed-data rules
Seed data should provide enough content to demonstrate the complete system, including:
- At least one Management account suitable for development/demo; its password must be created using the same hashing approach as the application.
- Several ordered Cybersecurity Fundamentals topics.
- Active topic quiz questions for each demo topic.
- Active final exam questions.

Do not commit real personal passwords or secrets. Learner accounts/attempts/requests/certificates may be created through normal workflows during testing rather than requiring hard-coded records.

## 10. C# model mapping rule
Model property names must match the database contract unless a deliberate mapping is documented. Examples:
- `User.UserID` ↔ `Users.UserID`
- `Topic.TopicID` ↔ `Topics.TopicID`
- `TopicQuizQuestion.QuestionID` ↔ `TopicQuizQuestions.QuestionID`
- `ExamQuestion.ExamQuestionID` ↔ `ExamQuestions.ExamQuestionID`
- `ExamAttempt.ExamAttemptID` ↔ `ExamAttempts.ExamAttemptID`

Do not invent alternate names in individual services that make integration ambiguous.

## 11. Values not yet fixed by the approved scope
The current project scope does not yet provide an agreed numeric:
- Topic quiz passing percentage
- Final exam passing percentage
- Number of questions presented per quiz
- Number of questions presented in the final exam
- CertificateNumber formatting pattern
- Password complexity minimums
- Password-reset-code expiry duration

Until the group agrees and documents these values, **do not independently hard-code conflicting values**. A member may prepare the code so the value can be set centrally later.

## 12. Database change procedure
If a member believes the schema must change:
1. Do not change it only on the member branch.
2. Explain why the current contract cannot support the required feature.
3. Get group agreement.
4. Update `DATABASE_CONTRACT.md` first.
5. Update `SYSTEM_SPECIFICATION.md` / responsibilities if affected.
6. Update `CreateTables.sql`, models and dependent code together.
7. Sync the approved main changes into all member branches.

This prevents one member's database change from breaking the other three sections.

## 13. Final database checklist
Before integration:
- Exactly the nine agreed tables exist.
- All table and column names match this document.
- PK/FK relationships match this document.
- Fixed values use the exact spellings in Section 3.
- PasswordHash never contains plaintext passwords.
- SQL commands are parameterized.
- Required defaults/unique constraints are present.
- Question DELETE works without destroying attempt history.
- Retake and certificate references preserve historical integrity.
- All four members use the same connection configuration and schema.
