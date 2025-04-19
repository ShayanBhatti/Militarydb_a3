namespace MyMvcApp.Models
{
    public class DashboardViewModel
    {
        public int TotalPersonnel { get; set; }
        public int TotalUnits { get; set; }
        public int TotalMissions { get; set; }
        public int ActivePersonnel { get; set; }
        public List<Mission> RecentMissions { get; set; } = new();
    }
}