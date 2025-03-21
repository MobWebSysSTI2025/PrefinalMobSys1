﻿using PrefinalMobSys1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Data
{
    public class AppShellContext
    {
        public static AppShellContext Instance { set; get; }

        public AppShellContext()
        {
            Instance = this;
        }

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get
            {
                return _isUserLoggedIn;
            }
            set
            {
                if (_isUserLoggedIn != value)
                {
                    _isUserLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


        public User CurrentUser { get; set; }
    }
}
