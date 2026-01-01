using System;
using System.Collections.Generic;
using S1API.Products;

namespace PDRS1.Systems
{
    public static class CustomerIntentCache
    {
        private static readonly Dictionary<string, DrugType> _intents = new();

        private static readonly DrugType[] _possibleDrugs =
        {
            DrugType.Marijuana,
            DrugType.Cocaine,
            DrugType.Methamphetamine,
            DrugType.Shrooms
        };

        // One shared RNG (important)
        private static readonly Random _rng = new Random();

        public static void GenerateIntent(string customerId)
        {
            int index = _rng.Next(0, _possibleDrugs.Length);
            _intents[customerId] = _possibleDrugs[index];
        }

        public static DrugType GetIntent(string customerId)
        {
            return _intents.TryGetValue(customerId, out var drug)
                ? drug
                : DrugType.Marijuana;
        }

        public static void ClearIntent(string customerId)
        {
            _intents.Remove(customerId);
        }
    }
}