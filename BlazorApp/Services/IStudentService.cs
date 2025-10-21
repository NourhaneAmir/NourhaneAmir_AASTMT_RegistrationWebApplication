using StudentRegistration.Shared.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public interface IStudentService
    {
        Task<Student?> GetStudentAsync(int registrationNumber);
        Task<bool> CanStudentRegisterAsync(int registrationNumber);
        Task<List<StudentCourseHistory>> GetStudentCourseHistoryAsync(int registrationNumber);
    }

    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Student?> GetStudentAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<Student>($"api/Students/{registrationNumber}");
        }

        public async Task<List<StudentCourseHistory>> GetStudentCourseHistoryAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<StudentCourseHistory>>($"api/Students/{registrationNumber}/course-history")
                ?? new List<StudentCourseHistory>();
        }
        public async Task<bool> CanStudentRegisterAsync(int registrationNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Students/{registrationNumber}/can-register");
                return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
