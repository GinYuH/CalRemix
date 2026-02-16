using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class CombosamaSlash : MurasamaSlash
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/NamelessMurasamaSlash";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<MurasamaSlash>());
            Projectile.scale = 2f;
        }
        public override void PostAI()
        {
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 3f, Color.Yellow.ToVector3() * (Slashing ? 4.5f : 3f));
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 3f, Color.Red.ToVector3() * (Slashing ? 4.5f : 3f));
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 3f, Color.Violet.ToVector3() * (Slashing ? 4.5f : 3f));
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
                    GeneralParticleHandler.SpawnParticle(new AltSparkParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.5f, (float)target.height * 0.5f) + Projectile.velocity * 1.2f, vector * (Slash3 ? 1f : 0.65f), affectedByGravity: false, (int)((float)Main.rand.Next(23, 35) * (Slash3 ? 1.2f : 1f)), Main.rand.NextFloat(0.95f, 1.8f) * (Slash3 ? 1.4f : 1f), Main.rand.NextBool(3) ? Color.Yellow : Main.rand.NextBool() ? Color.Red : Color.Violet));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.frameCounter <= 1)
                return false;

            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D yellow = TextureAssets.Projectile[ModContent.ProjectileType<ParadiseMurasamaSlash>()].Value;
            Texture2D red = TextureAssets.Projectile[ModContent.ProjectileType<MurasamaSlash>()].Value;
            Rectangle rectangle = value.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            Main.EntitySpriteDraw(origin: rectangle.Size() * 0.5f, effects: (Projectile.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: value, position: Projectile.Center - Main.screenPosition + Projectile.velocity * 0.3f + new Vector2(0f, -32f).RotatedBy(Projectile.rotation), sourceRectangle: rectangle, color: Color.White, rotation: Projectile.rotation, scale: Projectile.scale);
            Main.EntitySpriteDraw(origin: rectangle.Size() * 0.5f, effects: (Projectile.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: yellow, position: Projectile.Center - Main.screenPosition + Projectile.velocity * 0.3f + new Vector2(0f, -32f).RotatedBy(Projectile.rotation).RotatedBy(-MathHelper.PiOver2), sourceRectangle: rectangle, color: Color.White, rotation: Projectile.rotation, scale: Projectile.scale);
            Main.EntitySpriteDraw(origin: rectangle.Size() * 0.5f, effects: (Projectile.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: red, position: Projectile.Center - Main.screenPosition + Projectile.velocity * 0.3f + new Vector2(0f, -32f).RotatedBy(Projectile.rotation).RotatedBy(MathHelper.PiOver2), sourceRectangle: rectangle, color: Color.White, rotation: Projectile.rotation, scale: Projectile.scale);
            return false;
        }
    }
}