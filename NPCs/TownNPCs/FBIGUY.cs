using CalamityMod;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Magic;
using CalRemix.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;

namespace CalRemix.NPCs.TownNPCs
{
	[AutoloadHead]
	public class FBIGUY : ModNPC
	{
		private bool stash = false;
        /*
        // Time of day for traveller to leave (6PM)
        public const double despawnTime = 48600.0;

        // the time of day the traveler will spawn (double.MaxValue for no spawn)
        // saved and loaded with the world in TravelingMerchantSystem
        public static double spawnTime = double.MaxValue;
        public override bool PreAI()
        {
            if ((!Main.dayTime || Main.time >= despawnTime) && !IsNpcOnscreen(NPC.Center)) // If it's past the despawn time and the NPC isn't onscreen
            {
                // Here we despawn the NPC and send a message stating that the NPC has despawned
                // LegacyMisc.35 is {0) has departed!
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("LegacyMisc.35", NPC.FullName), 50, 125, 255);
                else ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.35", NPC.GetFullNetName()), new Color(50, 125, 255));
                NPC.active = false;
                NPC.netSkip = -1;
                NPC.life = 0;
                return false;
            }

            return true;
        }

        public static void UpdateTravelingMerchant()
        {
            bool travelerIsThere = NPC.FindFirstNPC(ModContent.NPCType<FBIGUY>()) != -1; // Find a Merchant if there's one spawned in the world

            // Main.time is set to 0 each morning, and only for one update. Sundialling will never skip past time 0 so this is the place for 'on new day' code
            if (Main.dayTime && Main.time == 0)
            {
                // insert code here to change the spawn chance based on other conditions (say, npcs which have arrived, or milestones the player has passed)
                // You can also add a day counter here to prevent the merchant from possibly spawning multiple days in a row.

                // NPC won't spawn today if it stayed all night
                if (!travelerIsThere && Main.rand.NextBool(4))
                { // 4 = 25% Chance
                  // Here we can make it so the NPC doesnt spawn at the EXACT same time every time it does spawn
                    spawnTime = GetRandomSpawnTime(5400, 8100); // minTime = 6:00am, maxTime = 7:30am
                }
                else
                {
                    spawnTime = double.MaxValue; // no spawn today
                }
            }

            // Spawn the traveler if the spawn conditions are met (time of day, no events, no sundial)
            if (!travelerIsThere && CanSpawnNow())
            {
                int newTraveler = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(), Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<FBIGUY>(), 1); // Spawning at the world spawn
                NPC traveler = Main.npc[newTraveler];
                traveler.homeless = true;
                traveler.direction = Main.spawnTileX >= WorldGen.bestX ? -1 : 1;
                traveler.netUpdate = true;

                // Prevents the traveler from spawning again the same day
                spawnTime = double.MaxValue;

                // Annouce that the traveler has spawned in!
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("Announcement.HasArrived", traveler.FullName), 50, 125, 255);
                else ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasArrived", traveler.GetFullNetName()), new Color(50, 125, 255));
            }
        }
        private static bool CanSpawnNow()
        {
            // can't spawn if any events are running
            if (Main.eclipse || Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return false;

            // can't spawn if the sundial is active
            if (Main.IsFastForwardingTime())
                return false;

            // can spawn if daytime, and between the spawn and despawn times
            return Main.dayTime && Main.time >= spawnTime && Main.time < despawnTime;
        }
        private static bool IsNpcOnscreen(Vector2 center)
        {
            int w = NPC.sWidth + NPC.safeRangeX * 2;
            int h = NPC.sHeight + NPC.safeRangeY * 2;
            Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
            foreach (Player player in Main.player)
            {
                // If any player is close enough to the traveling merchant, it will prevent the npc from despawning
                if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
            }
            return false;
        }
        public static double GetRandomSpawnTime(double minTime, double maxTime)
        {
            // A simple formula to get a random time between two chosen times
            return (maxTime - minTime) * Main.rand.NextDouble() + minTime;
        }
        */
        public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.ArmsDealer];
			NPCID.Sets.ExtraFramesCount[Type] = NPCID.Sets.ExtraFramesCount[NPCID.ArmsDealer];
            NPCID.Sets.AttackFrameCount[Type] = NPCID.Sets.AttackFrameCount[NPCID.ArmsDealer];
            NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
            NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 10;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false;
			NPCID.Sets.ShimmerTownTransform[Type] = false;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f, Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() 
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
            NPC.damage = 22;
            NPC.defense = 15;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.Passive;
			AnimationType = NPCID.ArmsDealer;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) 
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("There is a rumor that this bird is really a dragon, but rumors are just rumors.")
			});
		}
		public override List<string> SetNPCNameList() 
		{
			List<string> names = new()
			{
                "Lincoln",
                "Garfield",
                "McKinley",
                "Kennedy",
                "Mueller",
                "Scully",
                "Mulder",
                "Hoover",
                "Schrader",
                "Fitzgerald",
                "Snowden",
                "Comey",
            };
            return names;
		}
		public override string GetChat() 
		{
            WeightedRandom<string> chat = new();
            chat.Add("Listen: if anyone asks, I'm dead, you're dead, it would be impossible for us to have a conversation.");
            chat.Add("Yharim? You know, if we had our way, he'd be behind bars by now.");
            chat.Add("Do you happen to, ah... know the individual who created your machine? We've been trying to track him down.");
            chat.Add("I'm glad you aren't talking. Not only does it not alert people to my presence, it also prevents you from looking like a fool for talking to nobody.");
            chat.Add("We've gathered a lot of intel on the properties of that glowy liquid. It's more powerful than you might expect.");
            if (!stash)
            {
                chat.Add("Here's our deal: I study this machine, I give you some items to stay quiet about it, and nobody knows about anything that happened.", 2);
                chat.Add("No, I'm telling you, the machine is-uh... hey, do you want some free items?", 2);
            }
            if (Main.bloodMoon)
            {
                chat.Add("I find these nights easier to work under. Not only are they darker, they're also a refreshing break from the other kind of grotesque monsters.");
                chat.Add("We used to have most of these creatures contained. How did so many escape?");
            }
            if (Main.raining)
            {
                chat.Add("Rain. Great for cover, not great for everyday walking around.");
                chat.Add("At some point, we even tried to contain the water that falls from the sky. We may have been a bit too ambitious in those days.");
            }
            if (Main.IsItStorming)
            {
                chat.Add("You never heard this, but sometimes we use this electricity to get rid-what was that? ... I've been told I should stop talking.");
                chat.Add("This storm seems to be disrupting my connection back to our base. This is terrible, there's no way I can survive for long without constant surveillance!");
            }
            //chat.Add("Listen... how about you set up that machine again? We don't want to have to... surprise you.");
            return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2) 
		{
			if (!stash)
				button = Language.GetTextValue("Recieve Items");
		}
		public override void OnChatButtonClicked(bool firstButton, ref string shop) 
		{
			Player player = Main.LocalPlayer;
			if (firstButton)
            {
                /*
                if (DownedBossSystem.downedExoMechs)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<TwentyTwoon>());
                else if (DownedBossSystem.downedDoG)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Twentoon>());
                else if (NPC.downedGolemBoss)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Triploon>());
                else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Dualpoon>());
                else*/
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ItemID.Harpoon);
                int pick = 0;
                for (int i = 0; i < 50; i++)
                {
                    if (player.inventory[i].pick > pick)
                        pick = player.inventory[i].pick;
                }
                if (DownedBossSystem.downedPlaguebringer && !NPC.downedAncientCultist)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<EidolonTablet>());
                else if (NPC.downedGolemBoss && pick <= 210)
                {
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ItemID.Picksaw);
                }
                else if (NPC.downedMechBossAny && !NPC.downedPlantBoss)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Portabulb>());
                else if (Main.getGoodWorld)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ItemID.MechdusaSummon);
                Main.npcChatText = Main.rand.NextBool() ? "This is what I have for now. And remember: we have never interacted." : "These just magically appeared in your inventory, as I'm sure you know.";
                stash = true;
            }
        }
		public override bool CanGoToStatue(bool toKingStatue) => true;
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return false;
        }
        public override void AI()
        {
            NPC.homeless = true;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Harpoon;
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}