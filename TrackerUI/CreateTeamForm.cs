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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableListOfPerson = GlobalConfig.Connection.GetPersonAll();
        private List<PersonModel> selectedListOfPerson = new List<PersonModel>();
        ITeamRequester calledForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            WireUpLists();
            calledForm = caller;
        }

        /// <summary>
        /// Refresh the boxes
        /// </summary>
        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableListOfPerson;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedListOfPerson;
            teamMembersListBox.DisplayMember = "FullName";
        }

        /// <summary>
        /// Create the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (CheckValid())
            {
                PersonModel pm = new PersonModel(firstNameValue.Text,lastNameValue.Text,emailValue.Text,cellPhoneValue.Text);
                GlobalConfig.Connection.CreatePerson(pm);
                availableListOfPerson.Add(pm);
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";
            }
        }

        /// <summary>
        /// Check if member data is valid
        /// </summary>
        /// <returns></returns>
        private bool CheckValid()
        {
            bool output = true;
            string errorMessage = "";

            if (firstNameValue.Text.Length == 0)
            {
                errorMessage += "First name field should not be empty! ";
                output = false;
            }
            else if (firstNameValue.Text.Contains(",") || firstNameValue.Text.Contains("\""))
            {
                errorMessage += "First name field should not contain , or \" characters! ";
                output = false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                errorMessage += "Second name field should not be empty! ";
                output = false;
            }
            else if (lastNameValue.Text.Contains(",") || lastNameValue.Text.Contains("\""))
            {
                errorMessage += "Last name field should not contain , or \" characters! ";
                output = false;
            }

            if (emailValue.Text.Length == 0)
            {
                errorMessage += "Email field should not be empty! ";
                output = false;
            }
            else if (emailValue.Text.Contains(",") || emailValue.Text.Contains("\""))
            {
                errorMessage += "Email field should not contain , or \" characters! ";
                output = false;
            }

            if (cellPhoneValue.Text.Length == 0)
            {
                errorMessage += "Phone field should not be empty!";
                output = false;
            }
            else if (cellPhoneValue.Text.Contains(",") || cellPhoneValue.Text.Contains("\""))
            {
                errorMessage += "Cell phone field should not contain , or \" characters! ";
                output = false;
            }

            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage);
            }

            return output;
        }

        /// <summary>
        /// Add the player to team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTeamMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if (p != null)
            {
                availableListOfPerson.Remove(p);
                selectedListOfPerson.Add(p);
                WireUpLists(); 
            }
        }

        /// <summary>
        /// Remove the player from team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSelectedTeamMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            if (p != null)
            {
                availableListOfPerson.Add(p);
                selectedListOfPerson.Remove(p);
                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (TeamIsValid())
            {
                TeamModel tm = new TeamModel();
                tm.TeamName = teamNameValue.Text;
                tm.TeamMembers = selectedListOfPerson;
                GlobalConfig.Connection.CreateTeam(tm);
                calledForm.TeamComplete(tm);
                this.Close(); 
            }
        }

        private bool TeamIsValid()
        {
            bool output = true;
            string errorMessage = "";

            if (teamNameValue.Text.Length == 0)
            {
                errorMessage += "The team name field cannot be empty!";
                output = false;
            }
            else if (teamNameValue.Text.Contains(",") || teamNameValue.Text.Contains("\""))
            {
                errorMessage += "Team name should not contain , or \" characters! ";
                output = false;
            }

            if (selectedListOfPerson.Count == 0)
            {
                errorMessage += "At least one team member must be selected!";
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
