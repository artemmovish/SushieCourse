﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.Helper
{
    public class SinglTone
    {
        private static readonly Lazy<SinglTone> _instance = new(() => new SinglTone());

        public static SinglTone Instance => _instance.Value;

        public ApiClient ApiClient { get; private set; }

        private bool _auth;
        public bool Auth
        {
            get => _auth;
            set
            {
                if (_auth != value)
                {
                    _auth = value;
                    OnPropertyChanged(nameof(Auth));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private SinglTone()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://6da36731-1aa4-43b1-9d13-91979568b03e.tunnel4.com") };
            ApiClient = new ApiClient(httpClient);
            Auth = false;
        }
    }

}
