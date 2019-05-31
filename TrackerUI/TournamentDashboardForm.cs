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
    public partial class TournamentDashboardForm : Form, ITournamentRequester
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournamentsAll();

        public TournamentDashboardForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        /// <summary>
        /// Add the tournament
        /// </summary>
        /// <param name="model"></param>
        public void TournamentComplete(TournamentModel model)
        {
            tournaments.Add(model);
            WireUpLists();
        }

        /// <summary>
        /// Refresh the box
        /// </summary>
        public void WireUpLists()
        { 
            selectTournamentDropDown.DataSource = null;
            selectTournamentDropDown.DataSource = tournaments;
            selectTournamentDropDown.DisplayMember = "TournamentName";
        }

        /// <summary>
        /// Create tournament
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            createTournamentForm ctf = new createTournamentForm(this);
            ctf.Show();
        }

        /// <summary>
        /// Load the tournament viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tm = (TournamentModel)selectTournamentDropDown.SelectedItem;
            if (tm == null) return;
            TournamentViewerForm tvf = new TournamentViewerForm(tm);
            tvf.FormClosed += Tvf_FormClosed;
            tvf.Show();
        }

        /// <summary>
        /// When the tournament viewer closed refresh the box if needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tvf_FormClosed(object sender, FormClosedEventArgs e)
        {
            tournaments = GlobalConfig.Connection.GetTournamentsAll();
            this.WireUpLists();
        }
    }
}
