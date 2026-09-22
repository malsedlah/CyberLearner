# CyberLearner — Final System Specification

## 1. Purpose and authority
This document is the single source of truth for the CyberLearner implementation. Every team member must follow it. Do not add, remove, rename, or materially change a role, workflow, business rule, database table, or major feature without team agreement and an update to this document.

CyberLearner is a web-based cybersecurity learning and certification system developed for the APU Web Applications assignment using ASP.NET Web Forms on .NET Framework 4.7.2.

## 2. Fixed project scope
CyberLearner has exactly:
- One course: **Cybersecurity Fundamentals**
- One certificate: **Cybersecurity Fundamentals Certificate**
- Two roles: **Learner** and **Management**
- One final certification examination

“Management” is the system's administrator role.

The system is an academic prototype. It is not a commercial LMS, payment platform, multi-course marketplace, or production identity system.

## 3. Required technologies and assignment coverage
The implementation must demonstrate:
- ASP.NET Web Forms / .NET Framework 4.7.2
- HTML5 and CSS
- C# server-side processing
- JavaScript/client-side processing where appropriate
- Local relational database connectivity
- Insert, Display/Read, Update, and Delete operations
- Registration and authenticated modules
- Role-based authorization
- Form validation
- Navigation and usability
- Multimedia in learning content where appropriate
- Responsive, consistent interface

Actual database DELETE must be demonstrated. Quiz and/or exam question management is the planned safe DELETE use case; attempt history must not depend on deleted question records.

## 4. Roles and permissions

### 4.1 Learner
A Learner can:
- Register for an account
- Log in and log out
- Recover/reset a forgotten password through the prototype reset flow
- View/update their profile
- Access Cybersecurity Fundamentals learning topics
- Read learning materials
- Take topic quizzes
- Progress through topics after passing the required quiz
- View final exam access status and guidelines
- Take the certification exam when eligible
- View exam results and attempt history
- Request direct exam access if already knowledgeable
- Request a retake after an eligible failed attempt and waiting period
- View their certificate after passing

A Learner cannot manage other users, edit official learning/exam content, approve requests, alter scores, or issue certificates manually.

### 4.2 Management
Management can:
- Log in to protected Management pages
- View/manage learner accounts as permitted by the implementation
- Manage learning content
- Add, edit and delete topic quiz questions
- Add, edit and delete certification exam questions
- View learner quiz/exam results
- Review and approve/reject direct exam access requests
- Review and approve/reject eligible retake requests
- View/manage certificate records as required
- View Management dashboard summaries

Management must not see plaintext passwords or arbitrarily edit learner scores to create a pass. Certificates must be tied to a successful exam attempt.

## 5. Core learner workflow
Normal route:

Registration → Login → Learner Dashboard → Topics → Learning Material → Topic Quiz → Topic Quiz Result → Next Topic → Final Exam Access → Exam Guidelines → Certification Exam → Exam Result → Certificate if passed.

The next topic remains locked until the learner passes the current topic quiz.

Alternative direct-exam route:

Learner → Direct Exam Access Request → Management Review → Approved → Exam Access → Guidelines → Certification Exam.

Failed-exam retake route:

Failed Exam → Wait 7 days → Submit Retake Request → Management Review → Approved → One additional exam attempt.

## 6. Fixed business rules
1. Public registration creates a **Learner** account only.
2. Management accounts are not created through normal public registration.
3. A learner must pass the current topic quiz before the next topic unlocks.
4. Normal final-exam access requires completion of the required learning progression.
5. A knowledgeable learner may request direct certification exam access.
6. Direct exam access is not granted until Management approves the request.
7. Exam results must be recorded and displayed after an exam attempt.
8. Passing the final certification exam makes the learner eligible for the Cybersecurity Fundamentals Certificate.
9. A failed learner must wait **7 days** before an eligible retake request.
10. A retake requires Management approval.
11. An approved retake grants **one new attempt**; it does not create unlimited attempts.
12. Quiz and exam attempt history must be preserved.
13. Certificates must correspond to a successful exam attempt.
14. Scores must be calculated from submitted answers, not manually chosen by the learner.
15. Protected pages must require a valid authenticated session and correct role.

The exact numeric quiz/exam passing mark and question count must be agreed by the group before implementation if not already specified in the approved proposal; members must not independently invent conflicting values.

## 7. Authentication, authorization and session behavior
Expected lifecycle:

Login → credentials validated → authenticated Session created → UserID and Role stored → protected system use → Logout or session expiry → Session destroyed → login required again.

Minimum session information:
- UserID
- Role
- Any small display value needed by the UI, if appropriate

Rules:
- Learner pages require an authenticated Learner.
- Management pages require an authenticated Management user.
- Direct URL access must not bypass role checks.
- Logout must clear/abandon the authenticated session.
- Passwords must be stored as hashes, never plaintext.
- SQL queries must use parameters rather than concatenating user input.

## 8. Forgot-password prototype
The planned academic prototype flow is:

Forgot Password → enter registered email → generate temporary verification code → display code on screen for demonstration → user enters code → verify code → enter new password → hash password → update Users record.

The temporary code should expire and may be kept in Session. Displaying the code on screen is a prototype limitation; real email delivery is outside the required scope unless the group later approves it.

## 9. Database — fixed nine-table design

### 9.1 Users
Purpose: all Learner and Management accounts.

Fields:
- UserID
- FullName
- Email
- Username
- PasswordHash
- Role
- AccountStatus
- CreatedAt

### 9.2 Topics
Purpose: ordered learning content for the single Cybersecurity Fundamentals course.

Fields:
- TopicID
- TopicTitle
- TopicContent
- TopicOrder
- Status

### 9.3 TopicQuizQuestions
Purpose: quiz questions belonging to topics.

Fields:
- QuestionID
- TopicID
- QuestionText
- OptionA
- OptionB
- OptionC
- OptionD
- CorrectAnswer
- Status

### 9.4 TopicQuizAttempts
Purpose: preserve learner quiz attempts and progression evidence.

Fields:
- AttemptID
- UserID
- TopicID
- Score
- PassStatus
- AttemptDate

### 9.5 ExamQuestions
Purpose: questions for the one final certification exam.

Fields:
- ExamQuestionID
- QuestionText
- OptionA
- OptionB
- OptionC
- OptionD
- CorrectAnswer
- Status

### 9.6 ExamAttempts
Purpose: preserve certification exam attempts.

Fields:
- ExamAttemptID
- UserID
- AttemptNumber
- Score
- PassStatus
- AttemptDate

### 9.7 DirectExamRequests
Purpose: learner requests to bypass normal course progression and access the final exam.

Fields:
- RequestID
- UserID
- RequestDate
- Status
- ReviewedBy
- ReviewedDate

### 9.8 RetakeRequests
Purpose: requests for another exam attempt after failure and the waiting rule.

Fields:
- RetakeRequestID
- UserID
- ExamAttemptID
- RequestDate
- Status
- ReviewedBy
- ReviewedDate

### 9.9 Certificates
Purpose: certificate records produced from successful exam attempts.

Fields:
- CertificateID
- UserID
- ExamAttemptID
- CertificateNumber
- IssueDate

## 10. Database relationships
- Topics 1 → many TopicQuizQuestions
- Users 1 → many TopicQuizAttempts
- Topics 1 → many TopicQuizAttempts
- Users 1 → many ExamAttempts
- Users 1 → many DirectExamRequests
- Users 1 → many RetakeRequests
- ExamAttempts 1 → related RetakeRequests
- Users 1 → certificate record(s) as permitted by business logic
- ExamAttempts → Certificates
- DirectExamRequests.ReviewedBy → Management user in Users
- RetakeRequests.ReviewedBy → Management user in Users
- ExamQuestions are standalone because there is exactly one final exam

Do not add Courses, Exams, Progress, Sessions, Payments, Enrolments, Roles, or other new tables merely because they are common in commercial LMS products. The current design intentionally derives progression from attempts and stores authentication session state in ASP.NET Session.

## 11. Required interfaces

### Public
- Default.aspx — Home
- Register.aspx — Registration
- Login.aspx — Login
- ForgotPassword.aspx — Password reset prototype
- Logout.aspx — Logout/session termination

### Learner
- Learner/Dashboard.aspx
- Learner/Profile.aspx
- Learner/Topics.aspx
- Learner/LearningMaterial.aspx
- Learner/TopicQuiz.aspx
- Learner/TopicQuizResult.aspx
- Learner/ExamAccess.aspx
- Learner/ExamGuidelines.aspx
- Learner/CertificationExam.aspx
- Learner/ExamResult.aspx
- Learner/Results.aspx
- Learner/Requests.aspx
- Learner/Certificate.aspx

### Management
- Management/Dashboard.aspx
- Management/Learners.aspx
- Management/LearningContent.aspx
- Management/TopicQuizzes.aspx
- Management/CertificationExam.aspx
- Management/LearnerResults.aspx
- Management/Requests.aspx
- Management/Certificates.aspx

## 12. Navigation
Learner navigation:
Dashboard | Course | Exam | Results | Requests | Certificate | Profile | Logout

Management navigation:
Dashboard | Learners | Learning Content | Topic Quizzes | Certification Exam | Learner Results | Requests | Certificates | Logout

Public pages provide appropriate Home/Register/Login navigation.

## 13. UI and usability requirements
- Consistent master-page-based layout.
- Responsive enough for laptop, tablet and phone.
- Clear validation/error/success messages.
- Dashboard pages summarize information; detailed operations belong on their dedicated pages.
- Quiz and final exam should present one question at a time unless the group explicitly agrees on another consistent implementation.
- Locked content must be visibly distinguishable from available content.
- Forms must validate required fields and sensible formats.
- Destructive actions such as deleting a question should require confirmation.
- Never display PasswordHash, CorrectAnswer to learners during an active assessment, or internal-only security data.

## 14. Expected page behavior

### Registration
Validate required fields, unique username/email as appropriate, password requirements and confirmation. Insert a Learner into Users with a hashed password.

### Login
Validate credentials and active account state. Create Session and redirect according to Role.

### Profile
Display the current learner's information and permit approved profile updates without exposing password hashes.

### Topics/Learning Material
Display ordered active topics and content. Determine access from the learner's passed topic attempts.

### Topic Quiz
Load active questions for the selected accessible topic, collect answers, calculate score, determine pass/fail, and save TopicQuizAttempts.

### Exam Access/Guidelines
Determine whether the learner qualifies through course completion or approved direct access. Prevent unauthorized exam entry.

### Certification Exam
Load active exam questions, collect answers, calculate score/pass status and save ExamAttempts. Retake eligibility must respect approved retake logic.

### Requests
Learner can see relevant request state and submit only valid requests. Management can review pending requests and record reviewer/date/status.

### Certificate
Available only when supported by a passing ExamAttempt. Certificate number must identify the certificate record and should not be freely user-editable.

### Management content/question pages
Support required CRUD operations. Deleting questions is the planned physical DELETE demonstration; historical attempt records remain intact because attempts store results rather than question IDs.

## 15. Validation and security baseline
At minimum:
- Required-field validation
- Email-format validation
- Password confirmation
- Server-side validation even if client validation exists
- Parameterized SQL commands
- Password hashing
- Session/role checks on protected pages
- Output handling appropriate to ASP.NET controls
- Confirmation before destructive operations
- Do not expose exception/database details to normal users where avoidable

## 16. Shared implementation files
These affect multiple members and require coordination:
- Data/DatabaseHelper.cs
- Web.config
- Site.Master and code-behind
- Learner.Master and code-behind
- Management.Master and code-behind
- Styles/site.css
- Scripts/site.js
- Database/CreateTables.sql
- Database/SeedData.sql
- Global.asax

No member should make a major incompatible change to these files without informing the group.

## 17. Explicitly out of scope
Unless the whole group approves a scope change, do **not** add:
- Multiple courses
- Multiple certificates
- Student/teacher/instructor roles
- Payment/subscription/e-commerce
- Live chat or social network
- Real-time video classes
- External identity providers
- Mobile application
- AI tutor/chatbot
- Gamification system
- Complex analytics/recommendation engine
- Production email/SMS service
- Cloud deployment architecture
- Separate session database
- Additional course/exam/progress tables
- Features unrelated to the assignment

Keep the project small enough to complete, test and explain.

## 18. Definition of finished system
CyberLearner is complete when:
- It builds and runs successfully.
- Local database schema and seed data can be created.
- Learner can register/login/logout.
- Authentication and role authorization work.
- Learner can progress through learning topics and quizzes.
- Progression gating works.
- Direct exam access request/approval works.
- Eligible learner can take the final exam.
- Exam score/result is saved and displayed.
- Failed learner follows the 7-day retake request and approval workflow.
- Passing learner receives/views the certificate.
- Management can manage learners/content/questions and review results/requests/certificates within defined scope.
- Insert, Read/Display, Update and actual Delete are demonstrated.
- Validation, navigation, sessions and usability are functional.
- No required workflow depends on placeholder-only code.
- The group can explain how each major feature and database relationship works.

## 19. Scope-control rule
When implementation details are unclear, first check this specification and MEMBER_RESPONSIBILITIES.md. If neither document defines the detail, choose the smallest implementation consistent with the assignment and existing architecture, or ask the group before introducing a new feature/table/workflow. Do not silently expand scope.
