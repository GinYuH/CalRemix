using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.Utils;
using static System.MathF;
using CalRemix.Core.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class NightmareDeathRay : ModProjectile
    {
        public PrimitiveTrailCopy BeamDrawer
        {
            get;
            set;
        }

        public float LaserLength
        {
            get;
            set;
        }

        public Vector3 Rotation;

        public Matrix RotationMatrix;

        public float DarknessInterpolant
        {
            get
            {
                if (EntropicGod.Myself is null)
                    return 0f;

                Vector3 direction3D = Vector3.Transform(Vector3.UnitX, RotationMatrix);
                Vector2 direction = new(direction3D.X, -direction3D.Y * EntropicGod.Myself.ModNPC<EntropicGod>().LaserLengthFactor);
                return GetLerpValue(0.65f, 1f, Vector2.Dot(direction.SafeNormalize(Vector2.Zero), Vector2.UnitY), true);
            }
        }

        public ref float Time => ref Projectile.ai[0];

        public ref float Lifetime => ref Projectile.ai[1];

        public static float SquishFactor => 1.98f;

        public const float MaxLaserLength = 4600f;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 225;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9000;
            Projectile.alpha = 255;
            Projectile.Calamity().DealsDefenseDamage = true;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(LaserLength);
            writer.Write(Rotation.X);
            writer.Write(Rotation.Y);
            writer.Write(Rotation.Z);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            LaserLength = reader.ReadSingle();
            Rotation = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public override void AI()
        {
            // Immediately die if Noxus is not present.
            if (EntropicGod.Myself is null)
            {
                Projectile.Kill();
                return;
            }

            // Stick above Noxus.
            Projectile.Center = EntropicGod.Myself.Center - Vector2.UnitY.RotatedBy(EntropicGod.Myself.rotation) * EntropicGod.Myself.scale * 200f;

            // Fade in.
            Projectile.alpha = Clamp(Projectile.alpha - 25, 0, 255);

            // Determine the scale of the laser.
            CalculateScale();

            // The laser technically always faces to the right for Terraria's 2D system.
            // In 3D, however, the laser orientation is controlled by custom rotation values that are inherited from Noxus.
            // The matrix is stored as a variable instead of a property since having it used 200 times every frame for NPC collision loops would not be ideal.
            Projectile.velocity = Vector2.UnitX;
            Rotation = EntropicGod.Myself.ModNPC<EntropicGod>().LaserRotation;
            RotationMatrix = CreateRotationMatrix(Rotation) * Matrix.CreateScale(1f, 1f / SquishFactor, 1f);

            if (Time >= Lifetime)
                Projectile.Kill();

            // Make the laser quickly move outward.
            LaserLength = Pow(GetLerpValue(4f, 30f, Time, true), 2.4f) * MaxLaserLength * SquishFactor * EntropicGod.Myself.ModNPC<EntropicGod>().LaserLengthFactor;

            // And create bright light.
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 1.4f);

            Time++;
        }

        public static Matrix CreateRotationMatrix(Vector3 rotations)
        {
            return Matrix.CreateRotationZ(rotations.Z) * Matrix.CreateRotationY(rotations.Y) * Matrix.CreateRotationX(rotations.X);
        }

        public void CalculateScale()
        {
            Projectile.scale = CalamityUtils.Convert01To010(Time / Lifetime) * 1.45f;
            if (Projectile.scale > 1f)
                Projectile.scale = 1f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Time <= 30f || Projectile.timeLeft <= 10)
                return false;

            float _ = 0f;
            float width = Projectile.width * 0.4f;

            Vector3 direction3D = Vector3.Transform(Vector3.UnitX, RotationMatrix);
            Vector2 direction = new(direction3D.X, -direction3D.Y);

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + direction * LaserLength, width, ref _);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<NoxusFumes>(), EntropicGod.DebuffDuration_PowerfulAttack);
        }

        public float WidthFunction(float completionRatio, Vector2 v)
        {
            if (EntropicGod.Myself is null)
                return 0f;

            float squeezeInterpolant = GetLerpValue(1f, 0.92f, completionRatio, true);
            return MathHelper.SmoothStep(2f, Projectile.width, squeezeInterpolant) * Pow(completionRatio, 0.3f) * EntropicGod.Myself.ModNPC<EntropicGod>().LaserSquishFactor * 1.36f;
        }

        public Color ColorFunction(float completionRatio, Vector2 v)
        {
            Color color = Color.Lerp(Color.DarkSlateBlue, Color.HotPink, 0.3f);
            color = Color.Lerp(color, Color.DeepSkyBlue, 0.12f);

            // Make the color a bit darker when pointing downward.
            color = Color.Lerp(color, Color.Black, DarknessInterpolant * 0.4f + 0.16f);

            return color * Projectile.Opacity * GetLerpValue(1f, 0.8f, completionRatio, true) * Projectile.scale * 1.15f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalRemixHelper.SetBlendState(Main.spriteBatch, BlendState.Additive);
            DrawFrontGlow();
            DrawBloomFlare();
            DrawLaser();
            Main.spriteBatch.ResetBlendState();

            return false;
        }

        public void DrawFrontGlow()
        {
            float pulse = Cos(Main.GlobalTimeWrappedHourly * 36f);
            Texture2D backglowTexture = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;
            Vector2 origin = backglowTexture.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.scale * 20f;
            Vector2 baseScale = new Vector2(1f + pulse * 0.05f, 1f) * Projectile.scale * 0.5f;
            Main.spriteBatch.Draw(backglowTexture, drawPosition, null, Color.White, 0f, origin, baseScale * 0.7f, 0, 0f);
            Main.spriteBatch.Draw(backglowTexture, drawPosition, null, Color.Violet * 0.4f, 0f, origin, baseScale * 1.2f, 0, 0f);
            Main.spriteBatch.Draw(backglowTexture, drawPosition, null, Color.Blue * 0.3f, 0f, origin, baseScale * 1.7f, 0, 0f);
        }

        public void DrawBloomFlare()
        {
            Texture2D bloomFlare = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/BloomFlare").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.scale * 20f;
            Color bloomFlareColor = Color.Lerp(Color.Wheat, Color.Blue, 0.7f);
            float bloomFlareRotation = Main.GlobalTimeWrappedHourly * 1.76f;
            float bloomFlareScale = Projectile.scale * 0.33f;
            Main.spriteBatch.Draw(bloomFlare, drawPosition, null, bloomFlareColor, -bloomFlareRotation, bloomFlare.Size() * 0.5f, bloomFlareScale, 0, 0f);

            bloomFlareColor = Color.Lerp(Color.Wheat, Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.2f + 0.5f) % 1f, 1f, 0.55f), 0.7f);
            bloomFlareColor = Color.Lerp(bloomFlareColor, Color.DarkBlue, 0.63f);
            Main.spriteBatch.Draw(bloomFlare, drawPosition, null, bloomFlareColor, bloomFlareRotation, bloomFlare.Size() * 0.5f, bloomFlareScale, 0, 0f);
        }

        public void DrawLaser()
        {
            var laserShader = GameShaders.Misc[$"{Mod.Name}:NoxusLaserShader"];
            BeamDrawer ??= new PrimitiveTrailCopy(WidthFunction, ColorFunction, null, true, laserShader);

            laserShader.UseSaturation(4326f);
            laserShader.UseOpacity(0.5f);
            laserShader.UseColor(Color.Lerp(Color.White, Color.Black, DarknessInterpolant * 0.4f + 0.16f) * Pow(Projectile.scale, 1.8f));
            laserShader.SetShaderTexture(ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/GreyscaleTextures/MoltenNoise"));
            laserShader.Shader.Parameters["uStretchReverseFactor"].SetValue(0.3f);

            List<Vector2> points = new();
            for (int i = 0; i <= 32; i++)
                points.Add(Vector2.Lerp(Projectile.Center, Projectile.Center + Vector2.UnitX * LaserLength, i / 32f));

            if (Time >= 2f)
            {
                BeamDrawer.SpecifyPerspectiveMatrixMultiplier(CreateRotationMatrix(Rotation) * Matrix.CreateScale(1f, 1f / SquishFactor, 1f));
                BeamDrawer.Draw(points, Projectile.Center, 59);
            }
        }

        public override bool ShouldUpdatePosition() => false;
    }
}
