namespace CkgDomainLogic.Insurance.Models
{
    // ReSharper disable InconsistentNaming
    public class TerminEntity
    {
        public string key { get; set; }

        public string boxArt { get; set; }

        public string title { get; set; }

        public string startDateString { get; set; }

        public string start { get; set; }
        
        public string end { get; set; }

        public bool allDay { get; set; }

        public int boxenTotal { get; set; }

        public int boxenAvailable { get { return boxenTotal - boxenOccupied; } }

        public int boxenOccupied { get; set; }

        public bool spaceAvailable { get { return boxenOccupied < boxenTotal; } }

        public bool isBlocker { get; set; }

        public bool isCurrentEditing { get; set; }

        public string backgroundColor { get { return spaceAvailable ? "green" : (isBlocker ? "gray" : "red"); } }

        public int startTimeHours { get; set; }
        public int startTimeMinutes { get; set; }
        public int endTimeHours { get; set; }
        public int endTimeMinutes { get; set; }
    }
}
