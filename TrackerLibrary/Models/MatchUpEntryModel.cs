using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchUpEntryModel
    {
        /// <summary>
        /// Unique id for Mutchup entry
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the id of the competing team
        /// </summary>
        public int TeamCompetingId { get; set; }

        /// <summary>
        /// Represents the team in matchcup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represents the score for matchup
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Reprsents the id of the matchup this team is came from
        /// </summary>
        public int ParentMatchupId { get; set; }

        /// <summary>
        /// Represents the matchup this team is came from
        /// </summary>
        public MatchUpModel ParentMatch { get; set; }
    }
}
