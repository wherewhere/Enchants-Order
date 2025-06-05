using System;
using System.Collections;
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
#if WINRT
    sealed
#endif
    public class EnchantmentStep :
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
        /// The index of step.
        /// </summary>
        private readonly int step;

#if WINRT
        /// <summary>
        /// The list of enchantments in this step.
        /// </summary>
        private readonly List<IEnchantment> enchantments;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        public EnchantmentStep(int step) : base()
        {
            this.step = step;
            enchantments = [];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        /// <param name="capacity">The initial capacity of the list.</param>
        public EnchantmentStep(int step, int capacity)
        {
            this.step = step;
            enchantments = new(capacity);
        }

        /// <inheritdoc/>
        public int Count => enchantments.Count;
        
        /// <inheritdoc/>
        int IReadOnlyCollection<IEnchantment>.Count => ((IReadOnlyCollection<IEnchantment>)enchantments).Count;

        /// <inheritdoc/>
        public bool IsReadOnly => ((ICollection<IEnchantment>)enchantments).IsReadOnly;

        /// <inheritdoc/>
        public IEnchantment this[int index]
        {
            get => enchantments[index];
            set => enchantments[index] = value;
        }

        /// <inheritdoc/>
        IEnchantment IReadOnlyList<IEnchantment>.this[int index] => ((IReadOnlyList<IEnchantment>)enchantments)[index];
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        public EnchantmentStep(int step) : base() => this.step = step;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        /// <param name="enchantments">The list of enchantments in this step.</param>
        public EnchantmentStep(int step, IEnumerable<IEnchantment> enchantments) : base(enchantments) => this.step = step;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        /// <param name="capacity">The initial capacity of the list.</param>
        public EnchantmentStep(int step, int capacity) : base(capacity) => this.step = step;
#endif

        /// <summary>
        /// Gets or sets the index of step.
        /// </summary>
        public int Step => step;

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
        public int IndexOf(IEnchantment item) => enchantments.IndexOf(item);

        /// <inheritdoc/>
        public void Insert(int index, IEnchantment item) => enchantments.Insert(index, item);

        /// <inheritdoc/>
        public void RemoveAt(int index) => enchantments.RemoveAt(index);

        /// <inheritdoc/>
        public void Add(IEnchantment item) => enchantments.Add(item);

        /// <inheritdoc/>
        public void Clear() => enchantments.Clear();

        /// <inheritdoc/>
        public bool Contains(IEnchantment item) => enchantments.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(IEnchantment[] array, int arrayIndex) => enchantments.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public bool Remove(IEnchantment item) => enchantments.Remove(item);

        /// <inheritdoc/>
        public IEnumerator<IEnchantment> GetEnumerator() => enchantments.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)enchantments).GetEnumerator();
#endif
    }
}
