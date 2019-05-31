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
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester calledForm;

        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();
            calledForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateUserInput())
            {
                PrizeModel pm = new PrizeModel(placeNumberValue.Text, placeNameValue.Text, prizeAmountValue.Text, prizePercentageValue.Text);

                GlobalConfig.Connection.CreatePrize(pm);

                calledForm.PrizeComplete(pm);
                this.Close();
            }
        }

        /// <summary>
        /// Check if user input is correct
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserInput()
        {
            bool output = true;
            string errorMessage = "";
            int placeNumber = 0;
            bool validNumber = int.TryParse(placeNumberValue.Text, out placeNumber);
            if (!validNumber)
            {
                errorMessage += "Place number field, should be filled with number! ";
                output = false;
            }

            if (placeNumber <= 0 && validNumber)
            {
                errorMessage += "Place number field, should be filled with number > 0! ";
                output = false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                errorMessage += "Place name field, should not be empty!";
                output = false;
            }
            else if (placeNameValue.Text.Contains(",") || placeNameValue.Text.Contains("\""))
            {
                errorMessage += "Place name field, should not contain , or \" characters!";
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool amountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool percentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (!amountValid || !percentageValid)
            {
                errorMessage += "Prize amount or Prize percentage fields should contain decimal numbers > 0! ";
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                errorMessage += "Prize amount or Prize percentage fields should not be negative or 0! ";
                output = false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                errorMessage += "Prize percentage field should contain decimal number >= 0 and <= 100! ";
                output = false;
            }

            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage);
            }
        
            return output;
        }

    }
}
