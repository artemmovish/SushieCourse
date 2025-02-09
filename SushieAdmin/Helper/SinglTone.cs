using System;
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
            ApiClient = new ApiClient("https://bb9964d5-0626-47ed-a8c1-dcdc99a56d2d.tunnel4.com");
            Auth = false;
        }
    }
}
