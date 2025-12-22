using Blazored.LocalStorage;

namespace Boutique.Client.Services
{
    // Theme service for managing light/dark mode preferences
    public class ThemeService
    {
        // Local storage service for persisting theme preference
        private readonly ILocalStorageService _localStorage;
        // Current theme state (false = light, true = dark)
        private bool _isDarkMode = false;

        // Event for notifying components of theme changes
        public event Action? OnThemeChanged;

        // Constructor with dependency injection for local storage
        public ThemeService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // Public property to check current theme state
        public bool IsDarkMode => _isDarkMode;

        // Initialize theme service and load saved preference
        public async Task InitializeAsync()
        {
            // Load theme preference from local storage
            _isDarkMode = await _localStorage.GetItemAsync<bool>("isDarkMode");
            ApplyTheme();
        }

        // Toggle between light and dark themes
        public async Task ToggleThemeAsync()
        {
            // Switch theme state
            _isDarkMode = !_isDarkMode;
            // Save preference to local storage
            await _localStorage.SetItemAsync("isDarkMode", _isDarkMode);
            // Apply theme changes
            ApplyTheme();
            // Notify components of theme change
            OnThemeChanged?.Invoke();
        }

        // Apply theme changes (implementation handled by CSS classes in UI)
        private void ApplyTheme()
        {
            // Theme is applied via CSS classes in the UI components
        }
    }
}