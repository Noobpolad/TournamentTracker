USE [Tournaments]
GO
/****** Object:  Table [dbo].[MatchupEntries]    Script Date: 5/31/2019 11:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchupEntries](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MatchupId] [int] NOT NULL,
	[ParentMatchupId] [int] NULL,
	[TeamCompetingId] [int] NULL,
	[Score] [float] NULL,
 CONSTRAINT [PK_MatchupEntries] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Matchups]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matchups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[WinnerId] [int] NULL,
	[MatchupRound] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
 CONSTRAINT [PK_Matchups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[People]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nchar](100) NOT NULL,
	[LastName] [nchar](100) NOT NULL,
	[EmailAddress] [nchar](100) NULL,
	[CellphoneNumber] [nchar](20) NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prizes]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prizes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[PlaceNumber] [int] NOT NULL,
	[PlaceName] [nchar](30) NOT NULL,
	[PrizeAmount] [money] NOT NULL,
	[PrizePercentage] [float] NOT NULL,
 CONSTRAINT [PK_Prizes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamMembers]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamMembers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TeamId] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
 CONSTRAINT [PK_TeamMembers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nchar](100) NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TournamentEntries]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TournamentEntries](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TournamentId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
 CONSTRAINT [PK_TournamentEntries] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TournamentPrizes]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TournamentPrizes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TournamentId] [int] NOT NULL,
	[PrizeId] [int] NOT NULL,
 CONSTRAINT [PK_TournamentPrizes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tournaments]    Script Date: 5/31/2019 11:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tournaments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TournamentName] [nchar](50) NOT NULL,
	[EntryFee] [money] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Tournaments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MatchupEntries]  WITH CHECK ADD  CONSTRAINT [FK_MatchupEntries_Matchups] FOREIGN KEY([MatchupId])
REFERENCES [dbo].[Matchups] ([id])
GO
ALTER TABLE [dbo].[MatchupEntries] CHECK CONSTRAINT [FK_MatchupEntries_Matchups]
GO
ALTER TABLE [dbo].[MatchupEntries]  WITH CHECK ADD  CONSTRAINT [FK_MatchupEntries_ParentMatchupId] FOREIGN KEY([ParentMatchupId])
REFERENCES [dbo].[Matchups] ([id])
GO
ALTER TABLE [dbo].[MatchupEntries] CHECK CONSTRAINT [FK_MatchupEntries_ParentMatchupId]
GO
ALTER TABLE [dbo].[MatchupEntries]  WITH CHECK ADD  CONSTRAINT [FK_MatchupEntries_Teams] FOREIGN KEY([TeamCompetingId])
REFERENCES [dbo].[Teams] ([id])
GO
ALTER TABLE [dbo].[MatchupEntries] CHECK CONSTRAINT [FK_MatchupEntries_Teams]
GO
ALTER TABLE [dbo].[Matchups]  WITH CHECK ADD  CONSTRAINT [FK_Matchups_Teams] FOREIGN KEY([WinnerId])
REFERENCES [dbo].[Teams] ([id])
GO
ALTER TABLE [dbo].[Matchups] CHECK CONSTRAINT [FK_Matchups_Teams]
GO
ALTER TABLE [dbo].[Matchups]  WITH CHECK ADD  CONSTRAINT [FK_Matchups_Tournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[Tournaments] ([id])
GO
ALTER TABLE [dbo].[Matchups] CHECK CONSTRAINT [FK_Matchups_Tournament]
GO
ALTER TABLE [dbo].[TeamMembers]  WITH CHECK ADD  CONSTRAINT [FK_TeamMembers_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([id])
GO
ALTER TABLE [dbo].[TeamMembers] CHECK CONSTRAINT [FK_TeamMembers_People]
GO
ALTER TABLE [dbo].[TeamMembers]  WITH CHECK ADD  CONSTRAINT [FK_TeamMembers_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([id])
GO
ALTER TABLE [dbo].[TeamMembers] CHECK CONSTRAINT [FK_TeamMembers_Teams]
GO
ALTER TABLE [dbo].[TournamentEntries]  WITH CHECK ADD  CONSTRAINT [FK_TournamentEntries_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([id])
GO
ALTER TABLE [dbo].[TournamentEntries] CHECK CONSTRAINT [FK_TournamentEntries_Teams]
GO
ALTER TABLE [dbo].[TournamentEntries]  WITH CHECK ADD  CONSTRAINT [FK_TournamentEntries_Tournaments] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[Tournaments] ([id])
GO
ALTER TABLE [dbo].[TournamentEntries] CHECK CONSTRAINT [FK_TournamentEntries_Tournaments]
GO
ALTER TABLE [dbo].[TournamentPrizes]  WITH CHECK ADD  CONSTRAINT [FK_TournamentPrizes_PrizeId] FOREIGN KEY([PrizeId])
REFERENCES [dbo].[Prizes] ([id])
GO
ALTER TABLE [dbo].[TournamentPrizes] CHECK CONSTRAINT [FK_TournamentPrizes_PrizeId]
GO
ALTER TABLE [dbo].[TournamentPrizes]  WITH CHECK ADD  CONSTRAINT [FK_TournamentPrizes_Tournaments] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[Tournaments] ([id])
GO
ALTER TABLE [dbo].[TournamentPrizes] CHECK CONSTRAINT [FK_TournamentPrizes_Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spMatchup_GetByTournament]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchup_GetByTournament]
	@TournamentId int
AS
BEGIN
	SET NOCOUNT ON;

	select m.* from dbo.Matchups m where m.TournamentId = @TournamentId order by MatchupRound;

END
GO
/****** Object:  StoredProcedure [dbo].[spMatchup_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchup_Insert]
	@MatchupRound int,
	@TournamentId int,
	@id int = 0 output
AS
BEGIN

	SET NOCOUNT ON;

	insert into dbo.Matchups(MatchupRound,TournamentId)
	values(@MatchupRound,@TournamentId);

	select @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spMatchupEntries_Update]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchupEntries_Update]
	@id int,
	@TeamCompetingId int = null,
	@Score float = null
AS
BEGIN

	SET NOCOUNT ON;

   update dbo.MatchupEntries set Score = @Score, TeamCompetingId = @TeamCompetingId where id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[spMatchupEntry_GetByMatchup]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[spMatchupEntry_GetByMatchup]
	@MatchupId int
AS
BEGIN
	SET NOCOUNT ON;

	select * from dbo.MatchupEntries where MatchupId = @MatchupId; 

END
GO
/****** Object:  StoredProcedure [dbo].[spMatchupEntry_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchupEntry_Insert]
	@MatchupId int,
	@ParentMatchupId int,
	@TeamCompetingId int,
	@id int = 0 output
AS
BEGIN

	SET NOCOUNT ON;

    insert into dbo.MatchupEntries(MatchupId,ParentMatchupId,TeamCompetingId)
	values (@MatchupId,@ParentMatchupId,@TeamCompetingId);

	select @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spMatchups_Update]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchups_Update]
	@id int,
	@WinnerId int
AS
BEGIN

	SET NOCOUNT ON;

   update dbo.Matchups set WinnerId = @WinnerId where id = @id; 
END
GO
/****** Object:  StoredProcedure [dbo].[spPeopleGetAll]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPeopleGetAll]
AS
BEGIN

	SET NOCOUNT ON;

    select id,TRIM(FirstName) as FirstName,TRIM(LastName) as LastName,TRIM(EmailAddress) as EmailAddress,TRIM(CellphoneNumber) as CellphoneNumber from dbo.People;
END
GO
/****** Object:  StoredProcedure [dbo].[spPerson_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPerson_Insert]
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@Email nvarchar(100),
	@CellphoneNumber nvarchar(100),
	@id int = 0 output

AS
BEGIN

	SET NOCOUNT ON;

    insert into dbo.People (FirstName, LastName, EmailAddress, CellphoneNumber)
	values (@FirstName, @LastName, @Email, @CellphoneNumber);

	select @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spPrizes_GetByTournament]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPrizes_GetByTournament]
	@TournamentId int
AS
BEGIN
	SET NOCOUNT ON;

	select p.* from dbo.Prizes p inner join dbo.TournamentPrizes tp on p.id = tp.PrizeId where tp.TournamentId = @TournamentId;

END
GO
/****** Object:  StoredProcedure [dbo].[spPrizes_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPrizes_Insert] 
	@PlaceNumber int,
	@PlaceName nvarchar(50),
	@PrizeAmount money,
	@PrizePercentage float,
	@id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

    insert into dbo.Prizes (PlaceNumber, PlaceName, PrizeAmount, PrizePercentage)
	values (@PlaceNumber, @PlaceName, @PrizeAmount, @PrizePercentage);

	select @id = SCOPE_IDENTITY();

END
GO
/****** Object:  StoredProcedure [dbo].[spTeam_GetAll]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeam_GetAll]
AS
BEGIN

	SET NOCOUNT ON;

    select * from dbo.Teams;

END
GO
/****** Object:  StoredProcedure [dbo].[spTeamMembers_GetByTeam]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeamMembers_GetByTeam]
	@TeamId int
AS
BEGIN

	SET NOCOUNT ON;

	select * from dbo.TeamMembers m inner join dbo.People p on m.PersonId = p.id where m.TeamId = @TeamId;

END
GO
/****** Object:  StoredProcedure [dbo].[spTeamMembersInsert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeamMembersInsert]
	@TeamId int,
	@PersonId int,
	@Id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

	insert into dbo.TeamMembers(TeamId,PersonId)
	values (@TeamId,@PersonId)
	select @Id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spTeams_GetByTournament]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[spTeams_GetByTournament]
	@TournamentId int
AS
BEGIN
	SET NOCOUNT ON;

	select t.* from dbo.Teams t inner join dbo.TournamentEntries te on t.id = te.TeamId where te.TournamentId = @TournamentId;

END
GO
/****** Object:  StoredProcedure [dbo].[spTeamsInsert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeamsInsert]
	@TeamName nvarchar(100),
	@Id int = 0 output
AS
BEGIN

	SET NOCOUNT ON;

	insert into dbo.Teams (TeamName)
	values (@TeamName);

	select @Id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spTournament_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTournament_Insert]
	@TournamentName varchar(50),
	@EntryFee money,
	@id int = 0 output

AS
BEGIN
	SET NOCOUNT ON;

    insert into dbo.Tournaments(TournamentName, EntryFee, Active)
	values (@TournamentName,@EntryFee,1);
	select @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spTournamentEntries_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTournamentEntries_Insert]
	@TournamentId int,
	@TeamId int,
	@id int = 0 output
AS
BEGIN

	SET NOCOUNT ON;

	insert into dbo.TournamentEntries(TournamentId,TeamId)
	values(@TournamentId,@TeamId);
	select @id = SCOPE_IDENTITY();

END
GO
/****** Object:  StoredProcedure [dbo].[spTournamentPrizes_Insert]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================

CREATE PROCEDURE [dbo].[spTournamentPrizes_Insert]
	@TournamentId int,
	@PrizeId int,
	@id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

    insert into dbo.TournamentPrizes(TournamentId,PrizeId)
	values(@TournamentId,@PrizeId);
	select @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spTournaments_GetAll]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTournaments_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	select * from dbo.Tournaments where Active = 1;

END
GO
/****** Object:  StoredProcedure [dbo].[spToutnaments_Complete]    Script Date: 5/31/2019 11:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spToutnaments_Complete]
	@id int
AS
BEGIN

	SET NOCOUNT ON;

	update dbo.Tournaments set Active = 0 where id = @id;
    
END
GO
