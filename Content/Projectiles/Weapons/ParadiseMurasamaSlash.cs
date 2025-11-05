using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ParadiseMurasamaSlash : MurasamaSlash
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<MurasamaSlash>());
            Projectile.scale = 0.7f;
        }
        public override void PostAI()
        {
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 3f, Color.Yellow.ToVector3() * (Slashing ? 4.5f : 3f));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float pitch = (Slash2 ? (-0.1f) : (Slash3 ? 0.1f : (Slash1 ? (-0.15f) : 0f)));
            if (target.Organic())
                SoundEngine.PlaySound(Murasama.OrganicHit with { Pitch = pitch }, Projectile.Center);
            else
                SoundEngine.PlaySound(Murasama.InorganicHit with { Pitch = pitch }, Projectile.Center);

            float num2 = MathHelper.Clamp(Slash3 ? (18 - Projectile.numHits * 3) : (5 - Projectile.numHits * 2), 0f, 18f);
            for (int j = 0; j < num2; j++)
            {
                Vector2 vector = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(45f));
                if (Main.rand.NextBool())
                    GeneralParticleHandler.SpawnParticle(new AltSparkParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.5f, (float)target.height * 0.5f) + Projectile.velocity * 1.2f, vector * (Slash3 ? 1f : 0.65f), affectedByGravity: false, (int)((float)Main.rand.Next(23, 35) * (Slash3 ? 1.2f : 1f)), Main.rand.NextFloat(0.95f, 1.8f) * (Slash3 ? 1.4f : 1f), Color.Yellow));
            }
        }
    }
}