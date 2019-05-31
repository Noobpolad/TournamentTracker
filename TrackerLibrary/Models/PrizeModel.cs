using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// Represents the unique identifier
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Represents the place number for that prize
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represent the place name
        /// </summary>
        private string pn;
        public string PlaceName
        {
            get
            {
                return pn;
            }
            set { pn = value.Trim(); }
        }

        /// <summary>
        /// Represents the amount of the prize
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents the percentage for the price
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            PlaceNumber = Convert.ToInt32(placeNumber);
            PlaceName = placeName;
            PrizeAmount = Convert.ToDecimal(prizeAmount);
            PrizePercentage = Convert.ToDouble(prizePercentage);
        }
    }
}
