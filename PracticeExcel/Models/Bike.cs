namespace PracticeExcel.Models
{
    public class Bike
    {
        private int _manufacturerId;
        private string _bikeName;

        public int MANUFACTURER_ID
        {
            get { return _manufacturerId; }
            set
            {
                if (value > 0)
                {
                    _manufacturerId = value;
                }
                else
                {
                    throw new ArgumentException("MANUFACTURER_ID must be a positive integer.");
                }
            }
        }

        public string BIKE_NAME
        {
            get { return _bikeName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _bikeName = value.Trim();
                }
                else
                {
                    throw new ArgumentException("BIKE_NAME cannot be null or empty.");
                }
            }
        }
    }
}