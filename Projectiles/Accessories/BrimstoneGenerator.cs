using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using CalamityMod.Graphics.Metaballs;

namespace CalRemix.Projectiles.Accessories
{
    public class BrimstoneGenerator : ModProjectile //I wrote this, I can do this
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public bool start = true;
        public StreamGougeMetaball.CosmicParticle voidaura;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone Portal");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();

            if (modPlayer.tvohide)
            {
                Projectile.active = false;
            }
            if (!modPlayer.brimPortal)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
                modPlayer.brimPortal = false;
            if (modPlayer.brimPortal)
                Projectile.timeLeft = 2;

            if (Projectile.ai[1] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }

            Vector2 vector = player.Center - Projectile.Center;

            Projectile.Center = player.Center + new Vector2(300, 0).RotatedBy(Projectile.ai[1] + Projectile.ai[0] * MathHelper.PiOver2);

            Projectile.ai[1] += 0.025f;

            Projectile.velocity.X = (vector.X > 0f) ? -0.000001f : 0f;

            for (int k = 0; k < Main.projectile.Length; k++)
            {
                var proj = Main.projectile[k];
                bool tvo = Main.player[Projectile.owner].GetModPlayer<CalRemixPlayer>().tvo;
                bool tvoar = tvo ? proj.friendly : proj.arrow;
                if (proj.active && proj.owner == Projectile.owner && tvoar && !proj.GetGlobalProjectile<CalRemixProjectile>().nihilicArrow && proj.friendly && Vector2.Distance(proj.Center, Projectile.Center) < 65)
                {
                    Main.projectile[k].damage = (int)(proj.damage * 3f);
                    proj.extraUpdates += 1;
                    Main.projectile[k].GetGlobalProjectile<CalRemixProjectile>().nihilicArrow = true;
                    SoundEngine.PlaySound(SoundID.Item104 with { Volume = SoundID.Item104.Volume * 0.75f }, Projectile.Center);

                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 dustpos = Vector2.UnitX * (float)-(float)proj.width / 2f;
                        dustpos += -Vector2.UnitY.RotatedBy((double)((float)i * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                        dustpos = dustpos.RotatedBy((double)(proj.rotation - 1.57079637f), default);
                        int dust = Dust.NewDust(proj.Center, 0, 0, 27, 0f, 0f, 100, Color.Red, 1f);
                        Main.dust[dust].scale = 1.1f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = proj.Center + dustpos;
                        Main.dust[dust].velocity = proj.velocity * 0.1f;
                        Main.dust[dust].velocity = Vector2.Normalize(proj.Center - proj.velocity * 3f - Main.dust[dust].position) * 1.25f;
                    }
                }
            }

            if (voidaura == null)
            {
                voidaura = VoidGeneratorMetaball.SpawnParticle(Projectile.Center, Vector2.Zero, 800f);
            }
            else
            {
                voidaura.Center = Projectile.Center;
                voidaura.Size = 800;
            }
        }
        public override bool? CanCutTiles() => false;
    }
}
