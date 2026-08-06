using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class WisdomTeeth : ModProjectile
    {
        public ref float timer => ref Projectile.ai[0];

        public Vector2 Anchor
        {
            get { return new Vector2(Projectile.ai[1], Projectile.ai[2]); }
            set {
                Projectile.ai[1] = value.X; Projectile.ai[2] = value.Y;
            }
        }
        
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 14;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (timer == 0 && Projectile.owner == Main.myPlayer)
            {
                Anchor = Main.MouseWorld;
            }
            else
            {
                int travelTime = 30;
                int existTime = 180;
                bool top = Projectile.frame > 6;
                Vector2 idealPosition = Anchor + Vector2.UnitX * (Projectile.frame % 7 - 4) * 30 + Vector2.UnitY * top.ToDirectionInt() * 20 + Vector2.UnitY * MathHelper.Lerp(0, 10 * top.ToDirectionInt(), Utils.GetLerpValue(0, 3, Projectile.frame % 7, true));
                if (timer < travelTime)
                {
                    Vector2 topos = idealPosition;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, topos, 0.4f);
                }
                else
                {
                    int range = 100;
                    idealPosition.Y += top.ToDirectionInt() * range * MathF.Sin(Main.GameUpdateCount * 0.2f) + top.ToDirectionInt() * (range / 2 + 20);
                    Projectile.Center = Vector2.Lerp(Projectile.Center, idealPosition, 0.6f);
                    
                        Dust.NewDust(Projectile.Center, 2, 2,DustID.Blood);
                    
                }
            }
            timer++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike)
                target.AddBuff(ModContent.BuffType<HeavyBleeding>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.Calamity().stealthStrike)
                target.AddBuff(ModContent.BuffType<HeavyBleeding>(), 120);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
            }

        }
    }
}