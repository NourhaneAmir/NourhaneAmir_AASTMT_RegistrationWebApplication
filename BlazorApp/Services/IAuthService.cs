using StudentRegistration.Shared.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public interface IAuthService
    {
        Task<Student?> LoginAsync(int registrationNumber, string password);
        Task<Semester?> GetCurrentSemesterAsync();
        Task<bool> IsRegistrationOpenAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Student?> LoginAsync(int registrationNumber, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", new
                {
                    RegistrationNumber = registrationNumber,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Student>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        public async Task<Semester?> GetCurrentSemesterAsync()
        {
            return await _httpClient.GetFromJsonAsync<Semester>("api/Auth/current-semester");
        }

        public async Task<bool> IsRegistrationOpenAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Semesters/registration-status");
                return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
