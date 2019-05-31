using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.Connectors.TextHelper;

namespace TrackerLibrary.Connectors
{
    class TextFileConnector : IDataConnection
    {
        /// <summary>
        /// Delete the completed tournament from the tournaments file
        /// </summary>
        /// <param name="tm"></param>
        public void CompleteTournament(TournamentModel tm)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentsFileName.FullFilePath().LoadFile().ConvertToTournaments().Where(x => x.id != tm.id).ToList();
            
            tournaments.SaveToTournamentFile();
        }

        /// <summary>
        /// Save the new human to the txt file
        /// </summary>
        /// <param name="pm"></param>
        /// <returns>The human information</returns>
        public void CreatePerson(PersonModel pm)
        {
            List<PersonModel> listOfPeople = GlobalConfig.PeopleFileName.FullFilePath().LoadFile().ConvertToPeople();

            int curIndex = 1;

            if (listOfPeople.Count > 0)
            {
                curIndex = listOfPeople.OrderByDescending(p => p.id).First().id + 1;
            }

            pm.id = curIndex;

            listOfPeople.Add(pm);

            listOfPeople.SaveToPeopleFile();
        }

        /// <summary>
        /// Save the new prize to the txt file
        /// </summary>
        /// <param name="pm"></param>
        /// <returns>The prize information</returns>
        public void CreatePrize(PrizeModel pm)
        {
            List<PrizeModel> listOfPrizes = GlobalConfig.PrizeFileName.FullFilePath().LoadFile().ConvertToPrizes();

            int curIndex = 1;

            if (listOfPrizes.Count > 0)
            {
                curIndex = listOfPrizes.OrderByDescending(p => p.id).First().id + 1;
            }

            pm.id = curIndex;

            listOfPrizes.Add(pm);

            listOfPrizes.SaveToPrizeFile();
        }

        /// <summary>
        /// Save the team to the text file
        /// </summary>
        /// <param name="tm"></param>
        /// <returns>Comleted, last created team</returns>
        public void CreateTeam(TeamModel tm)
        {
            List<TeamModel> listOfTeams = GlobalConfig.TeamsFileName.FullFilePath().LoadFile().ConvertToTeams();

            int maxId = 1;
            if (listOfTeams.Count > 0)
            {
                maxId = listOfTeams.OrderByDescending(x => x.id).First().id + 1;
            }
            tm.id = maxId;
            listOfTeams.Add(tm);

            listOfTeams.SaveToTeamFile();
        }

        /// <summary>
        /// Save tournaments and rounds info to files
        /// </summary>
        /// <param name="tm"></param>
        public void CreateTournament(TournamentModel tm)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentsFileName.FullFilePath().LoadFile().ConvertToTournaments();
            int id = 1;

            if (tournaments.Count != 0)
            {
                id = tournaments.OrderByDescending(x => x.id).First().id + 1;
            }

            tm.id = id;

            tm.SaveRoundsToFile();

            tournaments.Add(tm);

            tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResult(tm);
        }

        /// <summary>
        /// Connects to the database and gets all the Persons
        /// </summary>
        /// <returns>List of People</returns>
        public List<PersonModel> GetPersonAll()
        {
            return GlobalConfig.PeopleFileName.FullFilePath().LoadFile().ConvertToPeople();
        }

        /// <summary>
        /// Connects to the database and returns Teams
        /// </summary>
        /// <returns>List of Teams</returns>
        public List<TeamModel> GetTeamAll()
        {
            return GlobalConfig.TeamsFileName.FullFilePath().LoadFile().ConvertToTeams();
        }

        /// <summary>
        /// Conncets to the database and returns all the Tournaments
        /// </summary>
        /// <returns></returns>
        public List<TournamentModel> GetTournamentsAll()
        {
            return GlobalConfig.TournamentsFileName.FullFilePath().LoadFile().ConvertToTournaments();
        }

        /// <summary>
        /// Update the info of the matchup
        /// </summary>
        /// <param name="m"></param>
        public void UpdateMatchup(MatchUpModel m)
        {
            m.UpdateMatchupToFile();
        }
    }
}
