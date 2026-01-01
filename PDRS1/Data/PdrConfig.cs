
using System;
using S1API.Entities;

namespace PDRS1.Data
{
    public enum CustomerLoyaltyTier
    {
        New,
        Regular,
        Loyal,
        VIP
    }
    /// <summary>
    /// Data class for PdrCustomer_Jay to store persistent data
    /// </summary>
    [Serializable]
    public class PdrCustomerJayData
    {
        /// <summary>
        /// Custom value for specific NPC behavior
        /// Add your own data fields here as needed
        /// </summary>
        public int CustomValue { get; set; } = 0;
    }

}