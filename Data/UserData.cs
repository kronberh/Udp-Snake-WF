using System.Net;

namespace ns_Data
{
    internal record UserData
    {
        public string Nickname { get; set; }
        public IPEndPoint Controller { get; set; }
        public UserData(string Nickname, IPEndPoint Controller)
        {
            this.Nickname = Nickname;
            this.Controller = Controller;
        }
    }
}