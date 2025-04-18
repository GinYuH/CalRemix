using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenZigZagFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pyro Flare");
            Main.projFrames[Projectile.type] = 5;
        }

        public override string Texture => "CalamityMod/Projectiles/Boss/FlareBomb";

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

            Projectile.ai[0]++;
            if (Projectile.ai[0] % 10 == 0)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(Projectile.ai[0] % 20 == 0 ? MathHelper.PiOver2 : - MathHelper.PiOver2) * 8;
            }

            int dust = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f);
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!Main.zenithWorld) target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
            else target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            int dust = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
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