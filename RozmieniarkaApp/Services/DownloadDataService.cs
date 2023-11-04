using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RozmieniarkaApp.Enums;

namespace RozmieniarkaApp.Services
{
    public static class DownloadDataService

    {
        private static string ipAddress = Preferences.Get("MachineIPaddress", "192.168.1.61");
        private static int port = Preferences.Get("MachinePort", 5555);
        private static string CreateMessage(DataQueryType dataQueryType)
        {
            string message = "";
            switch (dataQueryType)
            {
                case DataQueryType.Status:
                    message = "001006";
                    break;
                case DataQueryType.Inserted:
                    message = "003006";
                    break;
                case DataQueryType.Withdrawn:
                    message = "002006";
                    break;
                default:
                    break;
            }
            return message;
        }
        public static async Task<string> DownloadStatus(DataQueryType dataQueryType)
        {
            ipAddress = Preferences.Get("MachineIPaddress", "192.168.1.61");
            port = Preferences.Get("MachinePort", 5555);
            string message = CreateMessage(dataQueryType);
            string status = "";
            using (var client = new TcpClient())
            {
                try
                {
                    client.ConnectAsync(ipAddress, port).Wait(5000);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    status = "Error: " + ex.Message;
                }
                if (client.Connected)
                {
                    using (var stream = client.GetStream())
                    {
                        byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                        await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
                        byte[] dataToReceive = new byte[client.ReceiveBufferSize];
                        await Task.Delay(1000);
                        int bytesRead = await stream.ReadAsync(dataToReceive, 0, client.ReceiveBufferSize);
                        status = Encoding.ASCII.GetString(dataToReceive, 0, bytesRead);
                    }
                }
            }
            return status;
        }
    }
}
