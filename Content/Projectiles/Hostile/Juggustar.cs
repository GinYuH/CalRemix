using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class Juggustar : ModProjectile
    {
        public override string Texture => "CalamityMod/Particles/Sparkle";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.extraUpdates = 10;
                int jug = NPC.FindFirstNPC(ModContent.NPCType<Juggular>());
                if (jug != -1)
                {
                    NPC juggular = Main.npc[jug];
                    if (juggular.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        Projectile.Kill();
                    }
                    if (Projectile.ai[1] > 300)
                    {
                        Vector2 newpos = Vector2.Lerp(Projectile.Center, juggular.Center, 0.03f);
                        Projectile.velocity = newpos - Projectile.Center;
                    }
                }
                Projectile.ai[1]++;
            }
            if (Main.rand.NextBool(600) && Projectile.velocity != Vector2.Zero)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemMagicStar with { Pitch = 0.8f, Volume = 0.8f }, Projectile.Center);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);


            if (Projectile.ai[0] == 1 && Projectile.velocity == Vector2.Zero)
            {
                int jug = NPC.FindFirstNPC(ModContent.NPCType<Juggular>());
                if (jug != -1)
                {
                    NPC juggular = Main.npc[jug];
                    Texture2D arrow = ModContent.Request<Texture2D>("CalamityMod/Particles/TitaniumRailgunShellGlow").Value;
                    float comp = Utils.GetLerpValue(600, 560, Projectile.timeLeft, true);
                    Main.spriteBatch.Draw(arrow, Projectile.Center - Main.screenPosition, null, Color.Gold * 0.4f * comp, Projectile.DirectionTo(juggular.Center).ToRotation() + MathHelper.PiOver2, new Vector2(arrow.Width / 2, arrow.Height), new Vector2(1, (Projectile.Distance(juggular.Center) * 0.8f * comp) / (float)arrow.Height), 0, 0);

                }
            }



            Texture2D starr = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(starr, Projectile.Center - Main.screenPosition, null, Color.Magenta, 5 * Main.GlobalTimeWrappedHourly * (Projectile.whoAmI % 2 == 0).ToDirectionInt(), starr.Size() / 2, Projectile.scale, 0, 0);


            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
            Vector2 trailOffset = Projectile.Size * 0.5f;
            trailOffset += (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_) => trailOffset, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), 61);

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            if (Projectile.ai[0] == 1)
            {
                return Projectile.velocity != Vector2.Zero;
            }
            return true;
        }
        public float FlameTrailWidthFunction(float completionRatio) => MathHelper.SmoothStep(12f * Projectile.scale, 8f * Projectile.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0.8f, 0.27f, completionRatio, true) * Utils.GetLerpValue(0f, 0.067f, completionRatio, true);
            return Color.PaleGoldenrod * trailOpacity;
        }
    }
}