using System.Collections.Generic;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon;
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
using CalRemix.Core.World;
using CalRemix.Content.NPCs.Eclipse;
using CalRemix.Core.OutboundCompatibility;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class EDGY : ModNPC
    {
        public FireParticleSet FireDrawer = null;
        public bool canAfford = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone Clone");
            Main.npcFrameCount[NPC.type] = 28;
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
                .SetNPCAffection(ModContent.NPCType<WITCH>(), AffectionLevel.Love)

                .SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Stylist, AffectionLevel.Hate);

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
            NPC.lifeMax = 66000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BrimstoneCragsBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("The conflicted remnants of an attempt to replicate a witch's power. The unholy nature of its magic drew a powerful construct to ensnare it, and the chaotic energies within managed to restore some level of function to its mind. Now confused and aimless, it quietly wanders the night, hoping to find a purpose in a world that's scorned it from the beginning.")
                //new FlavorTextBestiaryInfoElement("A very tempered and collected individual. Despite the wild nature of fire, she holds a very cold and collected attitude. Some say this is to help better control the flames. She, alongside Permafrost were at the forefront of the armada's offense against Yharim's armies. Working together, two polarizing elements combine to shatter the target, and nothing could withstand temperature shock. No matter the machine, they would eventually break through with enough time and support. It would take a devil of a machine to quell them. Maybe even multiple.")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (RemixDowned.downedPyrogen) return true; 
            return false;
        }

        public override List<string> SetNPCNameList() => new List<string>() { "Cinder" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Stay focused. Discipline is everything when wielding flames.");
            dialogue.Add("Precision and control—two things that separate a wildfire from a weapon.");
            dialogue.Add("A calm mind can withstand any inferno.");
            dialogue.Add("Though our first encounter proved unfortunate, I thank you for freeing me from that accursed construct.");
            dialogue.Add("It troubles me that I've been imprisoned twice now. You wouldn't make it three, now, would you...?");
            dialogue.Add("Peace is... strange, when you were created for war. I thank you for bringing me back to my senses.");


            if (!Main.dayTime)
            {
                dialogue.Add("The dark is quiet, but fire never rests. Neither do I.");
                dialogue.Add("Nightfall cools the world... a strange feeling for one who lives in flame.");
                dialogue.Add("The night still unsettles me. You're certain that construct is destroyed, right?");

            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("I’m more about intensity than festivity… but maybe I can relax, just this once.");

            if (Main.bloodMoon)
            {
                dialogue.Add("This unease... it’s unnatural, like a flame that refuses to go out. Best stay alert.");
                dialogue.Add("This isn’t a normal night. I’ll stay vigilant— the flame responds to the unnatural.");
            }

            if (Main.LocalPlayer.ZoneUnderworldHeight)
            {
                dialogue.Add("Hell's fires feel like home. Here, the flames breathe freely, untamed and endless.");
                dialogue.Add("I could stay in this infernal landscape forever. It’s a rare place where fire is unbound.");
                dialogue.Add("It is easy to remain on task when the landscape is as chaotic I am.");
            }
            if (Main.zenithWorld && Main.LocalPlayer.ZoneUnderworldHeight)
            {
                dialogue.Add("It unnerves me how cold the underworld is today. Is this a single odd event, or a sign of things to come?");
            }
            int witch = NPC.FindFirstNPC(ModContent.NPCType<WITCH>());
            if (witch != -1)
            {
                dialogue.Add("I may owe my existence to the witch's power, but I wish she would stop shooting me hateful glances");
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
                            if (NPC.downedMoonlord)
                                n.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
                            if (DownedBossSystem.downedDoG)
                                n.AddBuff(ModContent.BuffType<Dragonfire>(), 600);
                        }
                    }
                    SoundEngine.PlaySound(SoundID.CoinPickup);
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.FlareSound, NPC.Center);
                    canAfford = true;
                    Main.npcChatText = Burn();
                }
                else { canAfford = false; Main.npcChatText = Burn(); }
            }
        }

        public string Burn()
        {
            if (canAfford) { 
                switch (Main.rand.Next(2))
                {
                    case 0:
                        return "Though the fires of this world may seem irrational, when shown proper guidance, they can clear a path to brighter tomorrows.";
                    case 1:
                        return "There's plenty of dormant energy in the air, and I've directed it all at your adversaries. I'm certain it will only grow more powerful as you bring forth change.";
                }
            }
            return "I appreciate that you wish to burn things as much as I do, but such heavy attacks take their toll on me- I can't perform them for free.";
        }

        public override bool PreAI()
        {
            FireDrawer?.Update();
            return true;
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 25;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 2;
            randExtraCooldown = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<BrimstoneDartMinion>();
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
