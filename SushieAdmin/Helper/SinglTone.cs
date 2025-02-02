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
                    OnPropertyChanged(nameof(Auth)); // Уведомляем UI
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
            ApiClient = new ApiClient("https://8af3ec20-7d20-478f-8c7b-c0daebfec9d1.tunnel4.com");
            Auth = false;
        }
    }
}
