using System.Collections.Generic;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalamityMod.World;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class IonogenLightning : BaseLaserbeamProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public NPC Owner => Projectile.ai[1] > -1 ? Main.npc[(int)Projectile.ai[1]] : null;
        public override Color LightCastColor => Projectile.ai[2] > 60 ? Color.Yellow  * 0.5f : new Color(92, 144, 245);
        public override float Lifetime => 18000f;
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 3200f; //100 tiles
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;

        List<Vector2> offsetPoints = new List<Vector2>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.timeLeft = Ionogen.LightningLife;
        }

        public override bool PreAI()
        {
            if (Owner != null)
            {
                if (!Owner.active || Owner.life <= 0 || Owner.type != ModContent.NPCType<Ionogen>())
                {
                    Projectile.Kill();
                    return false;
                }
            }
            Projectile.ai[2]++;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Owner != null)
                Projectile.Center = Owner.Center;

            if (Projectile.ai[1] <= -1 && Projectile.ai[2] % 5 == 0)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
            }
            if (Projectile.ai[2] == 60)
            {
                SoundEngine.PlaySound(CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrimstoneShotSound, Projectile.Center);
            }
            if (Main.getGoodWorld)
            {
                float one = Projectile.whoAmI % 2 == 0 && Main.zenithWorld ? -0.004f : 0.004f;
                if (CalamityWorld.revenge && CalamityWorld.LegendaryMode)
                {
                    one *= 2;
                }
                Projectile.velocity += (Projectile.velocity.ToRotation() + one).ToRotationVector2();
            }

            return true;
        }

        internal float WidthFunction(float completionRatio)
        {
            return Projectile.ai[2] > 60 ? 0.4f : 0.2f;
        }

        internal Color ColorFunction(float completionRatio)
        {
            return new Color(174, 227, 244);
        }

        internal float BackgroundWidthFunction(float completionRatio) => WidthFunction(completionRatio) * 4f;

        internal Color BackgroundColorFunction(float completionRatio)
        {
            return Projectile.ai[2] > 60 ? Color.Yellow * 0.6f : Color.LightBlue * 0.4f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion();
            GameShaders.Misc["CalamityMod:TeslaTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ZapTrail"));

            // every 2 frames update the offsets are randomized. This is effectively control for the lightning's fps
            if (Projectile.ai[2] % 2 == 0)
            {
                offsetPoints.Clear();
                for (int i = 0; i <= 75; i++)
                {
                    Vector2 baseVec = Vector2.Zero;
                    float width = 16;
                    if (i > 0)
                    {
                        baseVec += Main.rand.NextVector2Square(-width, width);
                    }
                    offsetPoints.Add(Main.rand.NextVector2Square(-width, width));
                }
            }
            // do not try to draw the lightning if the offset list isn't filled yet or an index error will occur
            if (offsetPoints.Count < 75)
                return false;

            // the final list of points that will be used to draw the lightning which combines a series of points travelling up the beam with the random values from the offset list
            List<Vector2> finalPoints = new List<Vector2>();
            for (int i = 0; i <= 75; i++)
            {
                Vector2 baseVec = Vector2.Lerp(Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, i / 73.5f);
                float width = 16 + (i / 75 * 10);
                if (i > 0)
                {
                    baseVec += offsetPoints[i];
                }
                finalPoints.Add(baseVec);
            }

            // 29FEB2024: Ozzatron: hopefully ported this correctly to the new prim system by Toasty
            PrimitiveRenderer.RenderTrail(finalPoints, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false, shader: GameShaders.Misc["CalamityMod:TeslaTrail"]), 75);
            PrimitiveRenderer.RenderTrail(finalPoints, new(WidthFunction, ColorFunction, smoothen: false, shader: GameShaders.Misc["CalamityMod:TeslaTrail"]), 75);

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
        public override bool ShouldUpdatePosition() => false;

        // Update CutTiles so the laser will cut tiles (like grass).
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * LaserLength, Projectile.width + 16, DelegateMethods.CutTiles);
        }

        public override bool? CanDamage()
        {
            return Projectile.ai[2] > 60;
        }
    }
}
