using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Events;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Phytogen
{
    public class PineappleFrond : ModNPC
    {
        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 30;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frond");
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 48;
            NPC.height = 54;
            NPC.defense = 20;
            NPC.LifeMaxNERB(4000, 5000, 70000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.netAlways = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            foreach (var v in Segments)
            {
                writer.Write(v.position.X);
                writer.Write(v.position.Y);
                writer.Write(v.locked);
                writer.Write(v.oldPosition.X);
                writer.Write(v.oldPosition.Y);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            List<VerletSimulatedSegment> segs = new List<VerletSimulatedSegment>();
            for (int i = 0; i < 30; i++)
            {
                VerletSimulatedSegment v = new VerletSimulatedSegment(new Vector2(reader.ReadSingle(), reader.ReadSingle()), reader.ReadBoolean());
                v.oldPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                segs.Add(v);
            }
            Segments = segs;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Segments == null || Segments.Count < segmentCount)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[^1].locked = true;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (Segments != null)
            for (int i = 0; i < segmentCount; i++)
            {
                if (target.getRect().Intersects(new Rectangle((int)Segments[i].position.X, (int)Segments[i].position.Y, 10, 10)) && Main.npc[(int)NPC.ai[0]].ai[0] > 0)
                {
                    target.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, 1);
                }
            }
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            NPC phyto = Main.npc[(int)NPC.ai[0]];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Phytogen>())
            {
                return false;
            }
            if (projectile.type == ModContent.ProjectileType<Potpourri>() && phyto.ai[0] == (int)Phytogen.PhaseType.Passive)
            {
                return false;
            }
            return null;
        }

        /*public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                if (projectile.getRect().Intersects(new Rectangle((int)Segments[i].position.X, (int)Segments[i].position.Y, 22, 22)) && NPC.immuneTime >= ContentSamples.ProjectilesByType[projectile.type].localNPCHitCooldown)
                {
                    NPC.HitInfo info = NPC.CalculateHitInfo(projectile.damage, projectile.direction, false, projectile.knockBack, projectile.DamageType, true);
                    NPC.StrikeNPC(info);
                    NPC.immuneTime = 0;
                    projectile.ModProjectile.OnHitNPC(NPC, info, projectile.damage);
                    projectile.penetrate--;
                }
            }
            return null;
        }*/

        public override void AI()
        {
            NPC.immuneTime++;
            NPC phyto = Main.npc[(int)NPC.ai[0]];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Phytogen>())
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                return;
            }
            //if (Main.netMode != NetmodeID.Server)
            {
                if (Segments is null)
                {
                    Segments = new List<VerletSimulatedSegment>(segmentCount);
                    for (int i = 0; i < segmentCount; ++i)
                        Segments[i] = new VerletSimulatedSegment(NPC.Center, false);
                }

                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = NPC.Center;

                Segments[Segments.Count - 1].oldPosition = Segments[Segments.Count - 1].position;
                Segments[Segments.Count - 1].position = phyto.Center;

                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 2, loops: segmentCount, gravity: 22f);
            }

            NPC.netUpdate = true;
            NPC.netSpam = 0;

            NPC.rotation = NPC.DirectionTo(phyto.Center).ToRotation() - MathHelper.PiOver2;

            int maxDist = 700;
            Rectangle safeZone = new Rectangle();
            switch (NPC.ai[1])
            {
                case 0: // Top right
                    safeZone = new Rectangle((int)phyto.Center.X, (int)phyto.Center.Y - maxDist, maxDist, maxDist);
                    break;
                case 2: // Top left
                    safeZone = new Rectangle((int)phyto.Center.X - maxDist, (int)phyto.Center.Y - maxDist, maxDist, maxDist);
                    break;
                case 1: // Bottom right
                    safeZone = new Rectangle((int)phyto.Center.X, (int)phyto.Center.Y, maxDist, maxDist);
                    break;
                case 3: // Bottom left
                    safeZone = new Rectangle((int)phyto.Center.X - maxDist, (int)phyto.Center.Y, maxDist, maxDist);
                    break;
            }
            NPC.ai[2]++;
            if (NPC.ai[2] >= NPC.ai[3] && NPC.velocity == Vector2.Zero)
            {
                NPC.velocity = NPC.DirectionTo(new Vector2(Main.rand.Next(safeZone.X, safeZone.X + safeZone.Width), Main.rand.Next(safeZone.Y, safeZone.Y + safeZone.Height))) * 10;
                NPC.ai[2] = 0;
                NPC.ai[3] = Main.rand.Next(120, 240);
            }

            Tile t = CalamityUtils.ParanoidTileRetrieval((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16));
            if (Collision.IsWorldPointSolid(NPC.Center) || t.WallType > 0 || BossRushEvent.BossRushActive || NPC.ai[2] > 300)
            {
                if (NPC.ai[2] > 60)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
                else if (Collision.IsWorldPointSolid(NPC.Center))
                {
                    NPC.velocity = NPC.DirectionTo(phyto.Center) * 10;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.Distance(NPC.Center) > maxDist)
            {
                NPC.velocity = NPC.DirectionTo(safeZone.Center.ToVector2()) * 10;
                NPC.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC phyto = Main.npc[(int)NPC.ai[0]];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Phytogen>())
            {
                return false;
            }
            if (Segments == null || Segments.Count <= 0)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[Segments.Count - 1].locked = true;
            }
            for (int i = 0; i < Segments.Count; i++)
            {
                VerletSimulatedSegment seg = Segments[i];
                float rot = 0f;
                if (i > 0)
                    rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                else
                    rot = NPC.rotation;
                Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, seg.position - Main.screenPosition, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, TextureAssets.Npc[Type].Value.Size() / 2, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            int goreChance = 2;
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    foreach (var seg in Segments)
                    {
                        for (int i = 0; i < goreChance; i++)
                        {
                            Gore.NewGore(NPC.GetSource_Death(), seg.position, Vector2.Zero, Mod.Find<ModGore>("Frond" + Main.rand.Next(1, 5)).Type, NPC.scale);
                        }
                    }
                }
            }
        }

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Phytogen>());
        }
    }
}