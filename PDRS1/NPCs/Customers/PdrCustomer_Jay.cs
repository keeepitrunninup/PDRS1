using System;
using MelonLoader;
using S1API.Entities;
using S1API.Messaging;
using S1API.Saveables;
using S1API.GameTime;
using S1API.Products;
using S1API.Economy;
using S1API.Properties;
using S1API.Map.Buildings;
using S1API.Map.ParkingLots;
using S1API.Vehicles;
using PDRS1.Systems;
using S1API.Map;
using UnityEngine;

namespace PDRS1.NPCs.Customers
{
    public sealed class PdrCustomerJay : NPC
    {
        public override bool IsPhysical => true;

        [SaveableField("JayData")] private JayData _data = new JayData();

        public PdrCustomerJay() : base(
            id: "pdr_customer_jay",
            firstName: "Jay",
            lastName: "Walker",
            icon: null)
        {
        }

        protected override void ConfigurePrefab(NPCPrefabBuilder builder)
        {
            MelonLogger.Msg("Configuring prefab for Jay Walker");

            var manorParking = ParkingLotRegistry.Get<ManorParking>();
            var docksWarehouse = Building.Get<DocksShippingContainer>();

            // Meeting spot - at the Docks
            Vector3 meetingSpot = new Vector3(-78.67f, -0.60f, -60.95f);
            // Spawn position - also at Docks
            Vector3 spawnPos = new Vector3(-80.0f, -0.60f, -65.0f);

            builder.WithIdentity("pdr_customer_jay", "Jay", "Walker")
                .WithAppearanceDefaults(av =>
                {
                    av.Gender = 0.0f;
                    av.Height = 1.0f;
                    av.Weight = 0.36f;
                    av.SkinColor = new Color32(150, 120, 95, 255);
                    av.LeftEyeLidColor = av.SkinColor;
                    av.RightEyeLidColor = av.SkinColor;
                    av.EyeBallTint = Color.white;
                    av.PupilDilation = 0.66f;
                    av.EyebrowScale = 0.85f;
                    av.EyebrowThickness = 0.6f;
                    av.EyebrowRestingHeight = 0.1f;
                    av.EyebrowRestingAngle = 0.05f;
                    av.LeftEye = (0.5f, 0.5f);
                    av.RightEye = (0.5f, 0.5f);
                    av.HairColor = new Color(0.1f, 0.1f, 0.1f);
                    av.HairPath = "Avatar/Hair/Spiky/Spiky";
                    av.WithBodyLayer("Avatar/Layers/Top/T-Shirt", Color.red);
                    av.WithBodyLayer("Avatar/Layers/Bottom/Jeans", new Color(0.15f, 0.2f, 0.3f));
                    av.WithAccessoryLayer("Avatar/Accessories/Feet/Sneakers/Sneakers", Color.white);
                })
                .WithSpawnPosition(spawnPos)
                .EnsureCustomer()
                .WithCustomerDefaults(cd =>
                {
                    cd.WithSpending(minWeekly: 400f, maxWeekly: 1000f)
                        .WithOrdersPerWeek(1, 4)
                        .WithPreferredOrderDay(Day.Sunday)
                        .WithOrderTime(900)
                        .WithStandards(CustomerStandard.VeryLow)
                        .AllowDirectApproach(true)
                        .GuaranteeFirstSample(true)
                        .WithCallPoliceChance(0.15f)
                        .WithDependence(baseAddiction: 0.1f, dependenceMultiplier: 1.1f)
                        .WithPreferredProperties(Property.Munchies, Property.Energizing, Property.Cyclopean);
                })
                .WithRelationshipDefaults(r =>
                {
                    r.WithDelta(1.5f)
                        .SetUnlocked(true)
                        .SetUnlockType(NPCRelationship.UnlockType.DirectApproach);
                });
        }



        protected override void OnCreated()
        {
            try
            {
                base.OnCreated();
                Appearance.Build();
                
                MelonLogger.Msg("[PDRS1] Jay Walker OnCreated called");
                
                TimeManager.OnDayPass += OnDayPassHandler;
                
                if (Customer != null)
                {
                    Customer.OnDealCompleted += OnDealCompleted;
                    MelonLogger.Msg("[PDRS1] Customer events wired");
                }
                
                Aggressiveness = 3f;
                Region = Region.Docks;
                
                Schedule.Enable();
                
                MelonLogger.Msg("[PDRS1] Jay Walker created successfully");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PDRS1] OnCreated failed: {ex.Message}");
                MelonLogger.Error($"[PDRS1] StackTrace: {ex.StackTrace}");
            }
        }

        public void TriggerManualMessage()
        {
            MelonLogger.Msg("[PDRS1] TriggerManualMessage called");
            SendDailyMessage();
        }

        private void OnDayPassHandler()
        {
            MelonLogger.Msg("[PDRS1] OnDayPassHandler triggered");
            SendDailyMessage();
        }
        
        private void SendDailyMessage()
        {
            MelonLogger.Msg($"[PDRS1] SendDailyMessage - Intro completed: {_data.IntroCompleted}");

            if (!_data.IntroCompleted)
            {
                SendIntroMessage();
                return;
            }

            if (!_data.WaitingForResponse)
            {
                SendDealOffer();
            }
            else
            {
                MelonLogger.Msg("[PDRS1] Waiting for player response");
            }
        }

        private void SendIntroMessage()
        {
            MelonLogger.Msg($"[PDRS1] Sending intro message {_data.IntroProgress}");

            if (_data.IntroProgress == 0)
            {
                SendTextMessage("Yo... I heard you got that good stuff", new[]
                {
                    new Response { Label = "INTRO_0", Text = "Yo, what's good?" }
                });
            }
            else if (_data.IntroProgress == 1)
            {
                SendTextMessage("I'm looking to buy regularly if you're reliable", new[]
                {
                    new Response { Label = "INTRO_1", Text = "Bet" }
                });
            }
            else if (_data.IntroProgress == 2)
            {
                SendTextMessage("I'll hit you up when I need something");
                _data.IntroCompleted = true;
                MelonLogger.Msg("[PDRS1] Intro completed!");
            }

            _data.IntroProgress++;
        }

        private void SendDealOffer()
        {
            MelonLogger.Msg("[PDRS1] SendDealOffer called");
            
            CustomerIntentCache.GenerateIntent("pdr_customer_jay");
            var drug = CustomerIntentCache.GetIntent("pdr_customer_jay");
            _data.CurrentDrug = drug.ToString();
            _data.WaitingForResponse = true;

            SendTextMessage($"Yo, you got any {drug}? Meet me at the Docks.", new[]
            {
                new Response { Label = "ACCEPT_DEAL", Text = "Yeah, I'll be there" },
                new Response { Label = "DECLINE_DEAL", Text = "Not today" }
            });
            
            MelonLogger.Msg($"[PDRS1] Jay asking for {drug}");
        }

        protected override void OnResponseLoaded(Response response)
        {
            MelonLogger.Msg($"[PDRS1] OnResponseLoaded: {response.Label}");

            switch (response.Label)
            {
                case "INTRO_0":
                case "INTRO_1":
                    response.OnTriggered = ProgressIntro;
                    break;
                case "ACCEPT_DEAL":
                    response.OnTriggered = AcceptDeal;
                    break;
                case "DECLINE_DEAL":
                    response.OnTriggered = DeclineDeal;
                    break;
            }
        }

        private void ProgressIntro()
        {
            MelonLogger.Msg("[PDRS1] Player responded to intro");
        }

        private void AcceptDeal()
        {
            MelonLogger.Msg("[PDRS1] ========================================");
            MelonLogger.Msg("[PDRS1] Player accepted deal!");
            MelonLogger.Msg("[PDRS1] ========================================");

            SendTextMessage("Bet. I'm heading there now.");
            _data.WaitingForResponse = false;
            
            Vector3 meetingSpot = new Vector3(-78.67f, -0.60f, -60.95f);
            Goto(meetingSpot);
            // Jay will walk to the meeting spot via his schedule
            // Player needs to go meet him there and interact in person
        }

        private void DeclineDeal()
        {
            MelonLogger.Msg("[PDRS1] Player declined deal");
            SendTextMessage("Aight, maybe next time");
            _data.WaitingForResponse = false;
            CustomerIntentCache.ClearIntent("pdr_customer_jay");
            _data.CurrentDrug = string.Empty;
        }

        private void OnDealCompleted()
        {
            MelonLogger.Msg("[PDRS1] ========================================");
            MelonLogger.Msg("[PDRS1] DEAL COMPLETED!");
            MelonLogger.Msg("[PDRS1] ========================================");

            CustomerLoyaltySystem.RegisterPurchase("pdr_customer_jay");
            var tier = CustomerLoyaltySystem.GetTier("pdr_customer_jay");
            
            MelonLogger.Msg($"[PDRS1] Jay's loyalty tier: {tier}");
            SendTextMessage($"Thanks bro. That {_data.CurrentDrug} is fire 🔥");

            CustomerIntentCache.ClearIntent("pdr_customer_jay");
            _data.CurrentDrug = string.Empty;
        }

        [Serializable]
        private class JayData
        {
            public int IntroProgress = 0;
            public bool IntroCompleted = false;
            public bool WaitingForResponse = false;
            public string CurrentDrug = string.Empty;
        }
    }
}