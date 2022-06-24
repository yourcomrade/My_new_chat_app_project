using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project_chat_app_and_server.Connect;
using Project_chat_app_and_server.Model;

namespace Project_chat_app_and_server.ViewModel
{
    public class MyMainViewModel : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        public ObservableCollection<UserInfoViewModel> users_list { get; set; }
        public ObservableCollection<MessageInfo> chat_bubbles { get; set; }
        public UserInfoViewModel m_UserInfoViewModel { get; set; }
        public myServer_connect MyServerConnect { get; set; }
        private readonly object m_lock = new object();
        public string Register
        {
            get =>MyServerConnect?.status;
            set
            {
                MyServerConnect.status = value;
                OnPropertyChanged("Register");
            }
        }

        public event Action Recvmess;
        public event Action Recvfile;
        public event Action NewUser;
        private bool Stillconnect = false;
        public UserInfoViewModel myProfile { get; set; }
        public ICommand Init_connect_to_server { get; set; }
        public ICommand  Send_message { get; set; }
       
        public ICommand  Send_file { get; set; }
        public ICommand  Recv_file { get; set; }
        public ICommand  Disconnect { get; set; }
        public ICommand  Load_ava { get; set; }
        //public myServer_connect MyServerConnect { get; set; }
        private string m_IPaddress="0";
        public string RecvMess=String.Empty;
        public string IPaddress
        {
            get => m_IPaddress;
            set
            {
                m_IPaddress = value;
                OnPropertyChanged("IPaddress");
            }
        }

        private string m_TextMessage=String.Empty;

        public string TextMessage
        {
            get => m_TextMessage;
            set
            {
                m_TextMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        private string m_portnum="0";
        public string portnum
        {
            get => m_portnum;
            set
            {
                m_portnum = value;
                OnPropertyChanged("portnum");
            }
        }

        public MyMainViewModel()
        {
            
            users_list = new ObservableCollection<UserInfoViewModel>();
            chat_bubbles = new ObservableCollection<MessageInfo>();


            myProfile = new UserInfoViewModel(new UserInfo
            {
                Username = String.Empty,
                UserID = String.Empty,
                AvatarSource = String.Empty
            });
           
            
            
            
            Load_ava = new RelayCommand<string>(async (p) => await Save_Ava(p), (p) =>
            {
                if (p == null)
                {
                    return false;
                }
                else
                    return true;
            } );
            Init_connect_to_server = new RelayCommand(async (p) => await Task.Run(async () => { await InitConnect();}),
                p => !string.IsNullOrEmpty(myProfile.Username));
            Disconnect = new RelayCommand(async p => await Task .Run(async () => { await DisconnectServer();}),
                p => !string.IsNullOrEmpty(myProfile.Username));
            Send_message = new RelayCommand(async p => await SendMessage(),
                o => !string.IsNullOrEmpty(myProfile.Username));
            Send_file = new RelayCommand<string>(async (p) => await m_Send_file(p), (p) =>
            {
                if (p == null)
                {
                    return false;
                }
                else
                    return true;
            });
            Recvmess += (async () => await MessageReceived());
            Recvfile += (async () => await FileReceive());
            NewUser += (async () => await Recv_newUser());

        }
        

        private async Task Save_Ava(string fs)
        {
            if (!string.IsNullOrEmpty(fs)&&!fs.Equals(" "))
            {
                myProfile.AvatarSource = fs;
                await MyServerConnect.my_pack.Send_msgAsync(fs, 7);
            }
            
        }

        private async Task InitConnect()
        {
            MyServerConnect=new myServer_connect(IPaddress,Convert.ToInt32(portnum));
           
            (bool, string ) res = await MyServerConnect.Try_connect_server_init(myProfile.Username);
            if (res.Item1)
            {
                Register = "Register successfull";
                myProfile.UserID = MyServerConnect.my_user_ID;
                Stillconnect = true;
                await Waiting();
            }
            else
            {
                Register = res.Item2;
            }
        }

        private async Task SendMessage()
        {
            if (!string.IsNullOrEmpty(TextMessage) && !string.IsNullOrWhiteSpace(TextMessage))
            {
                if (!MyServerConnect.status.Equals("Disconnect"))
                {
                    await MyServerConnect.my_pack.Send_msgAsync(TextMessage, 2);
                }
                
            }

        }

        private async Task Waiting()
        {
            while (Stillconnect)
            {     byte code = await MyServerConnect.my_pack.Read_codeAsync();
               
                   switch (code)
                   {
                       case 6:
                           RecvMess = await MyServerConnect.my_pack.Read_msgAsync();
                           NewUser?.Invoke();
                           break;
                       case 3:
                           RecvMess = await MyServerConnect.my_pack.Read_msgAsync();
                            Recvmess?.Invoke();
                           break;
                       case 5:
                           RecvMess = await MyServerConnect.my_pack.Recv_FileAsync();
                           break;
                       default:
                           MessageBox.Show(Convert.ToString(code));
                           break;
                   }
                   
                
            }
            
        }
        
        

        private async Task DisconnectServer()
        {
            Stillconnect = false;
            MyServerConnect.status = "Disconnect";
            
            await MyServerConnect.my_pack.Send_msgAsync("", 10);
            await MyServerConnect.Close_connection();
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                users_list.Clear();
                chat_bubbles.Clear();
            });
            
        }
        private async Task MessageReceived()
        {
           
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    var msg_arr = RecvMess.Split('$').ToArray();
                    chat_bubbles.Add(new MessageInfo
                    {
                        Date_time = DateTime.Now.ToShortDateString(),
                        Is_mess = true,
                        Lastest_message = msg_arr[2],
                        Username = msg_arr[0],
                        UserID = msg_arr[1]
                    });
                });
            });
           
        }

        private async Task Recv_newUser()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (users_list.Count > 0)
                {
                    users_list.Clear();
                }
                var msg_arr = @RecvMess.Split('#').ToArray();
                foreach (var sub_msg_arr in msg_arr)
                {
                    if (sub_msg_arr != String.Empty)
                    {
                        var new_user = sub_msg_arr.Split('$').ToArray();
                        users_list.Add(new UserInfoViewModel(new UserInfo
                        {
                            Username = new_user[0],
                            AvatarSource = new_user[1],
                            UserID = new_user[2]
                        }));
                    }
                   
                }

              


            });
        }

        private async Task m_Send_file(string fs)
        {
            await MyServerConnect.my_pack.Send_FileAsync(fs, 4);
        }

        private async Task FileReceive()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                chat_bubbles.Add(new MessageInfo
                {
                    Date_time = DateTime.Now.ToShortDateString(),
                    Is_mess = true,
                    Lastest_message = RecvMess,
                    Username = string.Empty,
                    UserID = string.Empty
                });
            });
        }
       
    }
}

