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
using CalRemix.UI.TransmogrifyUI;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Rosalina : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starborn Princess");
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
                new FlavorTextBestiaryInfoElement("The curious daughter of the cosmic serpent and the profaned goddess. Inheriting the powers of both, she has the ability to alter matter however she sees fit.")
            });
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Deimos" };
        }
        public override bool CanTownNPCSpawn(int useless)
        {
            //return CalamityMod.DownedBossSystem.downedProvidence;
            return false;
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
                "Did you hear about that one artist? I think they are called something like Everquartz... well- I think they're weird personally.",
                "I really like reading books and I like studying on how to warp myself to different timelines. If I didn't learn how to, I probably wouldn't even be here!",
                "I hate both of my parents to be honest, they're both as bad as each other... sort of. I can't remember.",
                "So, what's your opinion on chicken nuggets?",
                "*starts doing the cha cha slide* Cha cha real smooth!"
            };
            if(NPC.homeless)
            {
                chat.Add("I could've sworn I had a house... oh well.");
                chat.Add("Did you leave me homeless? That's a shame. To be honest, living in a house is overrated anyways!");
            }
            if (Main.LocalPlayer.ZoneHallow)
            {
                chat.Add("I think the Hallow is quite a deceiving place. It's quite unique, actually. I really do like it, but man those unicorns are lethal. Have you not seen them?!");
                chat.Add("Man, you would think that a biome full of fairies and unicorns would be a little kinder sometimes...");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp) chat.Add("Wow! Parties are so much fun. I love jumping around and dancing and all that confetti is so fun! It's so bright and colorful! I love it!");
            if(Main.bloodMoon)
            {
                chat.Add("Why are you here?! Go on, get out of my house before you feel the wrath of my anger!");
                chat.Add("You are lucky that I do not have contact with my parents. Shoo! Get out of here!");
            }
            if (NPC.AnyNPCs(NPCID.Angler)) chat.Add($"Did you hear about {Main.npc[NPC.FindFirstNPC(NPCID.Angler)].GivenName}? Yeah, that short kid with the weird hat and stuff? Well, I don't like him.");
            if(CalamityMod.DownedBossSystem.downedDoG)
            {
                chat.Add("I am sort of sad that my father is dead... I mean, I get he was a bad person but I just never got to spend time with him that much. I think it is for the better anyways, he didn't seem like the nicest guy around...");
                chat.Add("Considering how weak you originally were, I do wonder if your growth will ever reach a limit. I'm curious!");
            }
            return Main.rand.Next(chat);
        }
        public override void AddShops()
        {
            NPCShop shop = new NPCShop(Type, "Shop");
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Placeables.Furniture.CraftingStations.ProfanedCrucible>(), Item.buyPrice(0, 60), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.ProfanedPartisan>(), Item.buyPrice(3), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Magic.DivineRetribution>(), Item.buyPrice(3), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Melee.SeekingScorcher>(), Item.buyPrice(2), ref shop); 
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.ShatteredSun>(), Item.buyPrice(2), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.DivineGeode>(), Item.buyPrice(0, 6), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.NightmareFuel>(), Item.buyPrice(0, 12), ref shop, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.EndothermicEnergy>(), Item.buyPrice(0, 12), ref shop, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Materials.DarksunFragment>(), Item.buyPrice(0, 12), ref shop, CalamityMod.DownedBossSystem.downedDoG);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Accessories.ProfanedSoulCrystal>(), 10, ref shop, CalamityMod.DownedBossSystem.downedExoMechs && CalamityMod.DownedBossSystem.downedCalamitas, 1);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Ammo.HolyFireBullet>(), Item.buyPrice(0, 0, 20), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Ammo.ElysianArrow>(), Item.buyPrice(0, 20), ref shop);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.SummonItems.RuneofKos>(), Item.buyPrice(2), ref shop);
            shop.Register();
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.CalRemix.UI.TransmogrifyMenu");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = NPCShopDatabase.GetShopName(Type);
            } else
            {
                Main.npcChatText = "Ah, sorry... I forgot to tell you that this isn't implemented yet. They say they \"need to figure out UI\" first...";
            }
        }
    }
}
