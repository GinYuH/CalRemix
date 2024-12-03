using CalamityMod;
using CalamityMod.DataStructures;
using CalRemix.Content.NPCs.Bosses.BossScule;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenHarpoon : ModProjectile
    {
        public ref float NPCIndex => ref Projectile.ai[0];

        public ref float PlayerIndex => ref Projectile.ai[1];

        public ref float AttackType => ref Projectile.ai[2];

        public ref float AttackTime => ref Projectile.localAI[0];

        public ref float MaxAttackTime => ref Projectile.localAI[1];

        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 40;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 22222;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
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
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
            List<VerletSimulatedSegment> segs = new List<VerletSimulatedSegment>();
            for (int i = 0; i < 10; i++)
            {
                VerletSimulatedSegment v = new VerletSimulatedSegment(new Vector2(reader.ReadSingle(), reader.ReadSingle()), reader.ReadBoolean());
                v.oldPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                segs.Add(v);
            }
            Segments = segs;
        }


        public override void AI()
        {
            Projectile.timeLeft = 22;
            NPC n = Main.npc[(int)NPCIndex];
            Player p = Main.player[(int)PlayerIndex];
            if (n == null || !n.active || n.type != ModContent.NPCType<Pyrogen>() || n.life <= 0)
            {
                Projectile.Kill();
                return;
            }

            if (Segments == null)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(Projectile.Center);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[Segments.Count - 1].locked = true;
            }
            else
            {
                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = Projectile.Center;

                Segments[^1].oldPosition = Segments[^1].position;
                Segments[^1].position = n.Center;

                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, MathHelper.Lerp(8, 16, n.Distance(Projectile.Center) / 1200f), loops: segmentCount, gravity: 0.2f);

                Projectile.netUpdate = true;
                Projectile.netSpam = 0;
            }


            bool validPlayer = true;
            if (p == null || !p.active || p.dead)
            {
                Projectile.velocity = Projectile.SafeDirectionTo(n.Center) * 10;
                if (Projectile.Hitbox.Intersects(n.Hitbox))
                {
                    Projectile.Kill();
                    return;
                }
            }
            int hitPlayerTime = 30; // how long it takes to hit the player
            int maxPullSpeed = 6; // the max pull force the player has to endure, works together with maxDistOfScale
            int minDist = 400; // when to stop pulling
            int maxDistOfScale = 4000; // how far it takes for it to reach the max pull force
            if (validPlayer)
            {
                Vector2 playerOff = p.DirectionTo(n.Center) * (AttackTime > 60 ? 80 : 120);
                // Harmlessly grab the player and attempt to pull them in. this is resistable
                if (AttackType == 0)
                {
                    Projectile.localAI[0]++;
                    // Go back to Pyrogen
                    if (AttackTime > MaxAttackTime)
                    {
                        Projectile.position = Vector2.Lerp(p.Center + playerOff, n.Center, Utils.GetLerpValue(MaxAttackTime, MaxAttackTime + hitPlayerTime, AttackTime, true));
                        if (Projectile.Hitbox.Intersects(n.Hitbox))
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    // Launch towards the player then glue to their position
                    else
                    {
                        Projectile.position = Vector2.Lerp(n.Center, p.Center + playerOff, Utils.GetLerpValue(0, hitPlayerTime, AttackTime, true));
                    }
                    if (Projectile.localAI[0] == hitPlayerTime)
                    {
                        Projectile.netUpdate = true;
                    }
                    // Drag the player inwards
                    else if (AttackTime > hitPlayerTime)
                    {
                        p.velocity = Vector2.Lerp(p.velocity, p.velocity + p.SafeDirectionTo(n.Center) * maxPullSpeed, Utils.GetLerpValue(minDist, maxDistOfScale, p.Distance(n.Center), true));
                    }
                }
                else if (AttackType == 1)
                {
                    Projectile.localAI[0]++;
                    // Go back to Pyrogen
                    if (AttackTime > MaxAttackTime)
                    {
                        Projectile.velocity = Projectile.DirectionTo(n.Center) * MathHelper.Lerp(12, 24, Utils.GetLerpValue(0, 1000, Projectile.Distance(n.Center), true));
                        if (Projectile.Hitbox.Intersects(n.Hitbox))
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    // Launch towards the player then glue to their position
                    else
                    {
                        Projectile.position = Vector2.Lerp(n.Center, p.Center + playerOff, Utils.GetLerpValue(0, MaxAttackTime, AttackTime, true));
                    }
                    if (Projectile.localAI[0] == MaxAttackTime)
                    {
                        Projectile.netUpdate = true;
                    }
                    // Drag the player inwards
                    else if (AttackTime > MaxAttackTime && Projectile.Distance(n.Center) > 200)
                    {
                        p.position = Projectile.position - playerOff;
                        p.velocity = Projectile.velocity;
                    }
                }
                else if (AttackType == 2)
                {
                    Projectile.localAI[0]++;
                    float comp = Utils.GetLerpValue(0, hitPlayerTime, AttackTime, true);
                    int dist = (int)Projectile.localAI[2];
                    hitPlayerTime = 60;
                    if (AttackTime <= hitPlayerTime)
                    {
                        Projectile.position = Vector2.Lerp(n.Center, n.Center + n.Center.DirectionTo(p.Center) * dist, comp);
                        if (AttackTime % 5 == 0)
                        {
                            int projSpeed = 16;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi) * projSpeed, ModContent.ProjectileType<PyrogenFlare>(), (int)(n.damage / 6f), 0, Main.myPlayer, 0, 1);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2().RotatedBy(-MathHelper.Pi) * -projSpeed, ModContent.ProjectileType<PyrogenFlare>(), (int)(n.damage / 6f), 0, Main.myPlayer, 0, 1);
                        }
                    }
                    else
                    {
                        comp = Utils.GetLerpValue(hitPlayerTime, hitPlayerTime + 20, AttackTime, true);
                        Projectile.position = Vector2.Lerp(n.Center + n.Center.DirectionTo(p.Center) * dist, n.Center, comp);
                        if (Projectile.Hitbox.Intersects(n.Hitbox))
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    if (p.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        MaxAttackTime = -22;
                    }
                    if (MaxAttackTime == -22)
                    {
                        p.position = Projectile.position - playerOff;
                        p.velocity = Projectile.velocity;
                    }
                }
            }
            Projectile.rotation = n.SafeDirectionTo(p.Center).ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D reelTexture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PyrogenHarpoonHit").Value;
            Texture2D endTexture = AttackTime > 60 ? reelTexture : TextureAssets.Projectile[Projectile.type].Value;
            Texture2D chainTexture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PyrogenHarpoonChain").Value;
            
            NPC phyto = Main.npc[(int)NPCIndex];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Pyrogen>())
            {
                return false;
            }
            if (Segments != null)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    VerletSimulatedSegment seg = Segments[i];
                    float rot = 0f;
                    if (i > 0)
                        rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                    else
                        rot = Projectile.rotation;
                    if (i > 0)
                        Main.EntitySpriteDraw(chainTexture, seg.position - Main.screenPosition, null, Projectile.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, chainTexture.Size() / 2, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    else
                        Main.EntitySpriteDraw(endTexture, seg.position - Main.screenPosition, null, lightColor, rot, new Vector2(endTexture.Width / 2, endTexture.Height), Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(Projectile.whoAmI);
        }
    }
}
