using Boutique.Client.Models.DTOs;

namespace Boutique.Client.Services
{
    public class ProfileUpdateService
    {
        private readonly UserService _userService;
        private UserProfileDto? _cachedProfile;
        private DateTime _lastUpdate = DateTime.MinValue;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);

        public event Action<UserProfileDto?>? ProfileUpdated;

        public ProfileUpdateService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfileDto?> GetCurrentProfileAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _cachedProfile != null && DateTime.Now - _lastUpdate < _cacheExpiry)
            {
                return _cachedProfile;
            }

            try
            {
                var profile = await _userService.GetCurrentUserProfileAsync();
                if (profile != null)
                {
                    _cachedProfile = profile;
                    _lastUpdate = DateTime.Now;
                    ProfileUpdated?.Invoke(profile);
                }
                return profile;
            }
            catch
            {
                return _cachedProfile; // Return cached version if API fails
            }
        }

        public async Task UpdateProfileAsync(UpdateUserProfileDto updateDto)
        {
            await _userService.UpdateUserProfileAsync(updateDto);
            // Force refresh after update
            await GetCurrentProfileAsync(forceRefresh: true);
        }

        public void ClearCache()
        {
            _cachedProfile = null;
            _lastUpdate = DateTime.MinValue;
        }

        public UserProfileDto? GetCachedProfile()
        {
            return _cachedProfile;
        }
    }
}