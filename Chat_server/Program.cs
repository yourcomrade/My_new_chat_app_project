using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat_server
{
    internal class Program
    {
        private static TcpListener m_listener;
        private static List<Client> m_clients;
        private static int ID = 100;
        private static object m_lock = new object();
        public static async Task  Main(string[] args)
        {
            //Code find local IP address
            //Source: https://www.delftstack.com/howto/csharp/get-local-ip-address-in-csharp/
            string localIP = string.Empty;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, 							SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            Console.WriteLine("Listening at IP Address: " + localIP);
            m_listener = new TcpListener(IPAddress.Any, 2003);
            m_clients = new List<Client>();
            try
            {
                m_listener.Start();
                
                while (true)
                {
                   
                    var client =  await m_listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    if (client != null)
                    {

                        ThreadPool.QueueUserWorkItem(async state => await Serve(new Client(client, ID)));

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            finally
            {
                m_listener.Stop();
            }
            
        }
        public static async Task Serve(Client my_cl)
        {
            Monitor.Enter(m_lock);
            try
            {
                
                m_clients.Add(my_cl);
                ID++;
                Monitor.Pulse(m_lock);
            }
            finally
            {
                Monitor.Exit(m_lock);
            }
            Console.WriteLine($"Member of clients: {m_clients.Count}");
            while (my_cl.Stillhere)
            {
                byte code = await my_cl.my_pack.Read_codeAsync();
                switch (code)
                {
                    case 1:
                        
                            my_cl.username = await my_cl.my_pack.Read_msgAsync();
                            Console.WriteLine($"New user: {my_cl.username}");
                            await my_cl.my_pack.Send_msgAsync(my_cl.userId + " 5060", 1);
                           // await my_cl.my_pack.Send_msgAsync(my_cl.username + "$" + " " + "$" + my_cl.userId, 6);
                            await Task.Delay(2);
                            await Broadcast_newMember(my_cl.userId);
                            break;
                    case 2:
                        my_cl.lastest_mes = await my_cl.my_pack.Read_msgAsync();
                        await Broadcast(my_cl.username, my_cl.userId, my_cl.lastest_mes);
                        
                        break;
                        
                    case 4:
                        string file_name = await my_cl.my_pack.Recv_FileAsync();
                        Console.WriteLine($"New {file_name} upload");
                        await Broadcast(my_cl.username, my_cl.userId, file_name);
                        
                        break;
                    case 7:
                        string url_img = await my_cl.my_pack.Read_msgAsync();
                        Console.WriteLine($"New avatar from: {my_cl.username} and url: {url_img}");
                        my_cl.avatar = url_img;
                        await Broadcast_newMember(my_cl.userId);
                        break;
                    case 10:
                        Console.WriteLine($"{my_cl.username} has left");
                        break;
                }

                if (code == 10)
                {
                    my_cl.m_tcpClient.Close();
                    my_cl.m_tcpClient.Dispose();
                    
                    my_cl.Stillhere = false;
                    Monitor.Enter(m_lock);
                    try
                    {
                        m_clients.Remove(my_cl);
                        Monitor.Pulse(m_lock);
                    }
                    finally
                    {
                        Monitor.Exit(m_lock);
                    }
                    break;
                }
               
            }

           
        }

        public static async Task Broadcast_newMember(string userID)
        {
            var msg_to_all_user = String.Empty;
            foreach (var user in m_clients)
            {
                msg_to_all_user +="#"+user.username + "$" +
                    @user.avatar
                        + "$" + user.userId;
            }
            Console.WriteLine(msg_to_all_user);
            foreach (var user in m_clients)
            {
                await user.my_pack.Send_msgAsync(msg_to_all_user, 6);
            }
        }

        public static async Task Broadcast(string username, string userID, string unknown)
        {
                
                try
                {
                    foreach (var user in m_clients)
                    {
                        

                        if (File.Exists(unknown))
                        {
                            string msg_to_user = username + "$" + DateTime.Now.ToShortDateString() +
                                                 "$ send file name: " + unknown;
                            Console.WriteLine($"{msg_to_user}");
                            await user.my_pack.Send_msgAsync(msg_to_user, 3);
                            await user.my_pack.Send_FileAsync($"{unknown}", 5);

                        }
                        else
                        {
                            string msg_to_user = username + "$" + DateTime.Now.ToShortDateString() + "$" + unknown;
                            Console.WriteLine($"{msg_to_user}");
                            await user.my_pack.Send_msgAsync(msg_to_user, 3);

                        }

                    }

                    

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error from here: {e}");
                }
               
        }
    }
}