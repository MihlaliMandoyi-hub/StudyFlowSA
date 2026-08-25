using SQLite;
using StudyFlowSA.Models;

namespace StudyFlowSA.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        // Call this once before any other method runs
        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "studyflowsa.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<StudentProfile>();
            await _database.CreateTableAsync<Subject>();
            await _database.CreateTableAsync<StudyTask>();
            await _database.CreateTableAsync<StudySession>();
        }

        // ---------- StudentProfile ----------

        public async Task<StudentProfile?> GetProfileAsync()
        {
            await InitAsync();
            return await _database!.Table<StudentProfile>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveProfileAsync(StudentProfile profile)
        {
            await InitAsync();
            if (profile.Id != 0)
                return await _database!.UpdateAsync(profile);
            return await _database!.InsertAsync(profile);
        }

        // ---------- Subject ----------

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            await InitAsync();
            return await _database!.Table<Subject>().OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<Subject?> GetSubjectAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<Subject>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> SaveSubjectAsync(Subject subject)
        {
            await InitAsync();
            if (subject.Id != 0)
                return await _database!.UpdateAsync(subject);
            return await _database!.InsertAsync(subject);
        }

        public async Task<int> DeleteSubjectAsync(Subject subject)
        {
            await InitAsync();
            return await _database!.DeleteAsync(subject);
        }

        // ---------- StudyTask ----------

        public async Task<List<StudyTask>> GetTasksAsync()
        {
            await InitAsync();
            return await _database!.Table<StudyTask>().OrderBy(t => t.DueDate).ToListAsync();
        }

        public async Task<List<StudyTask>> GetTasksBySubjectAsync(int subjectId)
        {
            await InitAsync();
            return await _database!.Table<StudyTask>()
                .Where(t => t.SubjectId == subjectId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<StudyTask?> GetTaskAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<StudyTask>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> SaveTaskAsync(StudyTask task)
        {
            await InitAsync();
            if (task.Id != 0)
                return await _database!.UpdateAsync(task);
            return await _database!.InsertAsync(task);
        }

        public async Task<int> DeleteTaskAsync(StudyTask task)
        {
            await InitAsync();
            return await _database!.DeleteAsync(task);
        }

        // ---------- StudySession ----------

        public async Task<List<StudySession>> GetSessionsAsync()
        {
            await InitAsync();
            return await _database!.Table<StudySession>().OrderByDescending(s => s.StartTime).ToListAsync();
        }

        public async Task<List<StudySession>> GetSessionsBetweenAsync(DateTime start, DateTime end)
        {
            await InitAsync();
            return await _database!.Table<StudySession>()
                .Where(s => s.StartTime >= start && s.StartTime <= end)
                .ToListAsync();
        }

        public async Task<int> SaveSessionAsync(StudySession session)
        {
            await InitAsync();
            if (session.Id != 0)
                return await _database!.UpdateAsync(session);
            return await _database!.InsertAsync(session);
        }

        public async Task<int> DeleteSessionAsync(StudySession session)
        {
            await InitAsync();
            return await _database!.DeleteAsync(session);
        }
    }
}