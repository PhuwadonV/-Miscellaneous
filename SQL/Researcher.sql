create table Staff(
	ID int AUTO_INCREMENT primary key,
	Permission int not null unique,
	Username nvarchar(50) not null,
	Password nvarchar(50) not null,
	Email nvarchar(100)
);

create table Researcher(
	ID int AUTO_INCREMENT primary key,
	Name nvarchar(80) not null,
	Lastname nvarchar(80) not null,
	Email nvarchar(100),
	Phone nvarchar(20),
	Faculty nvarchar(80)
);

create table Assessor(
	ID int AUTO_INCREMENT primary key,
	Name nvarchar(80) not null,
	Lastname nvarchar(80) not null,
	Email nvarchar(100),
	Phone nvarchar(20),
	BankAccountNumber nvarchar(10)
);

create table Project(
	ID int AUTO_INCREMENT primary key,
	ProjectNumber int,
	Name_TH nvarchar(200),
	Name_EN nvarchar(200),
	LeaderID int null,
	AcademicYear smallint,
	SubmitRound tinyint,
	Budget int,
	AssessorID int null,
	Approve bit,
	TypeOfBudget nvarchar(100),
	BeginProject date,
	EndProject date,
	PaperDeadLine date,
	FilePath nvarchar(200),
	unique (ProjectNumber, AcademicYear),
	foreign key (LeaderID) references Researcher(ID),
	foreign key (AssessorID) references Assessor(ID)
);

create table Instalment(
	ProjectID int not null,
	Number tinyint,
	Amount int,
	WithdrawDate date,
	WithdrawAmount int,
	ReportDate date,
	FilePath nvarchar(200),
	primary key (ProjectID, Number),
	foreign key (ProjectID) references Project(ID)
);

create view ResearcherView as
	select ID, Name, Lastname, Email, Phone, Faculty from Researcher;
	
create view AssessorView as
	select ID, Name, Lastname, Email, Phone, BankAccountNumber from Assessor;
	
create view InstalmentsView as
	select ProjectID,
	Group_Concat(Number Separator ',') as Number,
	Group_Concat(Amount Separator ',') as Amount,
	Group_Concat(WithDrawDate Separator ',') as WithdrawDate,
	Group_Concat(WithDrawAmount Separator ',') as WithdrawAmount,
	Group_Concat(Reportdate Separator ',') as ReportDate
	from (select * from Instalment order by Number) as OrderedInstalment
	group by ProjectID;
	
create view ProjectView as
	select P.ID, ProjectNumber, Name_TH, Name_EN, AcademicYear,
	SubmitRound, Budget, Approve, TypeOfBudget,
	BeginProject, EndProject, PaperDeadLine,
	RV.ID as ResearcherID, RV.Name as ResearcherName,
	RV.Lastname as ResearcherLastname, RV.Email as ResearcherEmail,
	RV.Phone as ResearcherPhone, RV.Faculty as ResearcherFaculty,
	AV.ID as AssessorID, AV.Name as Assessor,
	AV.Lastname as AssessorLastname, AV.Email as AssessorEmail,
	AV.Phone as AssessorPhone, AV.BankAccountNumber as AssessorBankAccountNumber,
	IV.Number as InstalmentNumbers, IV.Amount as InstalmentAmounts,
	IV.WithdrawDate as InstalmentWithdrawDates,
	IV.WithdrawAmount as InstalmentWithdrawAmounts,
	IV.ReportDate as InstalmentReportDates
	from Project as P
	left join ResearcherView as RV on P.LeaderID = RV.ID
	left join AssessorView as AV on P.AssessorID = AV.ID
	left join InstalmentsView IV on IV.ProjectID = P.ID;