using System.Windows.Media;

namespace AssetTracker.WpfApp.Common.Models
{
    public class TagItem
    {
        public string Name { get; set; }
        public Color Color { get; set; } = Colors.LightGray; // Default color
                                                             // ... your existing properties
    }
}
