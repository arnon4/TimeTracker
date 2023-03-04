using System.ComponentModel.DataAnnotations.Schema;

namespace MegoTimeTracker.Models {
    public class Attendance {
        public int Id { get; set; }
        [ForeignKey("Student")]
        public virtual int StudentId { get; set; }
        public virtual Student Student {get; set;} = default!;
        public DateTime Time { get; set; }
        public bool IsActive { get; set; }
    }
}