namespace GeneralTools.Models
{
    public class MaintenanceMessage  
    {
        public bool IsComing { get; set; }

        public bool IsActive { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool LogonDisabledCore { private get; set; }


        public bool LogonDisabled { get { return LogonDisabledCore && IsActive; } }
        
        public bool IsAvailable { get { return IsComing || IsActive; } }
    }
}
