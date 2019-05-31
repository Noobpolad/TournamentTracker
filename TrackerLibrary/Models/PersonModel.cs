using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {
        /// <summary>
        /// Unique id for person
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the first name of the person
        /// </summary>
        private string fname;
        public string FirstName
        {
            get {
                return fname;
            }
            set { fname = value.Trim(); }
        }

        /// <summary>
        /// Represents the second name of the person
        /// </summary>
        private string lname;
        public string LastName
        {
            get
            {
                return lname;
            }
            set { lname = value.Trim(); }
        }


        /// <summary>
        /// Represents the email address of the persson
        /// </summary>
        private string mailAddress;
        public string EmailAddress
        {
            get
            {
                return mailAddress;
            }
            set { mailAddress = value.Trim(); }
        }


        /// <summary>
        /// Represents the cell phone number of the person
        /// </summary>
        private string cellNumber;
        public string CellphoneNumber
        {
            get
            {
                return cellNumber;
            }
            set { cellNumber = value.Trim(); }
        }

        public PersonModel()
        {

        }

        public PersonModel(string fn, string ln, string email, string phone)
        {
            FirstName = fn;
            LastName = ln;
            EmailAddress = email;
            CellphoneNumber = phone;
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
