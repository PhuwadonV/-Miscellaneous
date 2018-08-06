create view WordView as
	select WT.ID as WordID, Word, LT.Language as Language
		from WordTable as WT
		inner join LanguageTable as LT on WT.LanguageID = LT.ID
go
create view WordMeanningView as
	select ID as WordMeanningID, WMT.WordID, WV.Word, Language, Meanning
		from WordMeanningTable as WMT
		inner join WordView as WV on WMT.WordID = WV.WordID
go
create view RelatedWordView as
	select RWMT.WordMeanningID, WMV.Word, WMV.Language, WV.Word as ToWord, WV.Language as ToLanguage
		from RelatedWordTable as RWMT
		inner join WordMeanningView as WMV on RWMT.WordMeanningID = WMV.WordMeanningID
		inner join WordView as WV on RWMT.WordID = WV.WordID
go
create proc SearchLanguage @Word nvarchar(100) as
	select Language from WordView where Word = @Word
go
create proc SearchMeanning @Word nvarchar(100), @Language nvarchar(50) as
	select WordMeanningID, Meanning from WordMeanningView where Word = @Word and Language = @Language
go
create proc SearchToLanguage @Word nvarchar(100), @Language nvarchar(50) as
	select distinct ToLanguage from RelatedWordView where Word = @Word and Language = @Language
go
create proc SearchRelatedWord @Word nvarchar(100), @FromLanguage nvarchar(50), @ToLanguage nvarchar(50) as
	select WordMeanningID, ToWord from RelatedWordView
		where Word = @Word and Language = @FromLanguage and ToLanguage = @ToLanguage
go
create proc AddWordMeanning @Word nvarchar(100), @Language nvarchar(50), @Meanning nvarchar(500) as
	declare @lid int = 0
	declare @wid int = 0
	select top 1 @lid = ID from LanguageTable where Language = @Language
	if @lid = 0 begin
		insert into LanguageTable values(@Language)
		insert into WordTable values(@Word, SCOPE_IDENTITY())
		insert into WordMeanningTable values(SCOPE_IDENTITY(), @Meanning)
	end
	else begin
		select @wid = ID from WordTable where Word = @Word and LanguageID = @lid
		if @wid = 0 begin
			insert into WordTable values(@Word, @lid)
			insert into WordMeanningTable values(SCOPE_IDENTITY(), @Meanning)
		end
		else if not exists (select * from WordMeanningTable where WordID = @wid and Meanning = @Meanning) begin
			insert into WordMeanningTable values(@wid, @Meanning)
		end
	end
go
create proc AddRelatedWord @WordMeanningID int, @RelatedWord nvarchar(100), @RelatedLanguage nvarchar(50) as
declare @lid int = 0
	declare @wid int = 0
	select top 1 @lid = ID from LanguageTable where Language = @RelatedLanguage
	if @lid = 0 begin
		insert into LanguageTable values(@RelatedLanguage)
		insert into WordTable values(@RelatedWord, SCOPE_IDENTITY())
		insert into RelatedWordTable values(@WordMeanningID, SCOPE_IDENTITY())
	end
	else begin
		select @wid = ID from WordTable where Word = @RelatedWord and LanguageID = @lid
		if @wid = 0 begin
			insert into WordTable values(@RelatedWord, @lid)
			insert into RelatedWordTable values(@WordMeanningID, SCOPE_IDENTITY())
		end
		else if not exists (select * from RelatedWordTable where WordMeanningID = @WordMeanningID and WordID = @wid) begin
			insert into RelatedWordTable values(@WordMeanningID, @wid)
		end
	end
go
create proc DeleteWord @Word nvarchar(100), @Language nvarchar(50) as
	declare @wid int
	select @wid = WordID from WordView where Word = @Word and Language = @Language
	delete from RelatedWordTable where WordID = @wid
	delete RWT from RelatedWordTable as RWT 
		inner join WordMeanningTable as WMT on RWT.WordMeanningID = WMT.ID where WMT.WordID = @wid
	delete from WordMeanningTable where WordID = @wid
	delete from WordTable where ID = @wid
go
create proc DeleteRelatedWord @WordMeanningID int, @RelatedWord nvarchar(100), @RelatedLanguage nvarchar(50) as
	declare @wid int
	select @wid = WordID from WordView where Word = @RelatedWord and Language = @RelatedLanguage
	delete from RelatedWordTable where WordID = @wid
go
create proc EditMeanning @WordMeanningID int, @Meanning nvarchar(500) as
	update WordMeanningTable set Meanning = @Meanning where @WordMeanningID = ID