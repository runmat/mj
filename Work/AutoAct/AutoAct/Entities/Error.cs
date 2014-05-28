namespace AutoAct.Entities
{
    public class Error
    {
        public string field { get; set; }
        public string code { get; set; }
        public string timestamp { get; set; }
        public Message message { get; set; }
    }
}