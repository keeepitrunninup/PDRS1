using MelonLoader;
using S1API.Entities;
using PDRS1.NPCs.Customers;
using S1API.Map.Buildings;
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
            // F8 - Make Jay come to Manor (easy to find location)
            if (UnityEngine.Input.GetKeyDown(KeyCode.F8))
            {
                MelonLogger.Msg("[PDRS1] F8 PRESSED - Sending Jay to Manor");

                var jay = NPC.Get<PdrCustomerJay>();
                if (jay != null)
                {
                    // Send Jay to Manor front door (easy location to find him)
                    Vector3 docksLocation = new Vector3(168.21f, -11f, -67.17f); // Manor entrance
                    MelonLogger.Msg($"[PDRS1] Sending Jay to Manor entrance: {docksLocation}");
                    
                    jay.Goto(docksLocation);
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