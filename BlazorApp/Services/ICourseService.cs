using StudentRegistration.Shared.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetCoursesForStudentAsync(int registrationNumber);
        Task<List<Class>> GetAvailableClassesForStudentAsync(int registrationNumber);
        Task<List<Course>> GetEligibleCoursesForStudentAsync(int registrationNumber);
        Task<bool> IsStudentEligibleForCourseAsync(int registrationNumber, int courseId);
        Task<List<Class>> GetClassesByCourseAsync(int courseId);
        Task<List<Class>> GetAvailableClassesByCourseAsync(int courseId);

    }

    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;

        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Course>> GetCoursesForStudentAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<Course>>($"api/Courses/for-student/{registrationNumber}")
                ?? new List<Course>();
        }

        public async Task<List<Class>> GetAvailableClassesForStudentAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<Class>>($"api/Courses/for-student/{registrationNumber}/available-classes")
                ?? new List<Class>();
        }

        public async Task<List<Course>> GetEligibleCoursesForStudentAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<Course>>($"api/Courses/for-student/{registrationNumber}/eligible-courses")
                ?? new List<Course>();
        }

        public async Task<bool> IsStudentEligibleForCourseAsync(int registrationNumber, int courseId)
        {
            var response = await _httpClient.GetAsync($"api/Courses/{courseId}/is-eligible/{registrationNumber}");
            return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
        }
        public async Task<List<Class>> GetClassesByCourseAsync(int courseId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Class>>($"api/Courses/{courseId}/available-classes")
                    ?? new List<Class>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading available classes: {ex.Message}");
                return new List<Class>();
            }
        }

        public async Task<List<Class>> GetAvailableClassesByCourseAsync(int courseId)
        {
            return await _httpClient.GetFromJsonAsync<List<Class>>($"api/Courses/{courseId}/available-classes")
                ?? new List<Class>();
        }
    }
}
