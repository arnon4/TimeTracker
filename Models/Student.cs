namespace MegoTimeTracker.Models {
    public class Student {
        public int Id { get; set; }
        public int IdNumber { get; set; }
        public virtual List<Attendance> Attendances { get; set; } = default!;
    }
}