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
using TrackerUI.Requesters;

namespace TrackerUI
{
    public partial class createTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> listOfAvailableTeams = GlobalConfig.Connection.GetTeamAll();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();
        ITournamentRequester callerForm;

        public createTournamentForm(ITournamentRequester calling)
        {
            InitializeComponent();
            WireUpLists();
            callerForm = calling;
        }
        
        /// <summary>
        /// Refresh the boxes
        /// </summary>
        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = listOfAvailableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentPlayersListBox.DataSource = null;
            tournamentPlayersListBox.DataSource = selectedTeams;
            tournamentPlayersListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        /// <summary>
        /// Add the team to tournament
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel selected = (TeamModel)selectTeamDropDown.SelectedItem;

            if (selected != null)
            {
                listOfAvailableTeams.Remove(selected);
                selectedTeams.Add(selected);
                WireUpLists();
            }
        }

        /// <summary>
        /// Add the created prize
        /// </summary>
        /// <param name="pm"></param>
        public void PrizeComplete(PrizeModel pm)
        {
            selectedPrizes.Add(pm);
            WireUpLists();
        }

        /// <summary>
        /// Create the prize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            CreatePrizeForm cpf = new CreatePrizeForm(this);
            cpf.Show();
        }

        /// <summary>
        /// Add the created team
        /// </summary>
        /// <param name="tm"></param>
        public void TeamComplete(TeamModel tm)
        {
            selectedTeams.Add(tm);
            WireUpLists();
        }

        /// <summary>
        /// Create the team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm ctf = new CreateTeamForm(this);
            ctf.Show();
        }

        /// <summary>
        /// Remove the team from selected teams
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel tm = (TeamModel)tournamentPlayersListBox.SelectedItem;
            if (tm != null)
            {
                listOfAvailableTeams.Add(tm);
                selectedTeams.Remove(tm);
                WireUpLists();
            }
        }

        /// <summary>
        /// Remove the prize from selected prizes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel pm = (PrizeModel)prizesListBox.SelectedItem;
            if (pm != null)
            {
                selectedPrizes.Remove(pm);
                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            if (DataIsValid())
            {
                TournamentModel tm = new TournamentModel();

                tm.TournamentName = tournamentNameValue.Text;
                tm.EntryFee = Decimal.Parse(entryFeeValue.Text);
                tm.Teams = selectedTeams;
                tm.Prizes = selectedPrizes;

                TournamentLogic.CreateRounds(tm);

                GlobalConfig.Connection.CreateTournament(tm);

                callerForm.TournamentComplete(tm);
                this.Close(); 
            }
        }

        /// <summary>
        /// Check for data
        /// </summary>
        /// <returns></returns>
        private bool DataIsValid()
        {
            bool output = true;
            string errorMessage = "";

            if (entryFeeValue.Text.Length == 0)
            {
                errorMessage = "The entry fee field shound not be empty!";
                output = false;
            }

            decimal fee = 0;
            bool isValid = Decimal.TryParse(entryFeeValue.Text, out fee);

            if (!isValid)
            {
                errorMessage += "Enter the correct entry fee!";
                output = false;
            }

            if (tournamentNameValue.Text.Length == 0)
            {
                errorMessage += "Tournament name field should not be empty!";
                output = false;
            }
            else if (tournamentNameValue.Text.Contains(",") || tournamentNameValue.Text.Contains("\""))
            {
                errorMessage += "Tournament name should not contain , or \" characters! ";
                output = false;
            }

            if (selectedTeams.Count < 2)
            {
                errorMessage += "You need to select at least two teams!";
                output = false;
            }

            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage);
            }

            return output;
        }
    }
}
