using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class KoboldCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GoblinScout);
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;
            NPC.damage = 38;
            NPC.width = 32;
            NPC.height = 50;
            NPC.defense = 5;
            NPC.lifeMax = 250;
            NPC.value = Item.buyPrice(silver: 2);
            NPC.HitSound = SoundID.DD2_KoboldHurt with { Pitch = -0.2f };
            NPC.DeathSound = SoundID.DD2_KoboldDeath with { Pitch = -0.2f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            Banner = ModContent.NPCType<KoboldWarrior>();
            BannerItem = ModContent.ItemType<KoboldBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            NPC.TargetClosest();
            if (NPC.HasPlayerTarget)
                if (Collision.CanHitLine(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1) && NPC.Distance(Main.player[NPC.target].Center) < 920)
                {
                    NPC.Calamity().newAI[0]++;
                    if (NPC.Calamity().newAI[0] % 40 == 0)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemMagicMount, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 19, ModContent.ProjectileType<Kobolt>(), CalRemixHelper.ProjectileDamage(32, 64), 1f);
                        }
                    }
                }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.Y == 0)
            {
                if (NPC.velocity.X != 0)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > MathHelper.Lerp(12, 1, Utils.GetLerpValue(0, 5, Math.Abs(NPC.velocity.X), true)))
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y > frameHeight * (Main.npcFrameCount[Type] - 1))
                        NPC.frame.Y = 0;
                }
                else
                {
                    NPC.frame.X = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float pos = (spawnInfo.Player.position.X / 16);
            float fifth = Main.maxTilesX / 5f;
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !NPC.downedSlimeKing)
            {
                return 0f;
            }
            if (((pos > fifth && pos < fifth * 2) || (pos > fifth * 3 && pos < fifth * 4)) && Main.GetMoonPhase() > MoonPhase.Empty)
                return SpawnCondition.OverworldDay.Chance * 0.03f;
            return 0;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kobold1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kobold2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kobold3").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("KoboldCaster1").Type, NPC.scale);
                }
            }
        }
    }
}
