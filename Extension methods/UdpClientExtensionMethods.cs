using System.Net.Sockets;
using System.Net;

namespace ns_ExtensionMethods
{
    internal static class UdpClientExtensionMethods
    {
        public static Exception? TryConnect(this UdpClient udpClient, string iPAddressStr, int port)
        {
            try
            {
                udpClient.Connect(IPAddress.Parse(iPAddressStr), port);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public static async Task<Exception?> TrySendAsync(this UdpClient udpClient, byte[] data)
        {
            try
            {
                await udpClient.SendAsync(data);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public static async Task<(UdpReceiveResult, Exception?)> TryReceiveAsync(this UdpClient udpClient, TimeSpan awaitTime)
        {
            try
            {
                return (await udpClient.ReceiveAsync(new CancellationTokenSource(awaitTime).Token), null);
            }
            catch (Exception ex)
            {
                return (new(), ex);
            }
        }
    }
}
