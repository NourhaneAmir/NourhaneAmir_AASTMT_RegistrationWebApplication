using BlazorApp.Services;
using StudentRegistration.Shared.Models;

namespace BlazorApp
{
    public class AuthState
    {
        private readonly ILocalStorageService _localStorage;
        private const string StudentKey = "currentStudent";
        private const string SemesterKey = "currentSemester";

        public Student? CurrentStudent { get; private set; }
        public Semester? CurrentSemester { get; private set; }
        public bool IsAuthenticated => CurrentStudent != null;

        public event Action? OnChange;

        public AuthState(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InitializeAsync()
        {
            CurrentStudent = await _localStorage.GetItemAsync<Student>(StudentKey);
            CurrentSemester = await _localStorage.GetItemAsync<Semester>(SemesterKey);
            NotifyStateChanged();
        }

        public async Task SetStudentAsync(Student student)
        {
            CurrentStudent = student;
            await _localStorage.SetItemAsync(StudentKey, student);
            Console.WriteLine($"AuthState: Student stored in localStorage - {student.FullName}");
            NotifyStateChanged();
        }

        public async Task SetSemesterAsync(Semester semester)
        {
            CurrentSemester = semester;
            await _localStorage.SetItemAsync(SemesterKey, semester);
            Console.WriteLine($"AuthState: Semester stored in localStorage - {semester.FullSemesterName}");
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            CurrentStudent = null;
            CurrentSemester = null;
            await _localStorage.RemoveItemAsync(StudentKey);
            await _localStorage.RemoveItemAsync(SemesterKey);
            Console.WriteLine("AuthState: Logged out and cleared localStorage");
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
