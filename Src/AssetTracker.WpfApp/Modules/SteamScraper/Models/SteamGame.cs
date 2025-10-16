using AssetTracker.WpfApp.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Models
{
    public class SteamGame : BindableBase
    {
        private int _appId;
        private string _name;
        private int _playtimeForever;
        private string _imgIconUrl;

        public int AppId
        {
            get => _appId;
            set
            {
                if (_appId != value)
                {
                    SetProperty(ref _appId, value);
                    OnPropertyChanged(nameof(FullImageUrl));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
               SetProperty(ref _name, value);
            }
        }

        public int PlaytimeForever
        {
            get => _playtimeForever;
            set
            {
                if (_playtimeForever != value)
                {
                    SetProperty(ref _playtimeForever, value);
                    OnPropertyChanged(nameof(PlaytimeHours));
                }
            }
        }

        public string ImgIconUrl
        {
            get => _imgIconUrl;
            set
            {
                if (_imgIconUrl != value)
                {
                    SetProperty(ref _imgIconUrl, value);
                    OnPropertyChanged(nameof(FullImageUrl));
                }
            }
        }

        // Calculated properties for easier binding
        public string FullImageUrl =>
            $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/{AppId}/{ImgIconUrl}.jpg";

        public string PlaytimeHours => $"{PlaytimeForever} hrs";
    }
    public class SteamGamesResponse
    {
        public ResponseData Response { get; set; }
    }

    public class ResponseData
    {
        public int GameCount { get; set; }
        public List<SteamGame> Games { get; set; }
    }
}
