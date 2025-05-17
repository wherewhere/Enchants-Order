using System.Collections.Generic;

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The step of Enchant.
    /// </summary>
    public interface IEnchantmentStep : IList<IEnchantment>
    {
        /// <summary>
        /// The index of step.
        /// </summary>
        int Step { get; }
    }
}