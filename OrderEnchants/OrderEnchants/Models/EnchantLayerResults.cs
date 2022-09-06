using System.Collections.Generic;

namespace OrderEnchants.Models
{
    internal class EnchantLayerResults
    {
        public List<double> XpList { get; set; }
        public List<double> MaxStep { get; set; }

        public EnchantLayerResults(List<double> xpList, List<double> maxStep)
        {
            XpList = xpList;
            MaxStep = maxStep;
        }
    }
}
