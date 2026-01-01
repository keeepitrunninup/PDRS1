using MelonLoader;
using S1API.Entities;
using PDRS1.NPCs.Customers;
using UnityEngine;

[assembly: MelonInfo(typeof(PDRS1.Core), "PDRS1", "1.0.0", "Jack")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PDRS1
{
    /// <summary>
    /// Main entry point for the PDRS1 mod.
    /// Responsible for registering NPCs once the game scene is loaded.
    /// </summary>
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
            {
                return;
            }

            RegisterNpcs();
        }

        public override void OnUpdate()
        {
            // F8 - Trigger Jay's test message
            if (UnityEngine.Input.GetKeyDown(KeyCode.F8))
            {
                MelonLogger.Msg("[PDRS1] F8 PRESSED - Triggering Jay's test message");

                var jay = NPC.Get<PdrCustomerJay>();
                if (jay != null)
                {
                    jay.SendTextMessage("Jay Walker created successfully");
                    MelonLogger.Msg("[PDRS1] Test message triggered");
                }
                else
                {
                    MelonLogger.Error("[PDRS1] Could not find Jay!");
                }
            }
        }

        private static void RegisterNpcs()
        {
            NPC.Get<PdrCustomerJay>();
            MelonLogger.Msg("[PDRS1] Customer NPCs registered");
        }

        public override void OnApplicationQuit()
        {
            Instance = null;
        }
    }
}