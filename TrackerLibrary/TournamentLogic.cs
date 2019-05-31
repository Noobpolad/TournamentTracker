using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using System.Text.RegularExpressions;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        /// <summary>
        /// Initialize the rounds for the given tournament
        /// </summary>
        /// <param name="model"></param>
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomTeams = RandomizeTeams(model.Teams);
            int numberOfRounds = FindNumberOfRounds(model.Teams.Count);
            int byes = FindNumberOfByes(numberOfRounds, model.Teams.Count);
            model.Rounds.Add(CreateFirstRound(byes, model.Teams));
            CreateOtherRounds(model, numberOfRounds);
        }

        /// <summary>
        /// Create remaining rounds for the given tournament
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numberOfRounds"></param>
        private static void CreateOtherRounds(TournamentModel model, int numberOfRounds)
        {
            int round = 2;
            List<MatchUpModel> previousRound = model.Rounds[0];
            List<MatchUpModel> currentRound = new List<MatchUpModel>();
            MatchUpModel curModel = new MatchUpModel();

            while (round <= numberOfRounds)
            {
                foreach (MatchUpModel matchUp in previousRound)
                {
                    curModel.Entries.Add(new MatchUpEntryModel { ParentMatch = matchUp });

                    if (curModel.Entries.Count > 1)
                    {
                        curModel.MatchUpRound = round;
                        currentRound.Add(curModel);
                        curModel = new MatchUpModel();
                    }
                }
                model.Rounds.Add(currentRound);
                previousRound = currentRound;
                currentRound = new List<MatchUpModel>();
                round++;
            }
        }

        /// <summary>
        /// Create first round for the tournament
        /// </summary>
        /// <param name="numberOfByes"></param>
        /// <param name="teams"></param>
        /// <returns>First round</returns>
        private static List<MatchUpModel> CreateFirstRound(int numberOfByes, List<TeamModel> teams)
        {
            List<MatchUpModel> output = new List<MatchUpModel>();
            MatchUpModel cur = new MatchUpModel();

            foreach (TeamModel team in teams)
            {
                cur.Entries.Add(new MatchUpEntryModel { TeamCompeting = team });

                if (numberOfByes > 0 || cur.Entries.Count > 1)
                {
                    cur.MatchUpRound = 1;
                    numberOfByes--;
                    output.Add(cur);
                    cur = new MatchUpModel();
                }
            }
            return output;
        }

        /// <summary>
        /// Get the number of byes in this game in order to assign the matchups properly
        /// </summary>
        /// <param name="numberOfRounds"></param>
        /// <param name="numberOfTeams"></param>
        /// <returns>Number of byes</returns>
        private static int FindNumberOfByes(int numberOfRounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= numberOfRounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;
            return output;
        }

        /// <summary>
        /// Get the number of rounds to create
        /// </summary>
        /// <param name="numberOfTeams"></param>
        /// <returns>Number of rounds</returns>
        private static int FindNumberOfRounds(int numberOfTeams)
        {
            int output = 1;
            int var = 2;

            while (var < numberOfTeams)
            {
                var *= 2;
                output += 1;
            }
            return output;
        }

        /// <summary>
        /// Randomize the teams compeating
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static List<TeamModel> RandomizeTeams(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Identify the winners for the given matchups
        /// </summary>
        /// <param name="models"></param>
        private static void MarkWinnerInMatchups(List<MatchUpModel> models)
        {
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];

            foreach (MatchUpModel model in models)
            {
                if (model.Entries.Count == 1) {
                    model.Winner = model.Entries[0].TeamCompeting;
                    continue;
                }

                if (greaterWins == "0")
                {
                    if (model.Entries[0].Score < model.Entries[1].Score)
                    {
                        model.Winner = model.Entries[0].TeamCompeting;
                    }
                    else if (model.Entries[1].Score < model.Entries[0].Score)
                    {
                        model.Winner = model.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("This program is not supporting a tie games");
                    }
                }
                else
                {
                    if (model.Entries[0].Score > model.Entries[1].Score)
                    {
                        model.Winner = model.Entries[0].TeamCompeting;
                    }
                    else if (model.Entries[1].Score > model.Entries[0].Score)
                    {
                        model.Winner = model.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("This program is not supporting a tie games");
                    }
                } 
            }
        }

        /// <summary>
        /// Update the tournament
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateTournamentResult(TournamentModel model)
        {
            List<MatchUpModel> toScore = new List<MatchUpModel>();

            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel m in round)
                {
                    if (m.Winner == null && (m.Entries.Any(x => x.Score != 0) || m.Entries.Count == 1))
                    {
                        toScore.Add(m);
                    }
                }
            }

            MarkWinnerInMatchups(toScore);

            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));

            CheckIfTournamentComplete(model);

        }

        /// <summary>
        /// Check if tournament is complete
        /// </summary>
        /// <param name="model"></param>
        private static void CheckIfTournamentComplete(this TournamentModel model)
        {

            foreach (List<MatchUpModel> round in model.Rounds)
            {
                if (round.Any(x => x.Winner == null))
                {
                    return;
                }
            }

            CompleteTournament(model);
        }

        /// <summary>
        /// Complete the tournament with the message the prizes, that winners won
        /// </summary>
        /// <param name="model"></param>
        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);
            TeamModel winner = model.Rounds.Last().First().Winner;
            TeamModel looser = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != winner).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal looserPrize = 0;

            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.Teams.Count * model.EntryFee;
                foreach (PrizeModel prize in model.Prizes)
                {
                    if (prize.PlaceNumber == 1)
                    {
                        if (prize.PrizeAmount != 0)
                        {
                            winnerPrize = prize.PrizeAmount;
                        }
                        else
                        {
                            winnerPrize = Decimal.Multiply(totalIncome,Convert.ToDecimal(prize.PrizePercentage / 100));
                        }
                    }
                    else if (prize.PlaceNumber == 2)
                    {
                        if (prize.PrizeAmount != 0)
                        {
                            looserPrize = prize.PrizeAmount;
                        }
                        else
                        {
                            looserPrize = Decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100));
                        }
                    }
                }
            }

            string finalMessage = $"Congratulations for the {winner.TeamName} team";

            if (winnerPrize > 0)
            {
                finalMessage += $", the {winner.TeamName} team won {winnerPrize}$";
            }
            if (looserPrize > 0)
            {
                finalMessage += $", the {looser.TeamName} team won {looserPrize}$";
            }

            finalMessage += " ! Tournament completed !";

            model.CompleteTournament(finalMessage);
        }

        /// <summary>
        /// Promote the winners to next rounds
        /// </summary>
        /// <param name="models"></param>
        /// <param name="tournament"></param>
        private static void AdvanceWinners(List<MatchUpModel> models, TournamentModel tournament)
        {
            foreach (MatchUpModel model in models)
            {
                foreach (List<MatchUpModel> round in tournament.Rounds)
                {
                    foreach (MatchUpModel rm in round)
                    {
                        foreach (MatchUpEntryModel ent in rm.Entries)
                        {
                            if (ent.ParentMatch != null)
                            {
                                if (ent.ParentMatch.id == model.id)
                                {
                                    ent.TeamCompeting = model.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
