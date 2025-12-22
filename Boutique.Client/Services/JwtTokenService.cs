using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Boutique.Client.Services
{
    public class JwtTokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";

        public JwtTokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveToken(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }

        public async Task<string> GetToken()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }

        public async Task RemoveToken()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
    }
}
