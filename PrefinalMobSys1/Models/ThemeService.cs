using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Model
{
    public class ThemeService
    {
        private readonly IJSRuntime _js;

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetThemeAsync(string theme)
        {
            await _js.InvokeVoidAsync("setTheme", theme);
        }
    }
}
