using System;
using System.Collections.Generic;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.UI.CalamitasEnchants;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class EDGY : ModNPC
    {
        public FireParticleSet FireDrawer = null;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archseer");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<BrimstoneCragsBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(ModContent.NPCType<DILF>(), AffectionLevel.Like)
                .SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike);
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
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
                new FlavorTextBestiaryInfoElement("A very tempered and collected individual. Despite the wild nature of fire, she holds a very cold and collected attitude. Some say this is to help better control the flames. She, alongside Permafrost were at the forefront of the armada's offense against Yharim's armies. Working together, two polarizing elements combine to shatter the target, and nothing could withstand temperature shock. No matter the machine, they would eventually break through with enough time and support. It would take a devil of a machine to quell them. Maybe even multiple.")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedPyrogen;

        public override List<string> SetNPCNameList() => new List<string>() { "Cinder" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Stay focused. Discipline is everything when wielding flames.");
            dialogue.Add("Precision and control—two things that separate a wildfire from a weapon.");
            dialogue.Add("A calm mind can withstand any inferno.");

            if (!Main.dayTime)
            {
                dialogue.Add("The dark is quiet, but fire never rests. Neither do I.");
                dialogue.Add("Nightfall cools the world... a strange feeling for one who lives in flame.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("I’m more about intensity than festivity… but maybe I can relax, just this once.");

            if (Main.bloodMoon)
            {
                dialogue.Add("This unease... it’s unnatural, like a flame that refuses to go out. Best stay alert.");
                dialogue.Add("This isn’t a normal night. I’ll stay vigilant—the flame responds to the unnatural.");
            }

            if (Main.LocalPlayer.ZoneUnderworldHeight)
            {
                dialogue.Add("Hell's fires feel like home. Here, the flames breathe freely, untamed and endless.");
                dialogue.Add("I could stay in this infernal landscape forever. It’s a rare place where fire is unbound.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Burn Enemies (10 Gold)";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (Main.LocalPlayer.BuyItem(Item.buyPrice(gold: 1)))
                {
                    foreach (NPC n in Main.ActiveNPCs)
                    {
                        if (!n.friendly)
                        {
                            n.AddBuff(BuffID.OnFire3, 600);
                            n.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 600);
                            n.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
                        }
                    }
                    SoundEngine.PlaySound(SoundID.CoinPickup);
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.FlareSound, NPC.Center);
                }
            }
        }

        public override bool PreAI()
        {
            FireDrawer?.Update();
            return true;
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 10;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.InfernoFriendlyBolt;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (FireDrawer is null || FireDrawer.LocalTimer >= FireDrawer.SetLifetime)
                FireDrawer = new FireParticleSet(int.MaxValue, 1, Color.Red * 1.25f, Color.Red, 22, 1);
            else
                FireDrawer.DrawSet(NPC.Bottom - Vector2.UnitY * (12f - NPC.gfxOffY));
            return true;
        }
    }
}
