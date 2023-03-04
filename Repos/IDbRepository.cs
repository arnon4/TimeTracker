using MegoTimeTracker.Models;

namespace MegoTimeTracker.Repos {
    public interface IDbRepository {
        bool IsActive(int id);
        Task<bool> LogState(int id, string time);
        Task<List<Student>> GetStudentsAsync();
        Task<Student?> GetStudentAsync(int id);
        Task<bool> AddStudentAsync(int studentId);
        Task DeleteStudentAsync(int userId);
    }
}