# CyberLearner — Member Responsibilities and Scope Boundaries

## 1. Purpose
This document tells each member exactly what they own. Read **SYSTEM_SPECIFICATION.md first**. That file defines the final system and business rules; this file divides the implementation work.

Every member must:
- Work on their assigned branch.
- Stay inside their assigned functional scope.
- Test their work before requesting merge to main.
- Commit meaningful changes with clear messages.
- Avoid rewriting another member's files without coordination.
- Follow the fixed database/business rules in SYSTEM_SPECIFICATION.md.
- Ask the group before changing a shared file in a way that affects others.

Branch mapping:
- Member 1 → `member-1`
- Member 2 → `member-2`
- Member 3 → `member-3`
- Member 4 → `member-4`

`main` is the integrated/stable branch. Normal feature development should not be done directly on main.

---

## 2. Member 1 — Accounts, Authentication and Learner Management

### Mission
Implement identity and account functionality so users can securely register, authenticate, maintain their account and enter only the area allowed by their role.

### Primary pages owned
- `Register.aspx` + code-behind
- `Login.aspx` + code-behind
- `ForgotPassword.aspx` + code-behind
- `Logout.aspx` + code-behind
- `Learner/Profile.aspx` + code-behind
- `Management/Learners.aspx` + code-behind

### Backend files owned
- `Services/AuthService.cs`
- `Services/UserService.cs`
- `Security/PasswordHelper.cs`
- `Security/SessionHelper.cs`
- `Models/User.cs`

### Main database table
- `Users`

### Required functionality
1. Learner registration.
2. Public registration always assigns Learner role.
3. Validate required registration fields.
4. Prevent invalid/duplicate account information according to agreed rules.
5. Hash passwords before database storage.
6. Login by checking stored credentials safely.
7. Reject/handle inactive accounts according to AccountStatus.
8. Create authenticated ASP.NET Session containing UserID and Role.
9. Redirect Learner and Management to the correct area.
10. Prevent unauthorized role access through reusable session/role checks.
11. Logout and abandon/clear Session.
12. Learner profile display/update.
13. Management learner-list/account-management operations required by the project.
14. Forgot-password prototype: registered email → temporary code → on-screen demo code → verification → new hashed password → Users update.
15. Temporary reset code expires and is not stored as the user's password.

### Must not do
- Do not create Management accounts through public registration.
- Do not store plaintext passwords.
- Do not expose PasswordHash.
- Do not implement learning progression, quiz scoring, exam scoring, retake approval, or certificates.
- Do not add OAuth/Google login/2FA unless the group formally changes scope.
- Do not create new account-related tables without agreement.

### Dependencies / hand-off
Other members depend on Member 1's Session UserID and Role conventions. These names/meanings must remain stable after agreement.

### Member 1 completion checklist
- Registration works and inserts a Learner.
- Password is hashed.
- Login works for valid Learner and Management accounts.
- Wrong credentials fail safely.
- Session and role protection work.
- Logout destroys authenticated session.
- Profile read/update works.
- Forgot-password prototype works.
- Management learner page works within scope.
- No password hash is displayed.

---

## 3. Member 2 — Learning Content, Progression and Topic Quizzes

### Mission
Implement the learning journey from topics/materials through topic quizzes and progression unlocking.

### Primary pages owned
Learner:
- `Learner/Topics.aspx`
- `Learner/LearningMaterial.aspx`
- `Learner/TopicQuiz.aspx`
- `Learner/TopicQuizResult.aspx`

Management:
- `Management/LearningContent.aspx`
- `Management/TopicQuizzes.aspx`

### Backend files owned
- `Services/LearningService.cs`
- `Services/QuizService.cs`
- `Models/Topic.cs`
- `Models/TopicQuizQuestion.cs`
- `Models/TopicQuizAttempt.cs`

### Main database tables
- `Topics`
- `TopicQuizQuestions`
- `TopicQuizAttempts`

### Required functionality
1. Display active learning topics in TopicOrder.
2. Display the selected topic's learning material.
3. Determine topic availability from learner progress.
4. Keep later topics locked until the current required topic quiz is passed.
5. Load active quiz questions belonging to the selected topic.
6. Present quiz questions one at a time according to agreed UI.
7. Collect answers without exposing CorrectAnswer.
8. Calculate score and pass/fail using the group's agreed threshold.
9. Insert every required quiz attempt into TopicQuizAttempts.
10. Display topic quiz result.
11. Unlock next topic after a qualifying pass.
12. Preserve attempt history.
13. Management can display/add/update learning content.
14. Management can display/add/update/delete topic quiz questions.
15. Provide an actual database DELETE operation for quiz questions if this is the chosen CRUD demonstration.
16. Validate Management forms and require confirmation for deletion.

### Must not do
- Do not create a Courses table. There is exactly one course.
- Do not create a separate Progress table; progression is derived from quiz attempts.
- Do not implement final certification exam logic.
- Do not implement direct-exam or retake approval.
- Do not issue certificates.
- Do not delete TopicQuizAttempts merely because a question/content item is changed.
- Do not independently choose a pass threshold if the group has not agreed it.

### Dependencies / hand-off
- Uses current UserID/session conventions from Member 1.
- Member 3 needs a reliable way to determine normal course completion/final exam eligibility.
- Changes to progression rules must be coordinated with Member 3.

### Member 2 completion checklist
- Topics/materials display from database.
- Lock/unlock logic works.
- Quiz loads correct topic questions.
- Score and pass/fail calculate correctly.
- Attempt saves to database.
- Passing unlocks progression.
- Management content operations work.
- Management question CRUD works, including DELETE.
- Historical attempts remain preserved.

---

## 4. Member 3 — Certification Exam and Results

### Mission
Implement final certification exam eligibility, exam execution, scoring and result history.

### Primary pages owned
Learner:
- `Learner/ExamAccess.aspx`
- `Learner/ExamGuidelines.aspx`
- `Learner/CertificationExam.aspx`
- `Learner/ExamResult.aspx`
- `Learner/Results.aspx`

Management:
- `Management/CertificationExam.aspx`
- `Management/LearnerResults.aspx`

### Backend files owned
- `Services/ExamService.cs`
- `Models/ExamQuestion.cs`
- `Models/ExamAttempt.cs`

### Main database tables
- `ExamQuestions`
- `ExamAttempts`

### Required functionality
1. Determine whether learner currently has final-exam access.
2. Accept normal eligibility from completed learning progression.
3. Accept approved direct-exam access from Member 4's request workflow.
4. Accept an approved valid retake from Member 4's retake workflow.
5. Prevent learners without valid access from entering the exam directly by URL.
6. Display exam guidelines before exam.
7. Load active ExamQuestions.
8. Present final exam questions one at a time according to agreed UI.
9. Collect answers without exposing CorrectAnswer.
10. Calculate final score and pass/fail.
11. Determine/store AttemptNumber correctly.
12. Insert ExamAttempts record.
13. Display immediate exam result.
14. Display learner's exam result/attempt history.
15. Management can view learner results.
16. Management can display/add/update/delete exam questions.
17. Preserve ExamAttempts when exam questions change/delete.
18. Expose the successful attempt information needed by Member 4 to create/show a certificate.

### Must not do
- Do not create an Exams table; there is one final exam.
- Do not implement certificate issuance inside ExamService.
- Do not approve direct exam or retake requests.
- Do not bypass the 7-day/approval rules.
- Do not manually change a learner's score to pass.
- Do not independently invent exam pass thresholds/question counts if not agreed.
- Do not delete ExamAttempts when questions are deleted.

### Dependencies / hand-off
- Uses UserID/session from Member 1.
- Reads course-completion/progression outcome produced by Member 2.
- Reads approved direct/retake authorization produced by Member 4.
- Supplies passing ExamAttempt data to Member 4 for certificate logic.

### Member 3 completion checklist
- Exam eligibility works for normal progression.
- Approved direct access works.
- Approved retake access works.
- Unauthorized direct URL access is blocked.
- Guidelines display.
- Exam loads database questions.
- Score/pass status is calculated correctly.
- Attempt number and attempt record save correctly.
- Learner and Management results pages work.
- Exam-question CRUD works.
- Attempt history remains intact.

---

## 5. Member 4 — Requests, Certificates and Dashboards

### Mission
Implement the controlled exception workflows around the final exam, certificate lifecycle, and useful dashboard summaries.

### Primary pages owned
Learner:
- `Learner/Dashboard.aspx`
- `Learner/Requests.aspx`
- `Learner/Certificate.aspx`

Management:
- `Management/Dashboard.aspx`
- `Management/Requests.aspx`
- `Management/Certificates.aspx`

### Backend files owned
- `Services/RequestService.cs`
- `Services/CertificateService.cs`
- `Models/DirectExamRequest.cs`
- `Models/RetakeRequest.cs`
- `Models/Certificate.cs`

### Main database tables
- `DirectExamRequests`
- `RetakeRequests`
- `Certificates`

### Required functionality
1. Learner dashboard summarizes relevant learning/exam/request/certificate status; it does not replace detailed pages.
2. Management dashboard summarizes relevant system information.
3. Learner can submit a direct-exam request when appropriate.
4. Prevent duplicate/invalid pending requests where appropriate.
5. Management can view pending/direct exam requests.
6. Management can approve or reject direct exam requests.
7. Record Status, ReviewedBy and ReviewedDate.
8. After a failed exam, enforce the 7-day waiting rule before an eligible retake request.
9. Retake request references the failed ExamAttempt.
10. Management can approve/reject retake request.
11. Approved retake represents permission for one additional exam attempt.
12. Certificate is available only from a successful ExamAttempt.
13. Create/use a unique CertificateNumber according to the agreed simple format.
14. Display learner certificate information.
15. Management can view/manage certificate records only within defined business rules.
16. Dashboards must read real database data, not hard-coded fake totals once implementation is complete.

### Must not do
- Do not issue a certificate without a passing ExamAttempt.
- Do not allow a learner to self-approve a request.
- Do not remove the 7-day retake rule.
- Do not make one approval grant unlimited exam attempts.
- Do not edit exam scores.
- Do not implement final exam scoring.
- Do not create extra request/certificate tables without agreement.
- Do not turn dashboards into duplicate management pages.

### Dependencies / hand-off
- Uses UserID/Role session from Member 1.
- Dashboard may read learning status from Member 2 data.
- Direct approval and retake approval must be readable by Member 3's exam-access logic.
- Certificate logic consumes successful ExamAttempts from Member 3.

### Member 4 completion checklist
- Learner and Management dashboards show useful real summaries.
- Direct exam request/approval/rejection works.
- Reviewer and review date save.
- 7-day retake rule works.
- Retake request and Management decision work.
- One-approved-retake/one-new-attempt behavior is enforced with Member 3.
- Certificate requires a passing attempt.
- Learner can view certificate.
- Management certificate view works.

---

## 6. Shared files — coordination required
These are not owned exclusively by one member:
- `Data/DatabaseHelper.cs`
- `Web.config`
- `Site.Master` and code-behind
- `Learner.Master` and code-behind
- `Management.Master` and code-behind
- `Styles/site.css`
- `Scripts/site.js`
- `Database/CreateTables.sql`
- `Database/SeedData.sql`
- `Global.asax`

Rules:
1. Tell the group before making a major shared-file change.
2. Keep database connection logic centralized in DatabaseHelper/Web.config.
3. Do not create separate incompatible connection strings/services per member.
4. Keep common navigation consistent.
5. Resolve shared-file conflicts deliberately; do not simply overwrite another member's changes.
6. Database schema changes require group agreement because all four sections depend on it.

## 7. Integration boundaries
Expected dependency chain:

Member 1 Authentication/User → Member 2 Learning/Quiz → Member 3 Exam/Results → Member 4 Certificate

Additional controlled links:
- Member 4 direct-exam approval → Member 3 exam access
- Member 3 failed ExamAttempt → Member 4 retake request
- Member 4 approved retake → Member 3 one additional attempt
- Member 3 passing ExamAttempt → Member 4 certificate
- All protected features → Member 1 session identity

Members may call another member's completed service/data contract, but should not rewrite that member's internal implementation merely for convenience.

## 8. Git workflow
For normal development:
1. Switch to your assigned branch.
2. Sync required approved changes from main before starting a new task.
3. Implement only your assigned feature/files plus coordinated shared changes.
4. Build and test.
5. Commit with a clear message.
6. Push your branch.
7. Open a Pull Request into main.
8. Review conflicts/changes before merge.
9. Merge only tested work.

Do not use main as a personal development branch.

## 9. Scope-stop checklist for every member or coding agent
Before adding anything, ask:
- Is this required by SYSTEM_SPECIFICATION.md?
- Is it part of my assigned responsibility?
- Does an existing table/service/page already cover it?
- Would it change another member's contract?
- Would it add a new role, course, certificate, table, or workflow?
- Would it change a fixed business rule?
- Does it modify a shared file?

If a proposed change expands scope or affects another member, stop and coordinate first.

## 10. Global completion expectation
A member's work is not complete merely because the page opens. Their section must:
- Use the real local database where required.
- Enforce relevant authorization.
- Validate input.
- Handle normal failure cases.
- Preserve the agreed business rules.
- Build without errors.
- Integrate with the other sections.
- Be understandable enough for the member to explain during assessment.
- Contain no placeholder-only behavior for required functions.

The goal is one integrated CyberLearner system, not four unrelated mini-projects.
