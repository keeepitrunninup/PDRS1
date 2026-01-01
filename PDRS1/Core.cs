using MelonLoader;
using S1API.Entities;
using PDRS1.NPCs.Customers;

[assembly: MelonInfo(typeof(PDRS1.Core), "PDRS1", "1.0.0", "Jack")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PDRS1
{
    public sealed class Core : MelonMod
    {
        public static Core? Instance { get; private set; }

        public override void OnInitializeMelon()
        {
            Instance = this;
            MelonLogger.Msg("[PDRS1] Initialized");
            base.OnInitializeMelon();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (!sceneName.Contains("Game"))
                return;

            MelonLogger.Msg("[PDRS1] Game scene loaded");
            RegisterCustomers();
        }

        public override void OnUpdate()
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F8))
            {
                MelonLogger.Msg("[PDRS1] F8 PRESSED");

                var jay = NPC.Get<PdrCustomerJay>();
                if (jay is PdrCustomerJay jayCustomer)
                {
                    jayCustomer.TriggerManualMessage();
                }
                else
                {
                    MelonLogger.Error("[PDRS1] Could not find Jay!");
                }
            }
        }

        private void RegisterCustomers()
        {
            var jay = NPC.Get<PdrCustomerJay>();
            MelonLogger.Msg($"[PDRS1] Registered: {(jay != null ? jay.ID : "NULL")}");
        }

        public override void OnApplicationQuit()
        {
            Instance = null;
        }
    }
}