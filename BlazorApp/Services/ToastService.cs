namespace BlazorApp.Services
{
    public class ToastService
    {
        public event Action<string, string>? OnShow;

        public void ShowSuccess(string message)
        {
            OnShow?.Invoke(message, "success");
        }

        public void ShowWarning(string message)
        {
            OnShow?.Invoke(message, "warning");
        }

        public void ShowError(string message)
        {
            OnShow?.Invoke(message, "error");
        }

        public void ShowInfo(string message)
        {
            OnShow?.Invoke(message, "info");
        }
    }
}
