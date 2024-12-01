using CalamityMod;
using CalRemix.Content.NPCs.Bosses.BossScule;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
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
                // Harmlessly grab the player and attempt to pull them in. this is resistable
                if (AttackType == 0)
                {
                    Projectile.localAI[0]++;
                    // Go back to Pyrogen
                    if (AttackTime > MaxAttackTime)
                    {
                        Projectile.position = Vector2.Lerp(p.Center, n.Center, Utils.GetLerpValue(MaxAttackTime, MaxAttackTime + hitPlayerTime, AttackTime, true));
                        if (Projectile.Hitbox.Intersects(n.Hitbox))
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    // Launch towards the player then glue to their position
                    else
                    {
                        Projectile.position = Vector2.Lerp(n.Center, p.Center, Utils.GetLerpValue(0, hitPlayerTime, AttackTime, true));
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

                }
            }
            Projectile.rotation = n.SafeDirectionTo(p.Center).ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D reelTexture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PyrogenHarpoonHit").Value;
            Texture2D endTexture = AttackTime > 60 ? reelTexture : TextureAssets.Projectile[Projectile.type].Value;
            Texture2D chainTexture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PyrogenHarpoonChain").Value;
            NPC owner = Main.npc[(int)NPCIndex];
            Vector2 distToProj = Projectile.Center;
            float projRotation = Projectile.AngleTo(owner.Center) - 1.57f;
            bool doIDraw = true;

            while (doIDraw)
            {
                float distance = (owner.Center - distToProj).Length();
                if (distance < (chainTexture.Height + 1))
                {
                    doIDraw = false;
                }
                else if (!float.IsNaN(distance))
                {
                    Color drawColor = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                    distToProj += Projectile.SafeDirectionTo(owner.Center) * chainTexture.Height;
                    Main.EntitySpriteDraw(chainTexture, distToProj - Main.screenPosition,
                        new Rectangle(0, 0, chainTexture.Width, chainTexture.Height), drawColor, projRotation,
                        Utils.Size(chainTexture) / 2f, 1f, SpriteEffects.None, 0);
                }
            }
            return true;
        }
    }
}
