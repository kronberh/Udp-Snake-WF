namespace ns_DB
{
    public record BannedUser
    {
        public string IP { get; set; }
        public string Note { get; set; }
    }
}
