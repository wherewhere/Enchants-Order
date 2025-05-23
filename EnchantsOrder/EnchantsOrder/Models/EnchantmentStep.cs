﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The step of Enchant.
    /// </summary>
    /// <param name="step">The index of step.</param>
#if WINRT
    sealed
#endif
    public class EnchantmentStep(int step) :
#if WINRT
        IStringable,
#else
        List<IEnchantment>,
#endif
        IEnchantmentStep
#if !SILVERLIGHT && !WINDOWSPHONE && !NETFRAMEWORK || NET45_OR_GREATER
        , IReadOnlyList<IEnchantment>
#endif
    {
        /// <summary>
        /// Gets or sets the index of step.
        /// </summary>
        public int Step { get; set; } = step;

#if WINRT
        internal List<IEnchantment> Enchantments { get; set; }

        /// <inheritdoc/>
        public int Count => Enchantments.Count;
        
        /// <inheritdoc/>
        int IReadOnlyCollection<IEnchantment>.Count => ((IReadOnlyCollection<IEnchantment>)Enchantments).Count;

        /// <inheritdoc/>
        public bool IsReadOnly => ((ICollection<IEnchantment>)Enchantments).IsReadOnly;

        /// <inheritdoc/>
        public IEnchantment this[int index]
        {
            get => Enchantments[index];
            set => Enchantments[index] = value;
        }

        /// <inheritdoc/>
        IEnchantment IReadOnlyList<IEnchantment>.this[int index] => ((IReadOnlyList<IEnchantment>)Enchantments)[index];
#endif

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new();
            _ = builder.Append("Step ").Append(Step).Append(':');
            int half = Count / 2;
            for (int i = half; i > 0; i--)
            {
                bool flag = Count == 2;
                int index = (half * 2) - (i * 2);
                _ = builder.AppendFormat(" {0}{1} + {2}{3}{4}", flag ? "" : "(", this[index], this[index + 1], flag ? "" : ")", index + 2 == Count ? "" : " +");
            }
            if (Count % 2 == 1)
            {
                _ = builder.Append(' ').Append(this.Last());
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
