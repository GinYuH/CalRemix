using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Rosalina : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 60;
            NPC.lifeMax = 200000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;

            AnimationType = NPCID.Guide;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Seemingly born from both profaned and cosmic energies, the Celestial has inherited the powers of both. Somehow, she knows how to conjure divine artifacts from nothing.")
            });
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Deimos" };
        }
        public override bool CanTownNPCSpawn(int numNPCs, int money)
        {
            return CalamityMod.DownedBossSystem.downedProvidence;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 140;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<CalamityMod.Projectiles.Summon.ProfanedCrystalMeleeSpear>();
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return !toKingStatue;
        }
        public override string GetChat()
        {
            List<string> chat = new List<string>
            {
                "You wonder if I will try to kill you in the future? Sounds like too much effort.",
                "Can you go away? I was having a really good daydream before you came.",
                "I wish I didn't have to eat. Takes too long to get the food.",
            };
            if(NPC.homeless)
            {
                chat.Add("I do like being inside.");
                chat.Add("Can you hurry up and make me some housing? Preferably with a cozy bed.");
            }
            if (Main.LocalPlayer.ZoneHallow)
            {
                chat.Add("It's amusing that a place of such apparent goodness can be so violent.");
                chat.Add("You would think that a biome full of fairies and unicorns would be a little kinder.");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp) chat.Add("Woo, I sure love parties.");
            if(Main.bloodMoon)
            {
                chat.Add("There's too many pests around. I would appreciate it if you kept them out");
                chat.Add("Go. Those pests aren't going to kill themselves.");
            }
            if (NPC.AnyNPCs(NPCID.Angler)) chat.Add($"I must say that everybody here has good taste in fashion. Except {Main.npc[NPC.FindFirstNPC(NPCID.Angler)].GivenName}. He needs a new hat.");
            if(CalamityMod.DownedBossSystem.downedDoG)
            {
                chat.Add("Though my father's strength was great, his hubris led him to underestimate you.");
                chat.Add("Considering how weak you originally were, I do wonder if your growth will ever reach a limit.");
            }
            return Main.rand.Next(chat);
        }
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Placeables.Furniture.CraftingStations.ProfanedCrucible>(), Item.buyPrice(0, 60), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.ProfanedPartisan>(), Item.buyPrice(3), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Magic.DivineRetribution>(), Item.buyPrice(3), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Melee.SeekingScorcher>(), Item.buyPrice(2), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.ShatteredSun>(), Item.buyPrice(2), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.DivineGeode>(), Item.buyPrice(0, 6), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.NightmareFuel>(), Item.buyPrice(0, 12), ref shop, ref nextSlot, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.EndothermicEnergy>(), Item.buyPrice(0, 12), ref shop, ref nextSlot, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.DarksunFragment>(), Item.buyPrice(0, 12), ref shop, ref nextSlot, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Accessories.ProfanedSoulCrystal>(), 10, ref shop, ref nextSlot, CalamityMod.DownedBossSystem.downedExoMechs && CalamityMod.DownedBossSystem.downedSCal, 1);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Ammo.HolyFireBullet>(), Item.buyPrice(0, 0, 20), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Ammo.ElysianArrow>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.SummonItems.RuneofKos>(), Item.buyPrice(2), ref shop, ref nextSlot);
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }
    }
}
