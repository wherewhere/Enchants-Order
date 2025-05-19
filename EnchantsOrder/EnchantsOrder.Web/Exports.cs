using EnchantsOrder;
using EnchantsOrder.Models;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Exports for JavaScript.
/// </summary>
public static partial class Exports
{
    /// <inheritdoc cref="EnchantsOrder.EnchantsOrder.Ordering(System.Collections.Generic.IEnumerable{IEnchantment}, int)" />
    [JSExport]
    public static string Ordering(JSObject[] wantedList, int initialPenalty) =>
        JsonSerializer.Serialize(wantedList.Select(AsJSEnchantment).Ordering(initialPenalty), SourceGenerationContext.Default.OrderingResults);

    private readonly struct JSEnchantment(JSObject @object) : IEnchantment
    {
        /// <inheritdoc/>
        public string Name
        {
            get => @object.GetPropertyAsString("name");
            set => @object.SetProperty("name", value);
        }

        /// <inheritdoc/>
        public int Level
        {
            get => @object.GetPropertyAsInt32("level");
            set => @object.SetProperty("level", value);
        }

        /// <inheritdoc/>
        public int Weight
        {
            get => @object.GetPropertyAsInt32("weight");
            set => @object.SetProperty("weight", value);
        }

        /// <inheritdoc/>
        public long Experience => (long)Level * Weight;

        /// <inheritdoc/>
        public int CompareTo(IEnchantment other)
        {
            if (other is null) { return -1; }
            int value = Experience.CompareTo(other.Experience);
            if (value == 0)
            {
                value = Level.CompareTo(other.Level);
                if (value == 0)
                {
                    value = Name.CompareTo(other.Name);
                }
            }
            return value;
        }

        /// <inheritdoc/>
        public bool Equals(IEnchantment other) => CompareTo(other) == 0;
    }

    private static IEnchantment AsJSEnchantment(this JSObject @object) => new JSEnchantment(@object);

    [JsonSerializable(typeof(OrderingResults))]
    private partial class SourceGenerationContext : JsonSerializerContext;
}
