using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenOrbitalFlare : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/FlareBomb";

        public Vector2 pivot = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro Flare");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 320;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
            writer.Write(pivot.X);
            writer.Write(pivot.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
            pivot.X = reader.ReadSingle();
            pivot.Y = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Main.zenithWorld)
            {
                Lighting.AddLight(Projectile.Center, 0.2f, 1.6f, 1.6f);
            }
            else
            {
                Lighting.AddLight(Projectile.Center, 1f, 1.6f, 0f);
            }

            int dust = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f);
                }
            }

            Projectile.rotation += Projectile.direction * 0.5f;
            if (pivot == Vector2.Zero)
            {
                pivot = Projectile.Center;
            }
            else
            {
                Projectile.localAI[2] += 0.75f * (Projectile.localAI[1] == 1 ? -1f : 1f);
                float distance = 100;
                distance = Projectile.localAI[2] * 10;
                double deg = 360 / Projectile.ai[1] * Projectile.ai[0] + Projectile.localAI[2] * 1.22f;
                double rad = deg * (Math.PI / 180);
                float hyposx = pivot.X - (int)(Math.Cos(rad) * distance) - Projectile.width / 2;
                float hyposy = pivot.Y - (int)(Math.Sin(rad) * distance) - Projectile.height / 2;

                Projectile.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!Main.zenithWorld) target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
            else target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            int dust = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            PyrogenFlare.DrawPyrogenFlare(Projectile, lightColor);
            return false;
        }
    }
}