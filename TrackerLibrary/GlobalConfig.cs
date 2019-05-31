using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Connectors;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizeFileName = "Prizes.csv";
        public const string PeopleFileName = "People.csv";
        public const string TeamsFileName = "Teams.csv";
        public const string TournamentsFileName = "Tournaments.csv";
        public const string MatchupFile = "Matchups.csv";
        public const string MatchupEntryFile = "MatchupEntries.csv";

        public static IDataConnection Connection { get; private set; }

        /// <summary>
        /// Initialize the proper connection
        /// </summary>
        /// <param name="db">Connection type</param>
        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.SQL)
            {
                SQLConnector sql = new SQLConnector();
                Connection = sql;
            }
            else if (db == DatabaseType.TXT)
            {
                TextFileConnector txt = new TextFileConnector();
                Connection = txt;
            }
        }

        /// <summary>
        /// Gets the database server info
        /// </summary>
        /// <param name="name">Database name</param>
        /// <returns>The properly formated server information</returns>
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Gets the corresponding value with the given key from App.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The value of the given key</returns>
        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Sanitize the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToCSVCell(string str)
        {
            str = str.Trim();
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
