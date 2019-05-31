using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        public event EventHandler<string> OnTournamentComplete;

        /// <summary>
        /// Unique id for Tournament
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the name of a tournament
        /// </summary>
        private string tn;
        public string TournamentName
        {
            get
            {
                return tn;
            }
            set { tn = value.Trim(); }
        }

        /// <summary>
        /// Represents the entry fee
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Represents the list of teams in the tournament
        /// </summary>
        public List<TeamModel> Teams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// Represents the list of prizes for this tournament
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// Represents the list of matchups for each round
        /// </summary>
        public List<List<MatchUpModel>> Rounds { get; set; } = new List<List<MatchUpModel>>();

        /// <summary>
        /// Complete the tournament with appropriate message
        /// </summary>
        /// <param name="finalMessage"></param>
        public void CompleteTournament(string finalMessage)
        {
            OnTournamentComplete?.Invoke(this, finalMessage);
        }
    }
}
