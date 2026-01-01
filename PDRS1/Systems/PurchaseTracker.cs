using S1API.Entities;
using MelonLoader;
using System.Collections.Generic;
using S1API.Economy;
namespace PDRS1.Systems
{
    public static class PurchaseTracker
    {
        // Runtime-only tracking
        private static readonly Dictionary<string, int> _purchaseCounts =
            new Dictionary<string, int>();

        public static void RegisterCustomer(NPC npc)
        {
            if (npc.Customer == null)
                return;
            
            string id = npc.ID;
            
            if (!_purchaseCounts.ContainsKey(id))
                _purchaseCounts[id] = 0;

            npc.Customer.OnDealCompleted += () =>
            {
                _purchaseCounts[id]++;
                
                MelonLogger.Msg(
                    $"[PDRS1] {id} completed a purchase. Total: {_purchaseCounts[id]}"
                );
            };
        }

        public static int GetPurchaseCount(string npcId)
        {
            return _purchaseCounts.TryGetValue(npcId, out int count)
                ? count
                : 0;
        }
    }
}