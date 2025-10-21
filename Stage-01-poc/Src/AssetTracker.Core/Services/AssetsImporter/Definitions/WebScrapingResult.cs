using AssetTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.Core.Services.AssetsImporter.Definitions
{
    public class WebScrapingResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<OwnedAsset> OwnedAssets { get; set; }
        public string NextPageUrl { get; set; }

    }
}
