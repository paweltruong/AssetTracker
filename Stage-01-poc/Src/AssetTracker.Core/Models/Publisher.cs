namespace AssetTracker.Core.Models
{
    public sealed class Publisher : IEquatable<Publisher>
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public bool Equals(Publisher other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // Compare based on what makes a Publisher unique
            return Name == other.Name && Url == other.Url;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Publisher);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Url);
        }
    }
}
