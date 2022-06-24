
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Project_chat_app_and_server.Connect
{
    
    public class Packet
    {

        private readonly NetworkStream m_ns;
        public Packet(NetworkStream my_ns)
        {
            this.m_ns = my_ns;
        }
       
        private byte[] arr_msg(string msg, byte code)
        {
            using (var m_ms = new MemoryStream())
            {
                m_ms.WriteByte(code);
                byte[] msg_len = new byte[4];
                msg_len = BitConverter.GetBytes(msg.Length);
                m_ms.Write(msg_len,0,msg_len.Length);
                byte[] str = Encoding.ASCII.GetBytes(msg);
                m_ms.Write(str,0,str.Length);
                return m_ms.ToArray();
            }
           
        }
        public async Task Send_msgAsync(string msg, byte code)
        {
           
            byte[] mess = arr_msg(msg, code);
            await m_ns.WriteAsync(mess, 0, mess.Length);
            await m_ns.FlushAsync();

        }

        public async Task<byte> Read_codeAsync()
        {
            byte[] codes = new byte[1];
            await m_ns.ReadAsync(codes, 0, 1);
            return codes[0];
        }
        public async Task<string> Read_msgAsync()
        {
            byte[] msglen = new byte[4];
            int m1= await m_ns.ReadAsync(msglen, 0, 4);
            int len_of_msg = BitConverter.ToInt32(msglen,0);
            byte[] msgBytes = new byte[len_of_msg];
            m1= await m_ns.ReadAsync(msgBytes, 0, len_of_msg);
            string msg = Encoding.ASCII.GetString(msgBytes);
            return msg;
        }

        public async Task Send_FileAsync(string file_path, byte code)
        {

            using (var m_ms = new MemoryStream())
            {
                if (File.Exists(file_path))
                {
                    var file_name = Path.GetFileName(file_path);
                    byte[] file_name_bytes = Encoding.ASCII.GetBytes(file_name);
                    byte[] file_name_len = new byte[4];
                    file_name_len = BitConverter.GetBytes(file_name.Length);
                    byte[] file_content_len = new byte[8];
                    file_content_len = BitConverter.GetBytes(File.Open(file_path,FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite).Length);
                    byte[] file_content = new byte[1000];
                    m_ms.WriteByte(code); 
                    m_ms.Write(file_name_len, 0, 4);
                    m_ms.Write(file_content_len, 0, file_content_len.Length);
                    m_ms.Write(file_name_bytes, 0, file_name_bytes.Length);
                    using (var file_stream =new FileStream(file_path,FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite))//do this to fix the error the file is being used by another process
                    {
                        while (true)
                        {
                            int flag= await file_stream.ReadAsync(file_content, 0, 1000);
                            if (flag == 0)
                            {
                                file_stream.Close();
                                break;
                            }
                            await m_ms.WriteAsync(file_content, 0, 1000);
                        }
                    }
                    await m_ns.WriteAsync(m_ms.ToArray(),0,m_ms.ToArray().Length);
                    await m_ns.FlushAsync();
                }
            }
            
        }

        public async Task<string> Recv_FileAsync()
        {
            byte[] file_name_len = new byte[4];
            int m1= await m_ns.ReadAsync(file_name_len, 0, 4);
            int len_of_file_name = BitConverter.ToInt32(file_name_len,0);
            byte[] len_of_cont_byte= new byte[8];
           m1= await m_ns.ReadAsync(len_of_cont_byte, 0, 8);
            long len_of_content = BitConverter.ToInt64(len_of_cont_byte,0);
            byte[] file_name_bytes = new byte[len_of_file_name];
           m1= await m_ns.ReadAsync(file_name_bytes, 0, len_of_file_name);
            string file_name = Encoding.ASCII.GetString(file_name_bytes);
            using (var File_stream = new FileStream($"{file_name}",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                long length = 0;
                
                while (length < len_of_content)
                {
                    var buffer = new byte[1000];
                    var ind = await m_ns.ReadAsync(buffer, 0, 1000).ConfigureAwait(false);
                    length += ind;
                    await File_stream.WriteAsync(buffer, 0, ind).ConfigureAwait(false);
                }
                Console.WriteLine($"File saved as: {file_name}");
                Console.WriteLine($"Byte received: {File_stream.Length}");
                await File_stream.FlushAsync();
                File_stream.Close();
            }
            
            return file_name;
        }
    }
}
