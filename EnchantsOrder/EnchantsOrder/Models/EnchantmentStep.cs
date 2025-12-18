using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The information of an enchant step, which contains a list of enchantments in this step.
    /// </summary>
    /// <param name="step">The index of this step.</param>
    /// <param name="enchantments">The <see cref="IReadOnlyList{T}"/> of enchantments in this step.</param>
    public sealed class EnchantmentStep
#if SILVERLIGHT || WINDOWSPHONE7_0 || (NETFRAMEWORK && !NET45_OR_GREATER)
        (int step, params IList<IEnchantment> enchantments)
#else
        (int step, params IReadOnlyList<IEnchantment> enchantments)
#endif
        : IReadOnlyList<IEnchantment>
#if WINRT
        , IStringable
#endif
    {
#if !WINRT && !SILVERLIGHT && !WINDOWSPHONE7_0 && (!NETFRAMEWORK || NET45_OR_GREATER)
        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantmentStep"/> class.
        /// </summary>
        /// <param name="step">The index of step.</param>
        /// <param name="enchantments">The <see cref="IReadOnlyList{T}"/> of enchantments in this step.</param>
        [OverloadResolutionPriority(-1)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EnchantmentStep(int step, params IList<IEnchantment> enchantments) : this(step, (IReadOnlyList<IEnchantment>)enchantments)
        {
        }
#endif

        /// <inheritdoc/>
        public int Count => enchantments.Count;
        
        /// <inheritdoc/>
        public IEnchantment this[int index] => enchantments[index];

        /// <summary>
        /// Gets the index of this step.
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
                _ = builder.Append(' ');
                if (!flag)
                {
                    _ = builder.Append('(');
                }
                _ = builder.Append(enchantments[index])
                           .Append(" + ")
                           .Append(enchantments[index + 1]);
                if (!flag)
                {
                    _ = builder.Append(')');
                }
                if (index + 2 != Count)
                {
                    _ = builder.Append(" +");
                }
            }
            if ((Count & 1) == 1)
            {
                _ = builder.Append(' ').Append(enchantments[enchantments.Count - 1]);
            }
            return builder.ToString();
        }

        /// <inheritdoc/>
        public IEnumerator<IEnchantment> GetEnumerator() => enchantments.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)enchantments).GetEnumerator();
    }
}
