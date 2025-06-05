using EnchantsOrder;
using EnchantsOrder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Exports for JavaScript.
/// </summary>
public static partial class Exports
{
    /// <inheritdoc cref="EnchantsOrder.EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)" />
    [JSExport]
    public static JSObject Ordering(JSObject[] wantedList, int initialPenalty) =>
        JSONParse(JsonSerializer.Serialize(wantedList.Select(AsJSEnchantment).Ordering(initialPenalty), SourceGenerationContext.Default.OrderingResults));

    /// <summary>
    /// The JSON.parse() static method parses a JSON string, constructing the JavaScript value or object described by the string. An optional reviver function can be provided to perform a transformation on the resulting object before it is returned.
    /// </summary>
    /// <param name="text">The string to parse as JSON. See the JSON object for a description of JSON syntax.</param>
    /// <returns>The Object, Array, string, number, boolean, or null value corresponding to the given JSON <paramref name="text"/>.</returns>
    [JSImport("globalThis.JSON.parse")]
    public static partial JSObject JSONParse(string text);

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

    private static JSEnchantment AsJSEnchantment(this JSObject @object) => new(@object);

    [JsonSerializable(typeof(OrderingResults))]
    private partial class SourceGenerationContext : JsonSerializerContext;
}
