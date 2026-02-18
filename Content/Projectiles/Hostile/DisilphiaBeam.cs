using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using ReLogic.Utilities;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.NPCs.Subworlds.Sealed;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class DisilphiaBeam : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public SlotId BeamSoundSlot;

        public ref float Timer => ref Projectile.ai[1];
        public NPC Owner => Main.npc[(int)Projectile.ai[2]];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/BasicTrail"));

            List<Vector2> points = new List<Vector2>();

            int distMax = 30000;

            Vector2 start = Projectile.Center + Projectile.rotation.ToRotationVector2() * 10;
            Vector2 destination = Projectile.Center + Projectile.rotation.ToRotationVector2() * MathHelper.Lerp(0, distMax, Utils.GetLerpValue(0, 10, Projectile.localAI[2], true));

            Vector2 dist = destination - start;

            points.Add(start);

            int pointAmt = 90;

            for (int i = 1; i < pointAmt; i++)
            {
                points.Add(start + dist / (float)pointAmt * i);
            }

            points.Add(destination);

            PrimitiveRenderer.RenderTrail(points, new(FlameTrailBackWidthFunction, FlameTrailBackColorFunction, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/DoubleTrail"));
            PrimitiveRenderer.RenderTrail(points, new(FlameTrailWidthFunction, FlameTrailColorFunction, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);
            PrimitiveRenderer.RenderTrail(points, new(FlameTrailFrontWidthFunction, FlameTrailFrontColorFunction, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }
        public float FlameTrailWidthFunction(float completionRatio, Vector2 v)
        {
            int widthMax = 1400;
            if (completionRatio < 0.002f)
            {
                return MathHelper.Lerp(0, widthMax, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, 0.002f, completionRatio, true), 1));
            }
            return widthMax;
        }

        public Color FlameTrailColorFunction(float completionRatio, Vector2 v)
        {
            return Color.Red * MathHelper.Clamp(1 - completionRatio, 0.4f, 0.6f) * Utils.GetLerpValue(0, 60, Projectile.timeLeft, true);
        }
        public float FlameTrailBackWidthFunction(float completionRatio, Vector2 v)
        {
            return FlameTrailWidthFunction(completionRatio, v) * 4f;
        }

        public Color FlameTrailBackColorFunction(float completionRatio, Vector2 v)
        {
            return Color.Red * 0.1f * Utils.GetLerpValue(0, 60, Projectile.timeLeft, true);
        }
        public float FlameTrailFrontWidthFunction(float completionRatio, Vector2 v)
        {
            return FlameTrailWidthFunction(completionRatio, v) * 0.6f;
        }

        public Color FlameTrailFrontColorFunction(float completionRatio, Vector2 v)
        {
            return Color.White * Utils.GetLerpValue(0, 60, Projectile.timeLeft, true);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.Center, Owner.Center + unit * 3000, Projectile.scale * 900, ref point);
        }

        public override void AI()
        {
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
            if (Owner is null) return;
            if ((Owner.life < 0 || !Owner.active) || (Owner.type != ModContent.NPCType<Disilphia>()))
                Projectile.Kill();
            Timer++;

            Projectile.localAI[2]++;

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.position = Owner.Center + Projectile.rotation.ToRotationVector2() + new Vector2(Owner.direction * 120, -60);

            if (Projectile.localAI[2] > 60)
                Projectile.velocity.Y -= 0.005f;
            CastLights();
        }

        public override void OnKill(int timeLeft)
        {
        }

        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(1, 0, 0);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 3000, Projectile.scale * 1000, DelegateMethods.CastLight);
        }
        public override bool ShouldUpdatePosition()
        {
            if (Projectile.ai[2] == 0)
                return false;
            return true;
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * 3000, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}