using MegoTimeTracker.Data;
using MegoTimeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace MegoTimeTracker.Repos {
    public class DbRepository : IDbRepository {
        private readonly StudentDb _db;
        public DbRepository(StudentDb db) => _db = db;
        public bool IsActive(int id) {
            Student? student = StudentExists(id);
            if (student == default) throw new ArgumentException("Student does not exist in DB", nameof(id));

            return student.Attendances.Last().IsActive;
        }
        public async Task<bool> LogState(int id, string time) {
            Student? student = StudentExists(id);
            if (student == default) return false;

            var prevAttendence = _db.Attendances
                .Where(a => a.StudentId == student.Id)
                .OrderBy(a => a.Time)
                .LastOrDefault();

            Attendance attendance = new() {
                StudentId = id,
                Time = DateTime.Parse(time),
                Student = student,
                IsActive = prevAttendence == default || !prevAttendence.IsActive
            };

            if (prevAttendence != default) {
                // if a day has passed, set to login regardless of if they remembered to logout
                if (DateTime.Now.Date > prevAttendence.Time.Date) attendance.IsActive = true;
            }

            await _db.Attendances.AddAsync(attendance);
            student.Attendances.Add(attendance);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<Student?> GetStudentAsync(int id) {
            var student = await _db.Students.FirstAsync(x => x.IdNumber == id);
            if (student == default) return default;
            student.Attendances = await _db.Attendances.Where(a => a.StudentId == student.Id).ToListAsync();
            return student;
        }
        public async Task<List<Student>> GetStudentsAsync() {
            var students = _db.Students;
            var attendances = _db.Attendances;
            foreach (var student in students) {
                student.Attendances = await _db.Attendances
                    .Where(a => a.StudentId == student.Id)
                    .ToListAsync();
            }
            return await students.ToListAsync();
        }
        public async Task<bool> AddStudentAsync(int studentId) {
            // wrong ID number
            if (studentId.ToString().Length != 9) {
                return false;
            }
            // number exists in DB
            if (_db.Students.Where(s => s.IdNumber == studentId).Any()) {
                return false;
            }
            Student student = new() {
                IdNumber = studentId,
                Attendances = new()
            };
            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();
            return true;
        }
        private Student? StudentExists(int id) {
            return _db.Students.FirstOrDefault(s => s.IdNumber == id);
        }
        public async Task DeleteStudentAsync(int userId) {
            var studentInDb = await _db.Students.FirstOrDefaultAsync(s => s.IdNumber == userId);
            if (studentInDb != null) {
                _db.Students.Remove(studentInDb);
                foreach (var attendance in _db.Attendances.Where(a => a.StudentId == studentInDb.Id)) {
                    _db.Attendances.Remove(attendance);
                }
                await _db.SaveChangesAsync();
            }
        }
    }
}