using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.TileEntities;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
	public class FBIGUY : ModNPC
	{
		private bool stash = false;
        public const double despawnTime = 23400.0;
        public static double spawnTime = double.MaxValue;
        public override bool CanChat() => true;
        public override bool NeedSaving() => true;
        public override bool PreAI()
        {
            if ((Main.dayTime || Main.time >= despawnTime) && !OnScreen(NPC.Center))
            {
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("LegacyMisc.35", NPC.FullName), 50, 125, 255);
                else ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.35", NPC.GetFullNetName()), new Color(50, 125, 255));
                NPC.active = false;
                NPC.netSkip = -1;
                NPC.life = 0;
                return false;
            }
            return true;
        }
        public static void UpdateAgent()
        {
            bool agentExists = NPC.FindFirstNPC(ModContent.NPCType<FBIGUY>()) != -1;
            if (!Main.dayTime && Main.time == 0)
            {
                if (!agentExists)
                    spawnTime = 1800;
                else
                    spawnTime = double.MaxValue;
            }
            if (!agentExists && CanSpawnNow() && (Main.rand.NextBool(20) || (Main.bloodMoon && Main.rand.NextBool(10))))
            {
                Point point = FindCodebreaker();
                if (point == new Point(-1, -1))
                    return;
                Vector2 pos = point.ToWorldCoordinates();
                int newAgentIndex = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(), (int)pos.X, (int)pos.Y, ModContent.NPCType<FBIGUY>(), 1);
                NPC agent = Main.npc[newAgentIndex];
                agent.homeless = true;
                agent.direction = (int)point.X >= WorldGen.bestX ? -1 : 1;
                agent.netUpdate = true;
                spawnTime = double.MaxValue;
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(Language.GetTextValue("Announcement.HasArrived", agent.FullName), 50, 125, 255);
                else
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasArrived", agent.GetFullNetName()), new Color(50, 125, 255));
            }
        }
        private static bool CanSpawnNow()
        {
            if (Main.eclipse || Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return false;
            if (Main.IsFastForwardingTime())
                return false;
            return !Main.dayTime && Main.time == spawnTime && Main.time < despawnTime;
        }
        private static Point FindCodebreaker()
        {
            if (!TileEntity.ByID.Any())
                return new Point(-1, -1);
            for (int i = 0; i <= TileEntity.ByID.Keys.Max(); i++)
            {
                if (!TileEntity.ByID.ContainsKey(i))
                    continue;
                if (TileEntity.ByID[i] == null) 
                    continue;
                if (TileEntity.ByID[i].type == ModContent.TileEntityType<TECodebreaker>())
                {
                    TECodebreaker cb = TileEntity.ByID[i] as TECodebreaker;
                    if (cb.ContainsSensorArray)
                        return TileEntity.ByID[i].Position.ToPoint();
                }
            }
            return new Point(-1, -1);
        }
        private static bool OnScreen(Vector2 center)
        {
            int w = NPC.sWidth + NPC.safeRangeX * 2;
            int h = NPC.sHeight + NPC.safeRangeY * 2;
            Rectangle npcScreenRect = new((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
            foreach (Player player in Main.player)
                if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
            return false;
        }
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
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.ShimmerTownTransform[NPC.type] = false;
			NPCID.Sets.ShimmerTownTransform[Type] = false;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f, Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() 
		{
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
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
            if (FindCodebreaker() == new Point(-1, -1))
                chat.Add("Listen... how about you set up that machine again? We don't want to have to... surprise you.", 5);
            return chat;
		}
		public override void SetChatButtons(ref string button, ref string button2) 
		{
			if (!stash)
				button = Language.GetTextValue("Recieve Items");
            Main.LocalPlayer.currentShoppingSettings.HappinessReport = "";
		}
		public override void OnChatButtonClicked(bool firstButton, ref string shop) 
		{
			Player player = Main.LocalPlayer;
			if (firstButton)
            {
                if (DownedBossSystem.downedExoMechs)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<TwentyTwoon>());
                else if (DownedBossSystem.downedDoG)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Twentoon>());
                else if (NPC.downedGolemBoss)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Triploon>());
                else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    player.QuickSpawnItem(Terraria.Entity.GetSource_None(), ModContent.ItemType<Dualpoon>());
                else
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
		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return false;
        }
        public override void AI()
        {
            NPC.homeless = true;
            if (!OnScreen(NPC.Center) && !Main.dayTime)
            {
                Point p = FindCodebreaker();
                if (p == new Point(-1, -1) && NPC.Center.Distance(p.ToWorldCoordinates()) > 320)
                    NPC.Center = p.ToWorldCoordinates();
            }
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 20;
            randExtraCooldown = 10;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<AgentPoon>();
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 44f;
            randomOffset = 0.05f;
        }
        public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
        {
            if (!NPC.downedMoonlord)
            {
                Main.GetItemDrawFrame(ItemID.Harpoon, out item, out itemFrame);
                horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1, ItemID.Harpoon).X - 64;
            }
            else
            {
                Main.GetItemDrawFrame(ModContent.ItemType<Triploon>(), out item, out itemFrame);
                horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1, ModContent.ItemType<Triploon>()).X - 64;
            }
        }
    }
    public class FBIGUYSystem : ModSystem
    {
        public override void PreUpdateWorld()
        {
            FBIGUY.UpdateAgent();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (FBIGUY.spawnTime != double.MaxValue)
                tag["FBIGUYSpawnTime"] = FBIGUY.spawnTime;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("FBIGUYSpawnTime", out double spawnTime))
                FBIGUY.spawnTime = spawnTime;
            else
                FBIGUY.spawnTime = double.MaxValue;
        }
    }
}