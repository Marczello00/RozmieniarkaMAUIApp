using RozmieniarkaApp.Enums;
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
                    switch (ex)
                    {
                        case ArgumentNullException:
                            status = "Error: Nie podano adresu IP bądź portu!";
                            break;
                        case ArgumentException:
                            status = "Error: Nie podano adresu IP bądź portu!";
                            break;
                        case TimeoutException:
                            status = "Error: Przekroczono czas próby połączenia!";
                            break;
                        case SocketException:
                            status = "Error: Nie można nawiązać połączenia z rozmieniarką!";
                            break;
                        case ObjectDisposedException:
                            status = "Error: " + ex.Message;
                            break;
                        case TaskCanceledException:
                            status = "Error: " + ex.Message;
                            break;
                        default:
                            status = "Error: " + ex.Message;
                            break;
                    }
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
