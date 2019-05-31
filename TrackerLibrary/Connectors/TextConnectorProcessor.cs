using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.Connectors.TextHelper
{
    public static class TextConnectorProcessor
    {

        /// <summary>
        /// Forms the full path for the given file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Full path</returns>
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        /// <summary>
        /// Opens the file and reads all the lines
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>All the lines from the File</returns>
        public static List<string> LoadFile(this string filePath)
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["filePath"])) Directory.CreateDirectory(ConfigurationManager.AppSettings["filePath"]);

            if (!File.Exists(filePath))
            {
                return new List<string>();
            }
            return File.ReadAllLines(filePath).ToList();
        }

        /// <summary>
        /// Converts all the lines from the file to the PrizeModel objects
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of PrizeModel</returns>
        public static List<PrizeModel> ConvertToPrizes(this List<string> lines)
        {
            List<PrizeModel> listOfPrizes = new List<PrizeModel>();

            foreach (string line in lines)
            {
                PrizeModel pm = new PrizeModel();
                string[] columns = line.Split(',');
                pm.id = int.Parse(columns[0]);
                pm.PlaceName = columns[1];
                pm.PlaceNumber = int.Parse(columns[2]);
                pm.PrizeAmount = decimal.Parse(columns[3]);
                pm.PrizePercentage = double.Parse(columns[4]);
                listOfPrizes.Add(pm);
            }
            return listOfPrizes;
        }

        /// <summary>
        /// Converts all the lines from the file to the PersonModel objects
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of PersonModel</returns>
        public static List<PersonModel> ConvertToPeople(this List<string> lines)
        {
            List<PersonModel> listOfPeople = new List<PersonModel>();

            foreach (string line in lines)
            {
                PersonModel pm = new PersonModel();
                string[] columns = line.Split(',');
                pm.id = int.Parse(columns[0]);
                pm.FirstName = columns[1];
                pm.LastName = columns[2];
                pm.EmailAddress = columns[3];
                pm.CellphoneNumber = columns[4];
                listOfPeople.Add(pm);
            }
            return listOfPeople;
        }

        /// <summary>
        /// Saves the List of PersonModel objects to the file with the appropriate format
        /// </summary>
        /// <param name="listOfPeople"></param>
        public static void SaveToPeopleFile(this List<PersonModel> listOfPeople)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in listOfPeople)
            {
                lines.Add($"{p.id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellphoneNumber}");
            }
            File.WriteAllLines(GlobalConfig.PeopleFileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Saves the List of PrizeModel objects to the file with the appropriate format
        /// </summary>
        /// <param name="listOfPrizes"></param>
        public static void SaveToPrizeFile(this List<PrizeModel> listOfPrizes)
        {
            List<string> lines = new List<string>();
            foreach (PrizeModel p in listOfPrizes)
            {
                lines.Add($"{p.id},{p.PlaceName},{p.PlaceNumber},{p.PrizeAmount},{p.PrizePercentage}");
            }
            File.WriteAllLines(GlobalConfig.PrizeFileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Converts all the lines from the files to the TeamModel objects
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of TeamModel objects</returns>
        public static List<TeamModel> ConvertToTeams(this List<string> lines)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> listOfPeople = GlobalConfig.PeopleFileName.FullFilePath().LoadFile().ConvertToPeople();

            foreach (string line in lines)
            {
                TeamModel tm = new TeamModel();
                string[] columns = line.Split(',');
                tm.id = int.Parse(columns[0]);
                tm.TeamName = columns[1];
                string[] teamMembersIds = columns[2].Split('|');
                foreach (string id in teamMembersIds)
                {
                    tm.TeamMembers.Add(listOfPeople.Where(x => x.id == int.Parse(id)).First());
                }
                output.Add(tm);
            }
            return output;
        }

        /// <summary>
        /// Converts all the lines from the files to TournamentModel objects
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of TournamentModel</returns>
        public static List<TournamentModel> ConvertToTournaments(this List<string> lines)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = GlobalConfig.TeamsFileName.FullFilePath().LoadFile().ConvertToTeams();
            List<PrizeModel> prizes = GlobalConfig.PrizeFileName.FullFilePath().LoadFile().ConvertToPrizes();
            List<MatchUpModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchups();

            foreach (string line in lines)
            {
                TournamentModel model = new TournamentModel();
                string[] columns = line.Split(',');
                model.id = Convert.ToInt32(columns[0]);
                model.TournamentName = columns[1];
                model.EntryFee = Convert.ToDecimal(columns[2]);

                string[] teamIds = columns[3].Split('|');

                foreach (string id in teamIds)
                {
                    model.Teams.Add(teams.Where(x => x.id == Convert.ToInt32(id)).First());
                }

                if (columns[4].Length > 0)
                {
                    string[] prizeIds = columns[4].Split('|');

                    foreach (string id in prizeIds)
                    {
                        model.Prizes.Add(prizes.Where(x => x.id == Convert.ToInt32(id)).First());
                    } 
                }

                string[] rounds = columns[5].Split('|');

                foreach (string round in rounds)
                {
                    List<MatchUpModel> r = new List<MatchUpModel>();
                    string[] matchupIds = round.Split('^');
                    foreach (string id in matchupIds)
                    {
                        r.Add(matchups.Where(x => x.id == int.Parse(id)).First());
                    }
                    model.Rounds.Add(r);
                }

                output.Add(model);
            }
            return output;
        }

        /// <summary>
        /// Convert all the lines from the files to MatchupModel objects
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of MatchupModels</returns>
        public static List<MatchUpModel> ConvertToMatchups(this List<string> lines)
        {
            List<MatchUpModel> output = new List<MatchUpModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                MatchUpModel model = new MatchUpModel();
                model.id = int.Parse(columns[0]);
                model.Entries = ConvertStringToMatchupEntryModels(columns[1]);
                if (columns[2].Length > 0)
                {
                    model.Winner = GetTeamById(columns[2]);
                }
                else
                {
                    model.Winner = null;
                }
                model.MatchUpRound = int.Parse(columns[3]);
                output.Add(model);
            }
            return output;
        }

        /// <summary>
        /// Save tournaments to file
        /// </summary>
        /// <param name="listOfTournaments"></param>
        public static void SaveToTournamentFile(this List<TournamentModel> listOfTournaments)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel model in listOfTournaments)
            {
                lines.Add($"{model.id},{model.TournamentName},{model.EntryFee},{model.Teams.GetTeamsIds()},{model.Prizes.GetPrizesIds()},{model.Rounds.GetRoundsIds()}");
            }

            File.WriteAllLines(GlobalConfig.TournamentsFileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Save the rounds of tournament to file
        /// </summary>
        /// <param name="model"></param>
        public static void SaveRoundsToFile(this TournamentModel model)
        {
            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel matchup in round)
                {
                    matchup.SaveMatchupToFile();
                }
            }
        }

        /// <summary>
        /// Save the matchups of tournament to file
        /// </summary>
        /// <param name="model"></param>
        public static void SaveMatchupToFile(this MatchUpModel model)
        {
            List<MatchUpModel> models = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchups();

            int curId = 1;

            if (models.Count > 0)
            {
                curId = models.OrderByDescending(x => x.id).First().id + 1;
            }

            model.id = curId;
            models.Add(model);

            foreach (MatchUpEntryModel entry in model.Entries)
            {
                entry.SaveEntryToFile();
            }

            List<string> lines = new List<string>();

            foreach (MatchUpModel m in models)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.id.ToString();
                }
                lines.Add($"{m.id},{m.Entries.ConvertMatchupEntryListToString()},{winner},{m.MatchUpRound}");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        /// <summary>
        /// Replace the old matchup with the new one
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateMatchupToFile(this MatchUpModel model)
        {
            List<MatchUpModel> models = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchups();
            MatchUpModel oldMatch = new MatchUpModel();

            foreach (MatchUpModel m in models)
            {
                if (m.id == model.id)
                {
                    oldMatch = m;
                }
            }

            models.Remove(oldMatch);
            models.Add(model);


            foreach (MatchUpEntryModel entry in model.Entries)
            {
                entry.UpdateEntryToFile();
            }

            List<string> lines = new List<string>();

            foreach (MatchUpModel m in models)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.id.ToString();
                }
                lines.Add($"{m.id},{m.Entries.ConvertMatchupEntryListToString()},{winner},{m.MatchUpRound}");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        /// <summary>
        /// Save matchup entry of the tournament to file
        /// </summary>
        /// <param name="model"></param>
        public static void SaveEntryToFile(this MatchUpEntryModel model)
        {
            List<MatchUpEntryModel> models = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntries();
            int curId = 1;

            if (models.Count > 0)
            {
                curId = models.OrderByDescending(x => x.id).First().id + 1;
            }

            model.id = curId;
            models.Add(model);

            List<string> lines = new List<string>();

            foreach (MatchUpEntryModel entry in models)
            {
                string parent = "";
                string teamCompeting = "";
                if (entry.ParentMatch != null)
                {
                    parent = entry.ParentMatch.id.ToString();
                }
                if (entry.TeamCompeting != null)
                {
                    teamCompeting = entry.TeamCompeting.id.ToString();
                }
                lines.Add($"{entry.id},{teamCompeting},{entry.Score},{parent}");
            }
            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        /// <summary>
        /// Replace the old entry with the new one
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateEntryToFile(this MatchUpEntryModel model)
        {
            List<MatchUpEntryModel> models = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntries();
            MatchUpEntryModel oldEntry = new MatchUpEntryModel();

            foreach (MatchUpEntryModel entry in models)
            {
                if (entry.id == model.id)
                {
                    oldEntry = entry;
                }
            }

            models.Remove(oldEntry);
            models.Add(model);

            List<string> lines = new List<string>();

            foreach (MatchUpEntryModel entry in models)
            {
                string parent = "";
                string teamCompeting = "";
                if (entry.ParentMatch != null)
                {
                    parent = entry.ParentMatch.id.ToString();
                }
                if (entry.TeamCompeting != null)
                {
                    teamCompeting = entry.TeamCompeting.id.ToString();
                }
                lines.Add($"{entry.id},{teamCompeting},{entry.Score},{parent}");
            }
            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        /// <summary>
        /// Convert the entries to string
        /// </summary>
        /// <param name="models"></param>
        /// <returns>String of format (e1.id|e2.id|e3.id...)</returns>
        public static string ConvertMatchupEntryListToString(this List<MatchUpEntryModel> models)
        {
            if (models.Count == 0) return "";
            string output = "";
            foreach (MatchUpEntryModel model in models)
            {
                output += $"{model.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        /// <summary>
        /// Convert the data from the file to MatchupEntryModel models
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of MatchupEntryModel</returns>
        public static List<MatchUpEntryModel> ConvertToMatchupEntries(this List<string> lines)
        {
            List<MatchUpEntryModel> output = new List<MatchUpEntryModel>();

            foreach (string line in lines)
            {
                MatchUpEntryModel model = new MatchUpEntryModel();

                string[] columns = line.Split(',');
                model.id = int.Parse(columns[0]);

                if (columns[1].Length>0)
                {
                    model.TeamCompeting = GetTeamById(columns[1]);
                }
                else
                {
                    model.TeamCompeting = null;
                }

                model.Score = double.Parse(columns[2]);

                int parentId = 0;

                if (int.TryParse(columns[3],out parentId))
                {
                    model.ParentMatch = GetMatchupById(parentId.ToString());
                }
                else
                {
                    model.ParentMatch = null;
                }
                output.Add(model);
            }
            return output;
        }

        /// <summary>
        /// Convert the string data to actual MatchupEntryModel models
        /// </summary>
        /// <param name="input"></param>
        /// <returns>List of MatchupEntryModel</returns>
        private static List<MatchUpEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<string> output = new List<string>();
            List<string> lines = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();

            foreach (string id in ids)
            {
                foreach (string line in lines)
                {
                    string[] cols = line.Split(',');
                    if (cols[0] == id)
                    {
                        output.Add(line);
                    }
                }
            }

            return output.ConvertToMatchupEntries();
        }

        /// <summary>
        /// Get the Matchup with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Matchup with the specified id</returns>
        private static MatchUpModel GetMatchupById(string id)
        {
            List<string> lines = GlobalConfig.MatchupFile.FullFilePath().LoadFile();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                if (cols[0] == id)
                {
                    List<string> correctLine = new List<string>();
                    correctLine.Add(line);
                    return correctLine.ConvertToMatchups().First();
                }
            }
            return null;
        }

        /// <summary>
        /// Get the team with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Team with the specified id</returns>
        private static TeamModel GetTeamById(string id)
        {
            List<string> lines =  GlobalConfig.TeamsFileName.FullFilePath().LoadFile();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                if (cols[0]==id)
                {
                    List<string> correctLine = new List<string>();
                    correctLine.Add(line);
                    return correctLine.ConvertToTeams().First();
                }
            }
            return null;
        }

        /// <summary>
        /// Get the rounds in the string format
        /// </summary>
        /// <param name="listsOfRounds"></param>
        /// <returns>String in format ((round1.id)|(round2.id)|(round3.id)...)</returns>
        private static string GetRoundsIds(this List<List<MatchUpModel>> listsOfRounds)
        {
            string output = "";
            if (listsOfRounds.Count == 0) return output;
            foreach (var list in listsOfRounds)
            {
                output += $"{list.GetMatchUpIds()}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        /// <summary>
        /// Get the matchups in the string format
        /// </summary>
        /// <param name="listOfMatchUps"></param>
        /// <returns>String of format (matchup1.id^matchup2.id^matchup3.id...)</returns>
        private static string GetMatchUpIds(this List<MatchUpModel> listOfMatchUps)
        {
            string output = "";
            if (listOfMatchUps.Count == 0) return output;
            foreach (var model in listOfMatchUps)
            {
                output += $"{model.id}^";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        /// <summary>
        /// Get the prizes in string format
        /// </summary>
        /// <param name="listOfPrizes"></param>
        /// <returns>String of format (prize1.id|prize2.id|prize3.id...)</returns>
        private static string GetPrizesIds(this List<PrizeModel> listOfPrizes)
        {
            string output = "";
            if (listOfPrizes.Count == 0) return output;
            foreach (PrizeModel model in listOfPrizes)
            {
                output += $"{model.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        /// <summary>
        /// Get the teams in string format
        /// </summary>
        /// <param name="listOfTeams"></param>
        /// <returns>String of format (team1.id|team2.id|team3.id...)</returns>
        private static string GetTeamsIds(this List<TeamModel> listOfTeams)
        {
            string output = "";
            if (listOfTeams.Count == 0) return output;
            foreach (TeamModel model in listOfTeams)
            {
                output += $"{model.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        /// <summary>
        /// Saves the List of TeamModel objects to the file with the appropriate format
        /// </summary>
        /// <param name="listOfTeams"></param>
        public static void SaveToTeamFile(this List<TeamModel> listOfTeams)
        {
            List<string> teamsToSave = new List<string>();

            foreach (TeamModel team in listOfTeams)
            {
                teamsToSave.Add($"{team.id},{team.TeamName},{team.TeamMembers.GetMembersIds()}");
            }
            File.WriteAllLines(GlobalConfig.TeamsFileName.FullFilePath(), teamsToSave);
        }

        /// <summary>
        /// Forms the correct formating for the People in the teams
        /// </summary>
        /// <param name="people"></param>
        /// <returns>The ids of the People in the team</returns>
        private static string GetMembersIds(this List<PersonModel> listOfPeople)
        {
            string output = "";
            if (listOfPeople.Count == 0) return output;
            foreach (PersonModel person in listOfPeople)
            {
                output += $"{person.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
