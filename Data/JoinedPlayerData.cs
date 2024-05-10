using System.Text.Json;

namespace ns_Data
{
    internal class JoinedPlayerData
    {
        public string Name { get; set; }
        public int FieldSize { get; set; }
        public JoinedPlayerData(string Name, int FieldSize)
        {
            this.Name = Name;
            this.FieldSize = FieldSize;
        }
    }
}