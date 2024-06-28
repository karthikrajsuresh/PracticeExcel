namespace PracticeExcel.Models
{
    public class Part
    {
        private int _manufacturerId;
        private string _partNo;

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

        public string PART_NO
        {
            get { return _partNo; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _partNo = value.Trim();
                }
                else
                {
                    throw new ArgumentException("PART_NO cannot be null or empty.");
                }
            }
        }
    }
}