using System;

namespace OrderEnchants.Models
{
    public class Enchantment : IComparable<Enchantment>
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public int Weight { get; set; }
        public int Experience => Level * Weight;

        public Enchantment(string name, int level, int weight)
        {
            Name = name;
            Level = level;
            Weight = weight;
        }

        public override string ToString() => $"{Name} {Level.GetLoumaNumber()}";

        public int CompareTo(Enchantment other) => Experience.CompareTo(other.Level);
    }
}
