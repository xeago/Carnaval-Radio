using System;

namespace Guestbook
{
    [Serializable]
    public class gbMessage
    {
        public gbMessage()
        {
            ID = 0;
            SubmitDate = "";
            Name = "";
            Email = "";
            Message = "";
            ResponseToMessage = null;
        }

        public int ID { get; set; }

        public string SubmitDate { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public gbMessage ResponseToMessage { get; set; }
    }
}