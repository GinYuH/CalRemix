using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using System;
using Terraria.Audio;
using CalamityMod.Dusts;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.NPCs.SupremeCalamitas;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class CuboidCurse : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cuboid Curse");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 3200;
            NPC.damage = 90;
            NPC.defense = 10;
            NPC.knockBackResist = 0.3f;
            NPC.value = Item.buyPrice(gold: 15);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            bool p2 = NPC.life < NPC.lifeMax * 0.5f;
            NPC.TargetClosest();
            NPC.velocity.X *= 0.93f;
            if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
            {
                NPC.velocity.X = 0f;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 300f;
            }
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
            {
                NPC.position += NPC.netOffset;
                SoundEngine.PlaySound(in SoundID.Item8, NPC.position);
                for (int i = 0; i < 50; i++)
                {                    
                    int brimstoneDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1.5f);
                    Dust brimstoneDustfr = Main.dust[brimstoneDust];
                    Dust brimstoneDustgoddammit = brimstoneDustfr;
                    brimstoneDustgoddammit.velocity *= 3f;
                    Main.dust[brimstoneDust].noGravity = true;
                }
                NPC.position -= NPC.netOffset;
                NPC.position.X = NPC.ai[2] * 16f - (NPC.width / 2) + 8f;
                NPC.position.Y = NPC.ai[3] * 16f - NPC.height;
                NPC.netOffset *= 0f;
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
                SoundEngine.PlaySound(SupremeCalamitas.BrimstoneShotSound, NPC.position);
                for (int i = 0; i < 50; i++)
                {                    
                    int brimstoneDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1.5f);
                    Dust brimstoneDustfr = Main.dust[brimstoneDust];
                    Dust brimstoneDustgoddammit = brimstoneDustfr;
                    brimstoneDustgoddammit.velocity *= 3f;
                    Main.dust[brimstoneDust].noGravity = true;                    
                }
                if (p2)
                {
                    SoundEngine.PlaySound(SupremeCalamitas.BrimstoneBigShotSound, NPC.position);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = -2; i < 3; i++)
                        {
                            if (i == 0)
                                continue;
                            float spacing = 16 * 6;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top + Vector2.UnitX * i * spacing, Vector2.Zero, ModContent.ProjectileType<RedstonePillar>(), NPC.damage, 0f, Main.myPlayer);
                        }
                    }
                }
            }
            NPC.ai[0] += 1f;
            float modulo = p2 ? 30 : 50;
            if (NPC.ai[0] % modulo == 0)
            {
                NPC.ai[1] = 30f;
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] >= 200f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = 1f;
                
                int targetTileX = (int)Main.player[NPC.target].Center.X / 16;
                int targetTileY = (int)Main.player[NPC.target].Center.Y / 16;
                Vector2 chosenTile = Vector2.Zero;
                if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
                {
                    NPC.ai[1] = 20f;
                    NPC.ai[2] = chosenTile.X;
                    NPC.ai[3] = chosenTile.Y;
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1] -= 1f;
                if (NPC.ai[1] == 25f)
                {                    
                    SoundEngine.PlaySound(SupremeCalamitas.HellblastSound, NPC.position);                        
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int firebol = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 10, ModContent.ProjectileType<RedstoneFireball>(), NPC.damage / 2, 0f, Main.myPlayer);
                        Main.projectile[firebol].timeLeft = 300;
                        NPC.localAI[0] = 0f;
                    }                    
                }
            }
            NPC.direction = Math.Sign(NPC.DirectionTo(Main.player[NPC.target].Center).X);
            NPC.spriteDirection = -NPC.direction;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG)
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.4f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 1, 3);
        }
    }
}