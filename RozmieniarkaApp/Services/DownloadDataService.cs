﻿using RozmieniarkaApp.Enums;
using System.Net.Sockets;
using System.Text;
using System;

namespace RozmieniarkaApp.Services
{
    public static class DownloadDataService

    {
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
            string ipAddress = Preferences.Get("MachineIPaddress", "10.0.0.7");
            int port = Preferences.Get("MachinePort", 5555);
            string message = CreateMessage(dataQueryType);
            string status = "";
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            CancellationToken cts = new();
            using (var client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(ipAddress, port).WaitAsync(timeout, cts);
                }
                catch (Exception ex)
                {
                    //https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient.connectasync?view=net-7.0
                    // TODO handle diffrent errors
                    if(ex is ArgumentNullException)
                        status = "Error: Nie podano adresu IP!";
                    else if(ex is SocketException)
                        status = "Error: Nie można nawiązać połączenia z rozmieniarką!";
                    else if(ex is ObjectDisposedException)
                        status = "Error: " + ex.Message;
                    else if(ex is ArgumentOutOfRangeException)
                        status = "Error: Podany port: "+port+" nie istnieje!";
                    else if(ex is TaskCanceledException)
                        status = "Error: " + ex.Message;
                    else
                    status = "Error: " + ex.Message;
                }
                if (client.Connected)
                {
                    using var stream = client.GetStream();
                    byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                    await stream.WriteAsync(dataToSend);
                    byte[] dataToReceive = new byte[client.ReceiveBufferSize];
                    await Task.Delay(1000);
                    int bytesRead = await stream.ReadAsync(dataToReceive.AsMemory(0, client.ReceiveBufferSize));
                    status = Encoding.ASCII.GetString(dataToReceive, 0, bytesRead);
                }
            }
            if(status == "")
                status = "Error: Unknown error";
            return status;
        }
    }
}
