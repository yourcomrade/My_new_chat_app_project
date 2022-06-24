using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Project_chat_app_and_server.Connect
{
    public class myServer_connect
    {
        public string my_user_name { get; set; }
        public string my_user_ID { get; set; }


        private TcpClient m_TcpClient;
        public string IP_server { get; set; }
        public string status { get; set; }
        public int numport { get; set; }
        public Packet my_pack { get; set; }

        public myServer_connect(string IPaddress_server, int portnum)
        {
            IP_server = IPaddress_server;
            this.numport = portnum;
            this.m_TcpClient = new TcpClient(IPaddress_server, portnum);
            this.IP_server = IPaddress_server;
            my_pack = new Packet(m_TcpClient.GetStream());
        }



        public async Task<(bool, string)> Try_connect_server_init(string username)
        {
            if (!m_TcpClient.Connected)
            {
                this.m_TcpClient = new TcpClient(IP_server, numport);
                my_pack = new Packet(m_TcpClient.GetStream());
            }

            my_user_name = username;
            await my_pack.Send_msgAsync(my_user_name, 1);
            byte code = await my_pack.Read_codeAsync();
            if (code == 1)
            {
                string login = await my_pack.Read_msgAsync();
                my_user_ID = login.Split(' ').ToArray()[0];

                status = "Register successfully";
                return (true, login);
            }
            else
            {
                status = "Unsucessfully register ";
                return (false, "Unsuccess");
            }
        }

        public async Task Close_connection()
        {
            m_TcpClient.Close();
        }
    }
}

