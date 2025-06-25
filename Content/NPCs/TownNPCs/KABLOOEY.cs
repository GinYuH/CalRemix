using System.Collections.Generic;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Placeables;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class KABLOOEY : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Archdiviner");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<SunkenSeaBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SunkenSeaBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedHydrogen;

        public override List<string> SetNPCNameList() => new List<string>() { "Ivy" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Power is both a gift and a curse. Tread carefully around me.");
            dialogue.Add("My solitude is a safeguard, for all our sakes.");
            dialogue.Add("Control is everything. Without it, we're all just ticking bombs.");

            if (!Main.dayTime)
            {
                dialogue.Add("In the stillness of night, I find a fragile peace.");
                dialogue.Add("The darkness hides my fears, but it can't extinguish them.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("Celebrations are... unsettling. But I will try to blend in, for now.");

            if (Main.bloodMoon)
            {
                dialogue.Add("The Blood Moon rises, and with it, my dread. Stay clear of me tonight.");
                dialogue.Add("This cursed night brings out the worst. Keep your distance, for your own safety.");
            }

            if (Main.LocalPlayer.ZoneBeach)
            {
                dialogue.Add("The sea's calm is deceptive. Beneath, there’s turmoil much like my own.");
                dialogue.Add("The ocean's vastness mirrors the depths of my power. Both are best left undisturbed.");
            }

            if (Main.LocalPlayer.ZoneUndergroundDesert)
            {
                dialogue.Add("The Sunken Sea’s void is a haunting reminder of what unchecked power can do.");
                dialogue.Add("A wasteland where once was life. I must never let my power cause such ruin again.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Ivy";
            }
        }

        public override void AddShops()
        {
             NPCShop npcShop = new NPCShop(Type, "Ivy");
            npcShop.AddWithCustomValue(ModContent.ItemType<EutrophicSand>(), Item.buyPrice(copper: 20));
            npcShop.AddWithCustomValue(ModContent.ItemType<Navystone>(), Item.buyPrice(copper: 20));
            npcShop.AddWithCustomValue(ModContent.ItemType<HardenedEutrophicSand>(), Item.buyPrice(copper: 20));
            npcShop.AddWithCustomValue(ModContent.ItemType<SeaPrism>(), Item.buyPrice(silver: 20));
            npcShop.AddWithCustomValue(ModContent.ItemType<PrismShard>(), Item.buyPrice(silver: 1));
            npcShop.Register();
        }


        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 9f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Bomb;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }

        public override void OnKill()
        {
            int p = Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileID.Dynamite, 999999, 22f);
            Main.projectile[p].Kill();
        }
    }
}
