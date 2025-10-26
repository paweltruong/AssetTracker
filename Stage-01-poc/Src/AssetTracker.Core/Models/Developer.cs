namespace AssetTracker.Core.Models
{
    public sealed class Developer : IEquatable<Developer>
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public bool Equals(Developer other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // Compare based on what makes a developer unique
            return Name == other.Name && Url == other.Url;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Developer);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Url);
        }
    }
}
