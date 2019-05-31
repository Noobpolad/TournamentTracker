using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.Connectors
{
    public class SQLConnector : IDataConnection
    {
        private const string DataBase = "Tournaments";

        /// <summary>
        /// Save the new person to the database
        /// </summary>
        /// <param name="pm"></param>
        public void CreatePerson(PersonModel pm)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", pm.FirstName);
                p.Add("@LastName", pm.LastName);
                p.Add("@Email", pm.EmailAddress);
                p.Add("@CellphoneNumber", pm.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                pm.id = p.Get<int>("@id");
            }
        }

        /// <summary>
        /// Save the new prize to the database
        /// </summary>
        /// <param name="pm"></param>
        public void CreatePrize(PrizeModel pm)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", pm.PlaceNumber);
                p.Add("@PlaceName", pm.PlaceName);
                p.Add("@PrizeAmount", pm.PrizeAmount);
                p.Add("@PrizePercentage", pm.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                pm.id = p.Get<int>("@id");
            }
        }

        /// <summary>
        /// Save the Team to the database
        /// </summary>
        /// <param name="tm"></param>
        public void CreateTeam(TeamModel tm)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", tm.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeamsInsert", p, commandType: CommandType.StoredProcedure);

                tm.id = p.Get<int>("@id");

                foreach (PersonModel pm in tm.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", tm.id);
                    p.Add("@PersonId", pm.id);
                    connection.Execute("dbo.spTeamMembersInsert", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        /// <summary>
        /// Save the Tournament to database
        /// </summary>
        /// <param name="model"></param>
        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                SaveTournament(connection, model);
                SaveTournamentPrizes(connection, model);
                SaveTournamentEntries(connection, model);
                SaveTournamentRounds(connection, model);
                TournamentLogic.UpdateTournamentResult(model);
            }
        }

        /// <summary>
        /// Save rounds of tournament
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="model"></param>
        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@MatchupRound", matchup.MatchUpRound);
                    p.Add("@TournamentId", model.id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchup_Insert", p, commandType: CommandType.StoredProcedure);

                    matchup.id = p.Get<int>("@id");
                    foreach (MatchUpEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.id);
                        if (entry.ParentMatch != null)
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatch.id);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        if (entry.TeamCompeting != null)
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.id);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", null);
                        }

                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntry_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }

        /// <summary>
        /// Save the raw Tournament
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="model"></param>
        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);

            model.id = p.Get<int>("@id");
        }

        /// <summary>
        /// Save prizes of tournament
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="model"></param>
        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel pm in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.id);
                p.Add("@PrizeId", pm.id);
                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Save entries of tournament
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="model"></param>
        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel team in model.Teams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.id);
                p.Add("@TeamId", team.id);
                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Connects to the database and gets all the tournaments
        /// </summary>
        /// <returns>List of tournaments</returns>
        public List<TournamentModel> GetTournamentsAll()
        {
            List<TournamentModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();

                output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").ToList();

                foreach (TournamentModel m in output)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", m.id);
                    m.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    p = new DynamicParameters();
                    p.Add("@TournamentId", m.id);
                    m.Teams = connection.Query<TeamModel>("dbo.spTeams_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    foreach (TeamModel t in m.Teams)
                    {
                        p = new DynamicParameters();
                        p.Add("@TeamId", t.id);
                        t.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                    }

                    p = new DynamicParameters();
                    p.Add("@TournamentId", m.id);
                    List<MatchUpModel> matchups = connection.Query<MatchUpModel>("dbo.spMatchup_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    foreach (MatchUpModel mm in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", mm.id);
                        mm.Entries = connection.Query<MatchUpEntryModel>("dbo.spMatchupEntry_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();

                        List<TeamModel> allTeams = GetTeamAll();

                        if (mm.WinnerId > 0)
                        {
                            mm.Winner = allTeams.Where(x => x.id == mm.WinnerId).First();
                        }

                        foreach (MatchUpEntryModel me in mm.Entries)
                        {
                            if (me.TeamCompetingId > 0)
                            {
                                me.TeamCompeting = allTeams.Where(x => x.id == me.TeamCompetingId).First();
                            }
                            if (me.ParentMatchupId > 0)
                            {
                                me.ParentMatch = matchups.Where(x => x.id == me.ParentMatchupId).First();
                            }
                        }
                    }

                    List<MatchUpModel> currRow = new List<MatchUpModel>();
                    int currRound = 1;

                    foreach (MatchUpModel mm in matchups)
                    {
                        if (mm.MatchUpRound > currRound)
                        {
                            currRound++;
                            m.Rounds.Add(currRow);
                            currRow = new List<MatchUpModel>();
                        }
                        currRow.Add(mm);
                    }
                    m.Rounds.Add(currRow);
                }

            }
            return output;
        }

        /// <summary>
        /// Connects to the database and gets all the Persons
        /// </summary>
        /// <returns>List of People</returns>
        public List<PersonModel> GetPersonAll()
        {
            List<PersonModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                output = connection.Query<PersonModel>("dbo.spPeopleGetAll").ToList();
            }
            return output;
        }

        /// <summary>
        /// Connects to the database and returns Teams
        /// </summary>
        /// <returns>List of Teams</returns>
        public List<TeamModel> GetTeamAll()
        {
            List<TeamModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();
                foreach (TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        /// <summary>
        /// Update the matchup if needed
        /// </summary>
        /// <param name="m"></param>
        public void UpdateMatchup(MatchUpModel m)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();
                if (m.Winner != null)
                {
                    p.Add("@id", m.id);
                    p.Add("@WinnerId", m.Winner.id);

                    connection.Execute("dbo.spMatchups_Update", p, commandType: CommandType.StoredProcedure); 
                }

                foreach (MatchUpEntryModel entry in m.Entries)
                {
                    if (entry.TeamCompeting != null)
                    {
                        p = new DynamicParameters();
                        p.Add("@id", entry.id);
                        p.Add("@TeamCompetingId", entry.TeamCompeting.id);
                        p.Add("@Score", entry.Score);

                        connection.Execute("dbo.spMatchupEntries_Update", p, commandType: CommandType.StoredProcedure);
                    }
                }

            }
        }

        /// <summary>
        /// Mark the tournament as completed
        /// </summary>
        /// <param name="tm"></param>
        public void CompleteTournament(TournamentModel tm)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(DataBase)))
            {
                var p = new DynamicParameters();
                p.Add("@id", tm.id);
                connection.Execute("dbo.spToutnaments_Complete", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}