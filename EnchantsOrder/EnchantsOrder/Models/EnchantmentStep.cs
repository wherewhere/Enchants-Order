using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The step of Enchant.
    /// </summary>
    public
#if WINRT
        sealed
#endif
        class EnchantmentStep :
#if !WINRT
        List<IEnchantment>,
#endif
        IEnchantmentStep
    {
        /// <summary>
        /// The index of step.
        /// </summary>
        public int Step { get; set; }

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

        /// <summary>
        /// Initializes a new instance of <see cref="EnchantmentStep" />.
        /// </summary>
        public EnchantmentStep(int step) => Step = step;

        /// <inheritdoc/>
        public override string ToString()
        {
            string str = $"Step {Step}:";
            int half = Count / 2;
            for (int i = half; i > 0; i--)
            {
                bool istwo = Count == 2;
                int index = (half * 2) - (i * 2);
                str += $" {(istwo ? "" : "(")}{this[index]} + {this[index + 1]}{(istwo ? "" : ")")}";
            }
            if (Count % 2 == 1)
            {
                str += $"{(Count == 1 ? "" : " +")} {this.Last()}";
            }
            return str;
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
