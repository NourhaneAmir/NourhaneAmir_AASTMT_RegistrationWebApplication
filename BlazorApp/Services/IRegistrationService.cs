using StudentRegistration.Shared.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public Registration? Data { get; set; }
    }

    public interface IRegistrationService
    {
        Task<List<Class>> GetAvailableClassesAsync(int registrationNumber);
        Task<List<Class>> GetRegisteredClassesAsync(int registrationNumber);
        Task<RegistrationResult> RegisterForClassAsync(int registrationNumber, int classId);
        Task<RegistrationResult> DropClassAsync(int registrationNumber, int classId);
        Task<ScheduleDto> GetStudentScheduleAsync(int registrationNumber);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly HttpClient _httpClient;

        public RegistrationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Class>> GetAvailableClassesAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<Class>>($"api/Registration/available-classes/{registrationNumber}")
                ?? new List<Class>();
        }

        public async Task<List<Class>> GetRegisteredClassesAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<List<Class>>($"api/Registration/registered-classes/{registrationNumber}")
                ?? new List<Class>();
        }

        public async Task<RegistrationResult> RegisterForClassAsync(int registrationNumber, int classId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Registration/register", new
            {
                StudentRegistrationNumber = registrationNumber,
                ClassId = classId
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegistrationResult>() ?? new RegistrationResult();
            }
            else
            {
                return new RegistrationResult { Success = false, Message = "Registration failed" };
            }
        }

        public async Task<RegistrationResult> DropClassAsync(int registrationNumber, int classId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Registration/drop", new
            {
                StudentRegistrationNumber = registrationNumber,
                ClassId = classId
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegistrationResult>() ?? new RegistrationResult();
            }
            else
            {
                return new RegistrationResult { Success = false, Message = "Drop failed" };
            }
        }

        public async Task<ScheduleDto> GetStudentScheduleAsync(int registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<ScheduleDto>($"api/Registration/schedule/{registrationNumber}")
                ?? new ScheduleDto();
        }
    }

    public class ScheduleDto
    {
        public List<Class> Classes { get; set; } = new List<Class>();
        public int TotalCreditHours { get; set; }
        public string CurrentSemester { get; set; } = string.Empty;
    }
}
