using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchUpModel
    {
        /// <summary>
        /// Unique id for Matchup
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the teams competing in this matchup
        /// </summary>
        public List<MatchUpEntryModel> Entries { get; set; } = new List<MatchUpEntryModel>();

        /// <summary>
        /// Unique id for Winner
        /// </summary>
        public int WinnerId { get; set; }

        /// <summary>
        /// Represents the winner of this matchup
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Represents the number of round for this matchup
        /// </summary>
        public int MatchUpRound { get; set; }

        /// <summary>
        /// Getter of matchup to display in forms
        /// </summary>
        public string DisplayName
        {
            get
            {
                string output = "";

                foreach (MatchUpEntryModel me in Entries)
                {

                    if (me.TeamCompeting != null)
                    {
                        if (output.Length == 0)
                        {
                            output = me.TeamCompeting.TeamName.Trim();
                        }
                        else
                        {
                            output += " vs. " + me.TeamCompeting.TeamName.Trim(); 
                        }
                    }
                    else
                    {
                        output = "Matchup Not Determined Yet";
                        break;
                    }
                }

                return output;
            }
        }
    }
}
