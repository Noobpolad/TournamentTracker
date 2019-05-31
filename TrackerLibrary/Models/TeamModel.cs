using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// Unique id for team
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the list of players in the team
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        /// <summary>
        /// Represents the name of the team
        /// </summary>
        private string tn;
        public string TeamName
        {
            get
            {
                return tn;
            }
            set { tn = value.Trim(); }
        }
    }
}
