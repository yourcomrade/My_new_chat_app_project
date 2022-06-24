using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Project_chat_app_and_server.Model;

namespace Project_chat_app_and_server.ViewModel
{
    public class UserInfoViewModel:INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public UserInfoViewModel(UserInfo user)
        {
            this.m_userinfo = user;
        }
        private UserInfo m_userinfo;

        public string Username
        {
            get
            {
                if (m_userinfo.Username==null)
                    return String.Empty;
                else
                {
                    return m_userinfo.Username;
                }
            }
            set
            {
                try
                {
                    m_userinfo.Username = value;
                    OnPropertyChanged("Username");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }

        public string UserID
        {
            get
            {
                if (m_userinfo.UserID == null)
                {
                    return String.Empty;
                }
                else
                {
                    return m_userinfo.UserID;
                }
            }
            set
            {
                try
                {
                    m_userinfo.UserID = value;
                    OnPropertyChanged("UserID");
                }catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }

        public string AvatarSource
        {
            get => m_userinfo.AvatarSource;
            set
            {
                try
                {
                    m_userinfo.AvatarSource = value;
                    OnPropertyChanged("AvatarSource");
                }catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }
    }
}

