namespace ns_Data
{
    internal record CommunicationUnit(string Subject)
    {
        public string Subject { get; set; } = Subject;
        public object? Attachment { get; set; }
    }
}
