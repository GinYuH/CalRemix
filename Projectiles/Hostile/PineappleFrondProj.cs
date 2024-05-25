using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.DataStructures;
using CalRemix.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class PineappleFrondProj : ModProjectile
    {
        public override string Texture => "CalRemix/NPCs/Bosses/Phytogen/PineappleFrond";
        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 10;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frond");
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 22222;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 54;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            AIType = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Segments == null || Segments.Count < segmentCount)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(Projectile.Center + Vector2.UnitX * 5 * i);
                    Segments.Add(segment);
                }

                Segments[(int)Projectile.ai[0]].locked = true;
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                if (target.Distance(Segments[i].position) < 22)
                {
                    target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, Projectile.whoAmI), Projectile.damage, 1);
                }
            }
            return false;
        }

        public override void AI()
        {
            if (Segments is null)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; ++i)
                    Segments[i] = new VerletSimulatedSegment(Projectile.Center, false);
            }

            Segments[(int)Projectile.ai[0]].oldPosition = Segments[(int)Projectile.ai[0]].position;
            Segments[(int)Projectile.ai[0]].position = Segments[(int)Projectile.ai[0]].position + Projectile.velocity;

            Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 22, loops: segmentCount, gravity: 1f);

            Projectile.netUpdate = true;
            Projectile.netSpam = 0;

            Projectile.ai[2] += 1f;
            Projectile.localAI[0]++;
            if (Projectile.ai[2] > 5f)
            {
                Projectile.ai[2] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            if ((double)Projectile.velocity.Y < 0.25 && (double)Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X *= 0.8f;
            }
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Projectile.rotation += 22f;

            if ((Projectile.localAI[0] > 60 && Collision.IsWorldPointSolid(Projectile.Center)) || (Projectile.localAI[0] > 90 && Main.tile[(int)(Segments[(int)Projectile.ai[0]].position.X / 16), (int)(Segments[(int)Projectile.ai[0]].position.Y / 16)].WallType > 0))
            {
                Projectile.velocity = Vector2.Zero;
            }
            foreach (Player target in Main.player)
            {
                if (target == null || !target.active || target.dead)
                {
                    continue;
                }
                for (int i = 0; i < segmentCount; i++)
                {
                    if (target.Distance(Segments[i].position) < 22)
                    {
                        target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, Projectile.whoAmI), Projectile.damage, 1);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Segments == null || Segments.Count <= 0)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(Projectile.Center + Vector2.UnitX * 5 * i);
                    Segments.Add(segment);
                }

                Segments[(int)Projectile.ai[0]].locked = true;
            }
            for (int i = 0; i < Segments.Count; i++)
            {
                VerletSimulatedSegment seg = Segments[i];
                float rot = 0f;
                if (i > 0)
                    rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                else
                    rot = seg.position.DirectionTo(Segments[i + 1].position).ToRotation() - MathHelper.PiOver2;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, seg.position - Main.screenPosition, null, Projectile.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, TextureAssets.Projectile[Type].Value.Size() / 2, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int goreChance = 2;
            foreach (var seg in Segments)
            {
                for (int i = 0; i < goreChance; i++)
                { 
                    Gore.NewGore(Projectile.GetSource_Death(), seg.position, Vector2.Zero, Mod.Find<ModGore>("Frond" + Main.rand.Next(1, 5)).Type, Projectile.scale);
                }
            }
        }
    }
}