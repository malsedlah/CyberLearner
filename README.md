# CyberLearner

CyberLearner is an ASP.NET Web Forms group project for CT050-3-2-WAPP (Web Applications).

It is a web-based learning system for one course, Cybersecurity Fundamentals. Learners study topics, pass topic quizzes, take the final certification exam and receive a certificate. Management users manage the content, questions, learners, requests and certificates.

## Requirements

- Visual Studio with the ASP.NET and web development workload
- .NET Framework 4.7.2
- SQL Server Express LocalDB

## How to run

1. Open `CyberLearner.slnx` in Visual Studio.
2. Restore the NuGet packages (right click the solution, Restore NuGet Packages).
3. Open SQL Server Object Explorer, connect to `(localdb)\MSSQLLocalDB` and run `Database/CreateTables.sql`.
4. Run `Database/SeedData.sql` on the same server.
5. Press F5 to run the project.

The connection string is in `Web.config` under the name `CyberLearnerDB`.

## Demo account

| Role | Username | Password |
|---|---|---|
| Management | admin | Admin@123 |

Learner accounts are created from the Register page.

## Settings

These values are in the `appSettings` section of `Web.config`.

| Key | Value | Meaning |
|---|---|---|
| QuizPassMark | 60 | Pass mark (%) for a topic quiz |
| QuizQuestionCount | 5 | Number of questions in a topic quiz |
| ExamPassMark | 70 | Pass mark (%) for the certification exam |
| ExamQuestionCount | 20 | Number of questions in the certification exam |
| RetakeWaitDays | 7 | Days to wait before a retake request |
| ResetCodeMinutes | 10 | Minutes before a password reset code expires |

## Project structure

- `CyberLearner/` - the web project (pages, master pages, services, models)
- `Database/` - SQL scripts for the tables and the sample data
- `Docs/` - system specification, member responsibilities and database contract
