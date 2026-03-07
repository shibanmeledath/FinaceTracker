using Microsoft.JSInterop;

namespace FinanceTracker.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isDarkMode = true;

        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkMode => _isDarkMode;

        public async Task InitializeAsync()
        {
            var savedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            if (string.IsNullOrEmpty(savedTheme))
            {
                // Default to dark mode if no preference saved
                _isDarkMode = true;
            }
            else
            {
                _isDarkMode = savedTheme == "dark";
            }
            await ApplyThemeAsync();
        }

        public async Task ToggleThemeAsync()
        {
            _isDarkMode = !_isDarkMode;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _isDarkMode ? "dark" : "light");
            await ApplyThemeAsync();
            OnThemeChanged?.Invoke();
        }

        private async Task ApplyThemeAsync()
        {
            await _jsRuntime.InvokeVoidAsync("setTheme", _isDarkMode ? "dark" : "light");
        }
    }
}
