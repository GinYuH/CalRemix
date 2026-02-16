using CalamityMod;
using CalamityMod.Particles;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class AnomalyOrbule : ModProjectile
    {
        public override string Texture => "CalRemix/Content/NPCs/Subworlds/GreatSea/AnomalyDiscipleChain";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            Lighting.AddLight(Projectile.Center, 0, 1, 1);
            switch (Projectile.ai[0])
            {
                case 0:
                    {
                        int maxSpeed = 10;
                        if (Projectile.velocity.Length() < maxSpeed && Projectile.ai[1] > 30)
                        {
                            Projectile.velocity *= 1.2f;
                        }
                    }
                    break;
                case 1:
                    {
                        int maxSpeed = 10;
                        if (Projectile.velocity.Length() < maxSpeed && Projectile.ai[1] > 30)
                        {
                            Projectile.velocity *= 1.2f;
                        }
                        if (Projectile.ai[1] == 70)
                        {
                            SoundEngine.PlaySound(AnomalyDisciple3.OrbuleSound with { Pitch = 0.5f }, Projectile.Center);
                            GeneralParticleHandler.SpawnParticle(new BloomParticle(Projectile.Center, Vector2.Zero, Color.SeaGreen, 0.2f, 0.8f, 30));
                            Projectile.velocity = Projectile.DirectionTo(Main.player[(int)Projectile.ai[2]].Center) * 16;
                        }
                    }
                    break;
            }
        }

        public override void OnKill(int timeLeft)
        {
            GeneralParticleHandler.SpawnParticle(new BloomParticle(Projectile.Center, Vector2.Zero, Color.SeaGreen, 0.2f, 0.8f, 30));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, Color.Turquoise * 0.6f, 0, bloom.Size() / 2, Projectile.scale * 0.3f, 0);
            Main.spriteBatch.ExitShaderRegion();
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2, Projectile.scale, 0);
            return false;
        }
    }
}