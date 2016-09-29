using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers
{
    internal struct NameAndLocation
    {
        public NameAndLocation(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        public string Name { get; }
        public Location Location { get; }
    }
}