using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        private List<MatchUpModel> currentRound;
        private List<int> rounds;

        public TournamentViewerForm(TournamentModel tm)
        {
            InitializeComponent();
            tournament = tm;
            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;
            LoadFormData();
            LoadRounds();
        }

        /// <summary>
        /// Show the message when tournament complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tournament_OnTournamentComplete(object sender, string e)
        {
            MessageBox.Show(e);
            this.Close();
        }

        /// <summary>
        /// Set the rounds for round box
        /// </summary>
        private void WireUpRounds()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = rounds;
        }

        /// <summary>
        /// Set the matchups for selected round
        /// </summary>
        private void WireUpMatchUps()
        {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = currentRound;
            matchupListBox.DisplayMember = "DisplayName";
        }

        /// <summary>
        /// Calculate the number of rounds
        /// </summary>
        private void LoadRounds()
        {
            rounds = new List<int>();
            int curRound = 1;

            foreach (var round in tournament.Rounds)
            {
                rounds.Add(curRound++);
            }

            WireUpRounds();
        }

        /// <summary>
        /// Load the tournament title
        /// </summary>
        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

        /// <summary>
        /// Set the matchups for matchup box
        /// </summary>
        private void LoadMatchUps()
        {
            int curRound = (int)roundDropDown.SelectedItem;
            currentRound = tournament.Rounds.ElementAt(curRound - 1).Where(x => x.Winner == null || !unplayedOnlyCheckbox.Checked).ToList();
            WireUpMatchUps();
            DisplayMatchupInfo();
        }

        /// <summary>
        /// If nothing to select clean the form
        /// </summary>
        private void DisplayMatchupInfo()
        {
            bool isVisible = (matchupListBox.Items.Count > 0);

            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;
            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;
            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }

        /// <summary>
        /// Load matchups for round
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchUps();
            WireUpMatchUps();
        }

        /// <summary>
        /// Represent the matchup selected
        /// </summary>
        private void LoadMatchup()
        {
            MatchUpModel selectedMatchup = (MatchUpModel)matchupListBox.SelectedItem;

            if (selectedMatchup == null)
            {
                teamOneName.Text = "Not Yet Set";
                teamOneScoreValue.Text = "";
                teamTwoName.Text = "Not Yet Set";
                teamTwoScoreValue.Text = "";
                return;
            }

            for (int i = 0; i < selectedMatchup.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (selectedMatchup.Entries[0].TeamCompeting != null)
                    {
                        teamOneName.Text = selectedMatchup.Entries[0].TeamCompeting.TeamName.Trim();
                        teamOneScoreValue.Text = selectedMatchup.Entries[0].Score.ToString();
                    }
                    else
                    {
                        teamOneName.Text = "Not Yet Set";
                        teamOneScoreValue.Text = "";
                    }
                    continue;
                }
                else if (i == 1)
                {
                    if (selectedMatchup.Entries[1].TeamCompeting != null)
                    {
                        teamTwoName.Text = selectedMatchup.Entries[1].TeamCompeting.TeamName.Trim();
                        teamTwoScoreValue.Text = selectedMatchup.Entries[1].Score.ToString();
                    }
                    else
                    {
                        teamTwoName.Text = "Not Yet Set";
                        teamTwoScoreValue.Text = "";
                    }
                    return;
                }
            }

            teamTwoName.Text = "<bye>";
            teamTwoScoreValue.Text = "";
        }

        /// <summary>
        /// Load the selected matchup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup();
        }

        /// <summary>
        /// Load the unplayed or all matchups to box, according to option selected 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchUps();
        }

        /// <summary>
        /// Check if scores given are valid
        /// </summary>
        /// <returns></returns>
        private bool IsValidData()
        {
            bool output = true;
            string errorMessage = "";

            double firstTeamScore = 0;
            double secondTeamScore = 0;

            bool TeamOneScoreValid = double.TryParse(teamOneScoreValue.Text, out firstTeamScore);
            bool TeamTwoScoreValid = double.TryParse(teamTwoScoreValue.Text, out secondTeamScore);


            if (((MatchUpModel)matchupListBox.SelectedItem).Winner != null)
            {
                output = false;
                errorMessage += "You cannot change the already created record!";
            }
            else if (firstTeamScore < 0 || secondTeamScore < 0)
            {
                output = false;
                errorMessage += "Score must be >= 0!";
            }
            else if ((!TeamOneScoreValid && teamOneName.Text != "Not Yet Set") || (!TeamTwoScoreValid && (teamTwoName.Text != "Not Yet Set" && teamTwoName.Text != "<bye>")))
            {
                output = false;
                errorMessage += "Enter the correct score values!";
            }
            else if (teamOneName.Text == "Not Yet Set" || teamTwoName.Text == "Not Yet Set")
            {
                output = false;
                errorMessage += "You cannot assign the game that is not determined yet!";
            }
            else if (firstTeamScore == 0 && secondTeamScore == 0)
            {
                output = false;
                errorMessage += "Both of two teams cannot have the score of '0'!";
            }
            else if (firstTeamScore == secondTeamScore)
            {
                output = false;
                errorMessage += "This program does not support a tie!";
            }
            else if (teamTwoName.Text == "<bye>")
            {
                output = false;
                errorMessage += "You cannot assign the score to the <bye> matchup!";
            }

            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage);
            }

            return output;
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            if (!IsValidData()) return;

            MatchUpModel m = (MatchUpModel)matchupListBox.SelectedItem;

            m.Entries[0].Score = double.Parse(teamOneScoreValue.Text);
            m.Entries[1].Score = double.Parse(teamTwoScoreValue.Text);

            try
            {
                TournamentLogic.UpdateTournamentResult(tournament);
            }
            catch (Exception err)
            {
                MessageBox.Show($"The app had the following error:{err.Message}");
                return;
            }

            LoadMatchUps();
        }
    }
}
