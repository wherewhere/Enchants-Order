using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The step of Enchant.
    /// </summary>
    public
#if WINRT
        sealed
#endif
        class EnchantmentStep(int step) :
#if !WINRT
        List<IEnchantment>,
#endif
        IEnchantmentStep
    {
        /// <summary>
        /// The index of step.
        /// </summary>
        public int Step { get; set; } = step;

#if WINRT
        internal List<IEnchantment> Enchantments { get; set; }

        /// <inheritdoc/>
        public int Count => Enchantments.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => ((ICollection<IEnchantment>)Enchantments).IsReadOnly;

        /// <inheritdoc/>
        public IEnchantment this[int index]
        {
            get => Enchantments[index];
            set => Enchantments[index] = value;
        }
#endif

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new();
            _ = builder.Append($"Step {Step}:");
            int half = Count / 2;
            for (int i = half; i > 0; i--)
            {
                bool flag = Count == 2;
                int index = (half * 2) - (i * 2);
                _ = builder.Append($" {(flag ? "" : "(")}{this[index]} + {this[index + 1]}{(flag ? "" : ")")}{(index + 2 == Count ? "" : " +")}");
            }
            if (Count % 2 == 1)
            {
                _ = builder.Append($" {this.Last()}");
            }
            return builder.ToString();
        }

#if WINRT
        /// <inheritdoc/>
        public int IndexOf(IEnchantment item) => Enchantments.IndexOf(item);

        /// <inheritdoc/>
        public void Insert(int index, IEnchantment item) => Enchantments.Insert(index, item);

        /// <inheritdoc/>
        public void RemoveAt(int index) => Enchantments.RemoveAt(index);

        /// <inheritdoc/>
        public void Add(IEnchantment item) => Enchantments.Add(item);

        /// <inheritdoc/>
        public void Clear() => Enchantments.Clear();

        /// <inheritdoc/>
        public bool Contains(IEnchantment item) => Enchantments.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(IEnchantment[] array, int arrayIndex) => Enchantments.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public bool Remove(IEnchantment item) => Enchantments.Remove(item);

        /// <inheritdoc/>
        public IEnumerator<IEnchantment> GetEnumerator() => Enchantments.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Enchantments).GetEnumerator();
#endif
    }
}
