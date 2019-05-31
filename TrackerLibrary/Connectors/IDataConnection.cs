using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.Connectors
{
    public interface IDataConnection
    {
        void CreatePrize(PrizeModel pm);
        void CreatePerson(PersonModel pm);
        void CreateTeam(TeamModel tm);
        void UpdateMatchup(MatchUpModel m);
        void CreateTournament(TournamentModel tm);
        void CompleteTournament(TournamentModel tm);
        List<TeamModel> GetTeamAll();
        List<PersonModel> GetPersonAll();
        List<TournamentModel> GetTournamentsAll();
    }
}
