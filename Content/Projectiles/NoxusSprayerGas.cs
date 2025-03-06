using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using CalRemix.Content.Items.Tools;

namespace CalRemix.Content.Projectiles
{
    public class NoxusSprayerGas : ModProjectile
    {
        public ref float Time => ref Projectile.ai[0];

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 9;
            Projectile.timeLeft = Projectile.MaxUpdates * 20;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            // Create gas.
            float particleScale = GetLerpValue(0f, 32f, Time, true);
            var particle = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(0.9f, 0.9f), Color.MediumPurple, 15, particleScale, particleScale * 0.4f, 0.05f, true, 0f, true)
            {
                Rotation = Main.rand.NextFloat(TwoPi)
            };
            GeneralParticleHandler.SpawnParticle(particle);

            DeleteEverything();

            Time++;
        }

        public void DeleteEverything()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || !n.Hitbox.Intersects(Projectile.Hitbox) || NoxusSprayer.NPCsToNotDelete.Contains(n.type))
                    continue;

                n.active = false;

                for (int j = 0; j < 20; j++)
                {
                    float gasSize = n.width * Main.rand.NextFloat(0.1f, 0.8f);
                    NoxusGasMetaball.CreateParticle(n.Center + Main.rand.NextVector2Circular(40f, 40f), Main.rand.NextVector2Circular(4f, 4f), gasSize);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) => false;
    }
}
