use master
go
drop database if exists DictionaryDB
go
create database DictionaryDB
go
use DictionaryDB
go
create table LanguageTable(
	ID int identity(1,1) not null primary key,
	Language nvarchar(50) not null,
	unique(Language))
go
create table WordTable (
	ID int identity(1,1) not null primary key,
	Word nvarchar(100) not null,
	LanguageID int not null foreign key references LanguageTable(ID)
	unique(Word, LanguageID))
go
create table WordMeanningTable (
	ID int identity(1,1) not null primary key,
	WordID int not null foreign key references WordTable(ID),
	Meanning nvarchar(500),
	unique(WordID, Meanning))
go
create table RelatedWordTable (
	WordMeanningID int not null foreign key references WordMeanningTable(ID),
	WordID int not null foreign key references WordTable(ID),
	primary key(WordMeanningID, WordID))