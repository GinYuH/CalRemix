using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

//shamelessly 'borrowed' from carcinogen since they behave almost the same... different visuals and statuses though!

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenFlareStatic2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro Flare");
            Main.projFrames[Projectile.type] = 5;
        }

        public override string Texture => "CalamityMod/Projectiles/Boss/FlareBomb";

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 64;
            Projectile.height = 66;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240; //killed by hostile projectiles being despawned, as opposed to on timer
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

            Lighting.AddLight(Projectile.Center, 1f, 1.6f, 0f);
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }

        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return Projectile.position.Y > Main.player[(int)Projectile.ai[0]].Bottom.Y;
        }
    }
}