
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


INSERT INTO Users (FullName, Email, Username, PasswordHash, Role, AccountStatus)
VALUES (N'System Manager', N'admin@cyberlearner.com', N'admin', N'P5ocW+J9QIahssPU5fYHGA==:C0s9A+lVjG0QtOkU3mcx3j2fpVf8kwyO4pGsEGTf7p8=', N'Management', N'Active');

INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status)
VALUES (N'Introduction to Cybersecurity',
N'<h3>What is cybersecurity?</h3>
<p>Cybersecurity is the practice of protecting computers, networks, programs and data from attacks, damage and unauthorised access. Today almost everything we do depends on computers and the internet, such as banking, studying, shopping and communicating. Because of this, keeping information safe is important for every person and every organisation.</p>
<figure>
<img src="/Images/Learning/cia-triad.svg" alt="The CIA triad diagram" />
<figcaption>Figure 1: The CIA triad</figcaption>
</figure>
<h3>The CIA triad</h3>
<p>The three main goals of cybersecurity are known as the CIA triad.</p>
<ul>
<li><strong>Confidentiality</strong> means that information is only seen by people who are allowed to see it. Encryption and passwords help to keep data confidential.</li>
<li><strong>Integrity</strong> means that information is correct and has not been changed by someone without permission.</li>
<li><strong>Availability</strong> means that systems and data can be used by authorised users whenever they need them.</li>
</ul>
<h3>Important terms</h3>
<ul>
<li>A <strong>threat</strong> is anything that can cause harm to a system, for example a hacker or a virus.</li>
<li>A <strong>vulnerability</strong> is a weakness in a system that a threat can use.</li>
<li>A <strong>risk</strong> is the chance that a threat will use a vulnerability and cause damage.</li>
</ul>
<h3>Watch: Cybersecurity in 7 minutes</h3>
<div class="video-box">
<iframe src="https://www.youtube.com/embed/inWWhr5tnEA" title="Cybersecurity in 7 minutes" allowfullscreen></iframe>
</div>
<p>After reading this topic and watching the video, take the topic quiz to unlock the next topic.</p>', 1, N'Active');

INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status)
VALUES (N'Common Cyber Threats and Malware',
N'<h3>What is malware?</h3>
<p>Malware is short for malicious software. It is any program that is written to harm a computer, steal data or give an attacker control of a device. Malware usually enters a computer through email attachments, fake downloads, infected USB drives or unsafe websites.</p>
<figure>
<img src="/Images/Learning/malware-types.svg" alt="Common types of malware" />
<figcaption>Figure 2: Common types of malware</figcaption>
</figure>
<h3>Types of malware</h3>
<ul>
<li>A <strong>virus</strong> attaches itself to a file or program and spreads when the user opens that file.</li>
<li>A <strong>worm</strong> spreads by itself across a network without the user doing anything.</li>
<li>A <strong>trojan</strong> looks like a useful program but secretly does something harmful.</li>
<li><strong>Ransomware</strong> encrypts the files of the victim and asks for payment to unlock them.</li>
<li><strong>Spyware</strong> secretly records what the user does, such as the keys that are typed.</li>
</ul>
<h3>Other common attacks</h3>
<p>A <strong>Denial of Service (DoS)</strong> attack sends so many requests to a server that real users cannot use it. This attacks the availability of the system. A <strong>man in the middle</strong> attack happens when an attacker secretly sits between two parties and reads or changes their messages.</p>
<h3>How to protect yourself</h3>
<ol>
<li>Install antivirus software and keep it updated.</li>
<li>Update the operating system and applications regularly.</li>
<li>Do not open attachments or links from unknown senders.</li>
<li>Keep a backup of important files in another place.</li>
</ol>', 2, N'Active');

INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status)
VALUES (N'Passwords and Authentication',
N'<h3>What is authentication?</h3>
<p>Authentication is the process of proving who you are before you are given access to a system. The most common method is a username and password, but there are other methods as well.</p>
<h3>Authentication factors</h3>
<ul>
<li><strong>Something you know</strong>, for example a password or a PIN.</li>
<li><strong>Something you have</strong>, for example a phone or a smart card.</li>
<li><strong>Something you are</strong>, for example a fingerprint or face scan.</li>
</ul>
<p>When two different factors are used together it is called <strong>two factor authentication (2FA)</strong>. Even if an attacker steals the password, they still cannot log in without the second factor.</p>
<figure>
<img src="/Images/Learning/password-strength.svg" alt="Weak and strong password examples" />
<figcaption>Figure 3: Weak and strong passwords</figcaption>
</figure>
<h3>Strong passwords</h3>
<p>A strong password is long and hard to guess. Good practice is to:</p>
<ul>
<li>Use at least 12 characters.</li>
<li>Mix upper case letters, lower case letters, numbers and symbols.</li>
<li>Avoid names, birthdays and common words such as password or 123456.</li>
<li>Use a different password for every account.</li>
<li>Use a password manager to remember them.</li>
</ul>
<h3>Password attacks</h3>
<p>In a <strong>brute force attack</strong> the attacker tries every possible combination until the correct password is found. In a <strong>dictionary attack</strong> the attacker tries a list of common words and passwords.</p>
<h3>How websites store passwords</h3>
<p>A secure website never stores the real password. It stores a <strong>hash</strong>, which is a one way value calculated from the password. A random value called a <strong>salt</strong> is added so that two users with the same password get different hashes.</p>', 3, N'Active');

INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status)
VALUES (N'Phishing and Social Engineering',
N'<h3>What is social engineering?</h3>
<p>Social engineering is when an attacker tricks a person into giving away information or doing something unsafe. Instead of attacking the computer, the attacker attacks the human. People are often the weakest link in security.</p>
<h3>Phishing</h3>
<p>Phishing is the most common type of social engineering. The attacker sends a fake email or message that looks like it comes from a real company such as a bank. The message asks the victim to click a link and enter their login details on a fake website.</p>
<figure>
<img src="/Images/Learning/phishing-email.svg" alt="Example of a phishing email with warning signs" />
<figcaption>Figure 4: Warning signs in a phishing email</figcaption>
</figure>
<h3>Types of phishing</h3>
<ul>
<li><strong>Spear phishing</strong> targets one specific person or company using personal details.</li>
<li><strong>Smishing</strong> is phishing through SMS text messages.</li>
<li><strong>Vishing</strong> is phishing through voice calls.</li>
</ul>
<h3>Other social engineering tricks</h3>
<ul>
<li><strong>Pretexting</strong> is when the attacker invents a story, for example pretending to be IT support.</li>
<li><strong>Baiting</strong> is when the attacker leaves something attractive, such as a free USB drive that contains malware.</li>
<li><strong>Tailgating</strong> is when someone follows an employee through a secure door without permission.</li>
</ul>
<h3>How to spot a phishing email</h3>
<ol>
<li>Check the sender address carefully.</li>
<li>Look for spelling mistakes and urgent language.</li>
<li>Hover over links to see the real address before clicking.</li>
<li>Never send passwords by email.</li>
<li>If you are not sure, contact the company using their official website.</li>
</ol>', 4, N'Active');

INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status)
VALUES (N'Network Security Basics',
N'<h3>Why network security matters</h3>
<p>A network connects computers together so they can share data. If the network is not protected, an attacker can read the data that travels through it or get into the devices that are connected to it.</p>
<figure>
<img src="/Images/Learning/firewall.svg" alt="A firewall between the internet and a private network" />
<figcaption>Figure 5: A firewall protecting a private network</figcaption>
</figure>
<h3>Firewalls</h3>
<p>A firewall is hardware or software that checks the traffic going in and out of a network. It uses rules to decide which traffic is allowed and which traffic is blocked.</p>
<h3>Encryption and HTTPS</h3>
<p>Encryption changes readable data into unreadable data so that only someone with the correct key can read it. Websites that use <strong>HTTPS</strong> encrypt the data between the browser and the server. You can see a padlock icon in the address bar when HTTPS is used.</p>
<h3>VPN</h3>
<p>A Virtual Private Network (VPN) creates an encrypted tunnel between your device and another network. It is useful when you are using public Wi-Fi.</p>
<h3>Wi-Fi security</h3>
<ul>
<li>Use WPA2 or WPA3 encryption on your home router.</li>
<li>Change the default administrator password of the router.</li>
<li>Avoid doing online banking on public Wi-Fi.</li>
</ul>
<h3>Updates and patches</h3>
<p>Software companies release patches to fix vulnerabilities. Installing updates quickly is one of the easiest ways to stay safe because many attacks use old vulnerabilities that already have a fix.</p>
<p>This is the last topic. After passing this quiz you can go to the final certification exam.</p>', 5, N'Active');

INSERT INTO TopicQuizQuestions (TopicID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer, Status) VALUES
(1, N'What does CIA stand for in cybersecurity?', N'Control, Integrity, Access', N'Confidentiality, Integrity, Availability', N'Computer, Internet, Application', N'Confidentiality, Identity, Authentication', 'B', N'Active'),
(1, N'Which goal makes sure that data is not changed without permission?', N'Integrity', N'Availability', N'Confidentiality', N'Authentication', 'A', N'Active'),
(1, N'A weakness in a system that can be used by an attacker is called a', N'Threat', N'Risk', N'Vulnerability', N'Firewall', 'C', N'Active'),
(1, N'Which goal is affected when a website goes offline because of an attack?', N'Confidentiality', N'Integrity', N'Privacy', N'Availability', 'D', N'Active'),
(1, N'What is the main purpose of cybersecurity?', N'To make computers faster', N'To protect systems and data from attacks', N'To design websites', N'To store more files', 'B', N'Active'),

(2, N'What is malware?', N'Software written to cause harm', N'A type of hardware', N'A safe browser plugin', N'A network cable', 'A', N'Active'),
(2, N'Which malware encrypts files and asks for payment?', N'Spyware', N'Worm', N'Ransomware', N'Adware', 'C', N'Active'),
(2, N'Which malware can spread across a network by itself?', N'Trojan', N'Worm', N'Virus', N'Spyware', 'B', N'Active'),
(2, N'A program that looks useful but secretly does something harmful is a', N'Firewall', N'Patch', N'Worm', N'Trojan', 'D', N'Active'),
(2, N'What is the aim of a Denial of Service attack?', N'To make a service unavailable to real users', N'To steal passwords', N'To encrypt files', N'To read emails', 'A', N'Active'),

(3, N'Which of these is the strongest password?', N'password123', N'john1999', N'T7#kLp!29xQz', N'qwerty', 'C', N'Active'),
(3, N'What is two factor authentication?', N'Using two passwords', N'Using two different types of proof to log in', N'Logging in twice', N'Having two user accounts', 'B', N'Active'),
(3, N'A fingerprint is an example of', N'Something you are', N'Something you know', N'Something you have', N'Something you share', 'A', N'Active'),
(3, N'How should a secure website store passwords?', N'As plain text', N'In an email', N'In a text file on the desktop', N'As a salted hash', 'D', N'Active'),
(3, N'An attack that tries every possible password combination is called', N'Phishing', N'Brute force', N'Spoofing', N'Tailgating', 'B', N'Active'),

(4, N'What is phishing?', N'A type of firewall', N'A way to speed up a network', N'A fake message that tricks people into giving information', N'A backup method', 'C', N'Active'),
(4, N'Phishing that is sent by SMS is called', N'Smishing', N'Vishing', N'Baiting', N'Pharming', 'A', N'Active'),
(4, N'Which of these is a warning sign of a phishing email?', N'It comes from a colleague you know', N'It has no links', N'It uses your correct full name', N'It asks you to act urgently and click a link', 'D', N'Active'),
(4, N'Following an employee through a secure door without permission is called', N'Pretexting', N'Tailgating', N'Spear phishing', N'Encryption', 'B', N'Active'),
(4, N'What should you do if you receive a suspicious email from your bank?', N'Click the link to check', N'Reply with your password', N'Contact the bank using the official website', N'Forward it to all your friends', 'C', N'Active'),

(5, N'What does a firewall do?', N'Filters network traffic using rules', N'Makes the internet faster', N'Stores passwords', N'Removes dust from the computer', 'A', N'Active'),
(5, N'What does HTTPS provide?', N'Faster downloads', N'Free Wi-Fi', N'More storage', N'Encrypted communication between browser and server', 'D', N'Active'),
(5, N'What is a VPN used for?', N'Scanning for viruses', N'Creating an encrypted tunnel to another network', N'Cleaning the hard disk', N'Printing documents', 'B', N'Active'),
(5, N'Which Wi-Fi security standard is the most secure?', N'WEP', N'Open network', N'WPA3', N'WPA', 'C', N'Active'),
(5, N'Why should software be updated regularly?', N'Updates fix known vulnerabilities', N'Updates make the screen bigger', N'Updates delete old files', N'Updates are only for new colours', 'A', N'Active');

INSERT INTO ExamQuestions (QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer, Status) VALUES
(N'Which part of the CIA triad is about keeping information secret from unauthorised people?', N'Integrity', N'Confidentiality', N'Availability', N'Accountability', 'B', N'Active'),
(N'Which part of the CIA triad makes sure that systems can be used when needed?', N'Availability', N'Integrity', N'Confidentiality', N'Authentication', 'A', N'Active'),
(N'The chance that a threat will use a vulnerability and cause damage is called', N'An exploit', N'A patch', N'A risk', N'A backup', 'C', N'Active'),
(N'Anything that can cause harm to a system is called a', N'Policy', N'Hash', N'Protocol', N'Threat', 'D', N'Active'),
(N'Which type of malware needs the user to open an infected file to spread?', N'Virus', N'Worm', N'Firewall', N'Router', 'A', N'Active'),
(N'Which malware secretly records what the user types?', N'Ransomware', N'Spyware', N'Worm', N'Trojan', 'B', N'Active'),
(N'What is the best protection against losing files to ransomware?', N'Using a bigger monitor', N'Turning off the firewall', N'Keeping regular backups', N'Sharing passwords', 'C', N'Active'),
(N'An attacker who secretly reads messages between two parties is doing a', N'Brute force attack', N'Dictionary attack', N'Denial of Service attack', N'Man in the middle attack', 'D', N'Active'),
(N'Which of these is an example of something you have?', N'A password', N'A smart card', N'A fingerprint', N'A PIN', 'B', N'Active'),
(N'Why is a salt added before hashing a password?', N'So the same password gives different hashes', N'To make the password shorter', N'To show the password to the user', N'To make login faster', 'A', N'Active'),
(N'An attack that tries a list of common words as passwords is called', N'Phishing', N'Tailgating', N'A dictionary attack', N'Baiting', 'C', N'Active'),
(N'Which is the best habit for managing many passwords?', N'Use the same password everywhere', N'Write them on a sticky note', N'Tell them to a friend', N'Use a password manager', 'D', N'Active'),
(N'Phishing that targets one specific person using personal details is called', N'Spear phishing', N'Smishing', N'Vishing', N'Spamming', 'A', N'Active'),
(N'Phishing done through a phone call is called', N'Smishing', N'Vishing', N'Baiting', N'Pretexting', 'B', N'Active'),
(N'Leaving an infected USB drive for someone to find is an example of', N'Tailgating', N'Encryption', N'Baiting', N'Hashing', 'C', N'Active'),
(N'Social engineering mainly attacks', N'The firewall', N'The network cable', N'The operating system', N'The human user', 'D', N'Active'),
(N'What should you check before clicking a link in an email?', N'The real address the link goes to', N'The colour of the link', N'The size of the email', N'The time it was sent', 'A', N'Active'),
(N'Which device or software blocks unwanted network traffic?', N'Printer', N'Firewall', N'Scanner', N'Keyboard', 'B', N'Active'),
(N'Which sign in the browser shows a website is using HTTPS?', N'A star icon', N'A red banner', N'A padlock icon', N'A download arrow', 'C', N'Active'),
(N'Encryption is used to', N'Delete data', N'Compress data', N'Copy data', N'Make data unreadable without the key', 'D', N'Active'),
(N'What should be changed first on a new home router?', N'The default administrator password', N'The colour of the lights', N'The length of the cable', N'The brand name', 'A', N'Active'),
(N'Why is public Wi-Fi risky?', N'It is always slow', N'Other people on the network may intercept your data', N'It uses too much battery', N'It blocks all websites', 'B', N'Active'),
(N'A software update that fixes a security weakness is called a', N'Cookie', N'Worm', N'Patch', N'Salt', 'C', N'Active'),
(N'Which of the following is NOT a type of malware?', N'Trojan', N'Ransomware', N'Spyware', N'Firewall', 'D', N'Active'),
(N'Using a password together with a code sent to your phone is an example of', N'Two factor authentication', N'A brute force attack', N'A dictionary attack', N'Social engineering', 'A', N'Active');
GO
