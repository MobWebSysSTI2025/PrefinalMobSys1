using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Helpers
{
	public static class ThemeHelper
	{
		public static void SetTheme(bool isDarkMode)
		{
			App.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
		}
	}
}
