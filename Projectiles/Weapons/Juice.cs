using CalamityMod.Buffs.DamageOverTime;
using CalRemix.UI.ElementalSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
    public class Juice : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            int dustType = DustID.Ichor;
            switch (Projectile.ai[2])
            {
                case 0:
                    dustType = DustID.Blood;
                    break;
                case 1:
                    dustType = DustID.InfernoFork;
                    break;
                case 2:
                    dustType = DustID.Ichor;
                    break;
                case 3:
                    dustType = DustID.Blood;
                    break;
                case 4:
                    dustType = DustID.DungeonWater;
                    break;
                case 5:
                    dustType = DustID.Venom;
                    break;
                case 6:
                    dustType = DustID.HallowedWeapons;
                    break;
            }
            Projectile.scale -= 0.002f;
            if (Projectile.scale <= 0f)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] > 3f)
            {
                Projectile.velocity.Y += 0.075f;
                for (int i = 0; i < 3; i++)
                {
                    float velX = Projectile.velocity.X / 3f * (float)i;
                    float velY = Projectile.velocity.Y / 3f * (float)i;
                    int radius = 14;
                    int dusd = Dust.NewDust(new Vector2(Projectile.position.X + (float)radius, Projectile.position.Y + (float)radius), Projectile.width - radius * 2, Projectile.height - radius * 2, dustType, 0f, 0f, 100, Scale: 2);
                    Main.dust[dusd].noGravity = true;
                    Dust dusdagain = Main.dust[dusd];
                    Dust dusdforthethirdtime = dusdagain;
                    dusdforthethirdtime.velocity *= 0.1f;
                    dusdagain = Main.dust[dusd];
                    dusdforthethirdtime = dusdagain;
                    dusdforthethirdtime.velocity += Projectile.velocity * 0.5f;
                    Main.dust[dusd].position.X -= velX;
                    Main.dust[dusd].position.Y -= velY;
                }
                if (Main.rand.NextBool(8))
                {
                    int radius = 16;
                    int dusdsequel = Dust.NewDust(new Vector2(Projectile.position.X + (float)radius, Projectile.position.Y + (float)radius), Projectile.width - radius * 2, Projectile.height - radius * 2, dustType, 0f, 0f, 100, default(Color));
                    Dust dusdsequelagain = Main.dust[dusdsequel];
                    Dust nomoredusdgoddamit = dusdsequelagain;
                    nomoredusdgoddamit.velocity *= 0.25f;
                    dusdsequelagain = Main.dust[dusdsequel];
                    nomoredusdgoddamit = dusdsequelagain;
                    nomoredusdgoddamit.velocity += Projectile.velocity * 0.5f;
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int debuffType = BuffID.Ichor;
            switch (Projectile.ai[2])
            {
                case 0:
                    debuffType = ModContent.BuffType<BurningBlood>();
                    break;
                case 1:
                    debuffType = BuffID.OnFire3;
                    break;
                case 2:
                    debuffType = BuffID.Ichor;
                    break;
                case 3:
                    debuffType = BuffID.CursedInferno;
                    break;
                case 4:
                    debuffType = BuffID.Frostburn2;
                    break;
                case 5:
                    debuffType = BuffID.Venom;
                    break;
                case 6:
                    debuffType = ModContent.BuffType<HolyFlames>();
                    break;
            }
            target.AddBuff(debuffType, 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int debuffType = BuffID.Ichor;
            switch (Projectile.ai[2])
            {
                case 0:
                    debuffType = ModContent.BuffType<BurningBlood>();
                    break;
                case 1:
                    debuffType = BuffID.OnFire3;
                    break;
                case 2:
                    debuffType = BuffID.Ichor;
                    break;
                case 3:
                    debuffType = BuffID.CursedInferno;
                    break;
                case 4:
                    debuffType = BuffID.Frostburn2;
                    break;
                case 5:
                    debuffType = BuffID.Venom;
                    break;
                case 6:
                    debuffType = ModContent.BuffType<HolyFlames>();
                    break;
            }
            target.AddBuff(debuffType, 180);
        }
    }
}