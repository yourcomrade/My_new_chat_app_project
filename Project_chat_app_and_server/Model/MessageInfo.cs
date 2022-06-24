namespace Project_chat_app_and_server.Model
{
    public class MessageInfo
    {
        public string Username { get; set; }
        private string m_Lastest_message;
        public bool Is_mess { get; set; }
        public string Date_time { get; set; }

        public string Lastest_message
        {
            get => m_Lastest_message;
            set
            {
                if (Is_mess)
                {
                    m_Lastest_message = value;
                }
            }
        }

        public string UserID { get; set; }
    }

}

