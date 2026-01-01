using PDRS1.Data;
using System.Collections.Generic;

namespace PDRS1.Systems
{
    public static class CustomerLoyaltySystem
    {
        private static readonly Dictionary<string, int> _purchaseCounts = new();
        private static readonly Dictionary<string, CustomerLoyaltyTier> _tiers = new();

        public static void RegisterPurchase(string customerId)
        {
            if (!_purchaseCounts.ContainsKey(customerId))
                _purchaseCounts[customerId] = 0;

            _purchaseCounts[customerId]++;

            _tiers[customerId] = CalculateTier(_purchaseCounts[customerId]);
        }

        public static CustomerLoyaltyTier GetTier(string customerId)
        {
            return _tiers.TryGetValue(customerId, out var tier)
                ? tier
                : CustomerLoyaltyTier.New;
        }

        private static CustomerLoyaltyTier CalculateTier(int purchases)
        {
            if (purchases >= 20) return CustomerLoyaltyTier.VIP;
            if (purchases >= 10) return CustomerLoyaltyTier.Loyal;
            if (purchases >= 3)  return CustomerLoyaltyTier.Regular;
            return CustomerLoyaltyTier.New;
        }
    }
}