namespace Guestbook
{
    public class gbResponse
    {
        private int ciResponseCode;

        public gbResponse()
        {
            ciResponseCode = 0;
            ResponseDescription = "";
        }

        public int ResponseCode
        {
            get { return ciResponseCode; }
            set
            {
                if (Utility.IsNumeric(value))
                {
                    ciResponseCode = value;
                }
            }
        }

        public string ResponseDescription { get; set; }
    }
}