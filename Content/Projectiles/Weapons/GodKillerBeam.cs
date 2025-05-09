using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using ReLogic.Utilities;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GodKillerBeam : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public SlotId BeamSoundSlot;

        public ref float Timer => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/GreyscaleTextures/FieryBeam"));

            List<Vector2> points = new List<Vector2>();

            int distMax = 3000;

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
            PrimitiveRenderer.RenderTrail(points, new(FlameTrailWidthFunction, FlameTrailColorFunction, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);
            PrimitiveRenderer.RenderTrail(points, new(FlameTrailFrontWidthFunction, FlameTrailFrontColorFunction, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);

            Main.spriteBatch.ExitShaderRegion();

            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type != ModContent.ProjectileType<GodKillerEXHoldout>())
                    continue;
                if (p.owner == Projectile.owner)
                {
                    Color c = Lighting.GetColor(p.Center.ToSafeTileCoordinates());
                    ProjectileLoader.PreDraw(p, ref c);
                }
            }
            return false;
        }
        public float FlameTrailWidthFunction(float completionRatio)
        {
            int widthMax = 200;
            int widthMin = 88;
            int widthStart = 10;
            float startFull = 0.025f;
            float ret = 0;
            if (completionRatio < startFull)
            {
                ret = MathHelper.Lerp(widthStart * Projectile.scale, widthMin * Projectile.scale, CalamityUtils.SineOutEasing((float)Utils.GetLerpValue(0f, startFull, completionRatio, true), 1));
            }
            else if (completionRatio > 1 - (startFull * 2))
            {
                ret = MathHelper.Lerp(widthMax * Projectile.scale, 0, CalamityUtils.ExpOutEasing((float)Utils.GetLerpValue(1 - startFull * 2, 1, completionRatio, true), 1));
            }
            else
            {
                ret = MathHelper.SmoothStep(widthMin * Projectile.scale, widthMax * Projectile.scale, completionRatio);
            }
            if (Projectile.ai[2] == 1)
            {
                ret = MathHelper.Lerp(ret, 0, CalamityUtils.ExpInEasing(Utils.GetLerpValue(10, 0, Projectile.timeLeft, true), 1));
            }
            return ret;
        }

        public Color FlameTrailColorFunction(float completionRatio)
        {
            return Color.Cyan * MathHelper.Clamp(1 - completionRatio, 0.4f, 0.6f);
        }
        public float FlameTrailBackWidthFunction(float completionRatio)
        {
            return FlameTrailWidthFunction(completionRatio) * 4f;
        }

        public Color FlameTrailBackColorFunction(float completionRatio)
        {
            return Color.Cyan * 0.1f;
        }
        public float FlameTrailFrontWidthFunction(float completionRatio)
        {
            return FlameTrailWidthFunction(completionRatio) * 0.6f;
        }

        public Color FlameTrailFrontColorFunction(float completionRatio)
        {
            return Color.White;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.Center, Owner.Center + unit * 3000, 88, ref point);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 300);
        }

        public override void AI()
        {
            if (Owner is null) return;
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<GodKillerEXHoldout>()] <= 0)
                Projectile.Kill();
            Timer++;

            Projectile.localAI[2]++;
            if (!Owner.channel)
                Projectile.ai[2] = 1;

            if (Projectile.ai[2] == 0)
                Projectile.timeLeft = 10;

            if (Projectile.ai[2] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.position = Owner.Center + Projectile.rotation.ToRotationVector2() * (Projectile.DirectionTo(Main.MouseWorld).X > 0 ? 8 : 22);
            }
           // else
               // Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 80;

            if (Timer == 1)
            BeamSoundSlot = SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/GodKillerLoop") with { Pitch = 0.4f }, Projectile.Center);
            if (SoundEngine.TryGetActiveSound(BeamSoundSlot, out var deathraySound) && deathraySound.IsPlaying)
                deathraySound.Position = Projectile.Center;
            
            // Start the loop sound if the start sound finished.
            if (deathraySound is null || !deathraySound.IsPlaying)
            {
                if (deathraySound is null)
                {
                    deathraySound?.Stop();
                    BeamSoundSlot = SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/GodKillerLoop") with { Pitch = 0.4f }, Projectile.Center);
                }
                else if (deathraySound is not null)
                    deathraySound.Resume();
            }
            

            UpdatePlayer(Owner);
            ChargeLaser(Owner);
            //SetLaserPosition(Owner);
            CastLights();
        }

        private void SetLaserPosition(Player player)
        {
        }

        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(BeamSoundSlot, out var deathraySound) && deathraySound.IsPlaying)
                deathraySound?.Stop();
            SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/GodKillerEnd"), Projectile.Center);
        }

        private void ChargeLaser(Player player)
        {
            /*if (!player.controlUseItem || !player.active || player.dead)
            {
                SoundEngine.TryGetActiveSound(BeamSoundSlot, out var deathraySound);
                Projectile.Kill();
                deathraySound?.Stop();
            }*/
        }

        private void UpdatePlayer(Player player)
        {
            if (Projectile.owner == Main.myPlayer && Projectile.ai[2] == 0)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                Projectile.velocity = diff;
                Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                Projectile.netUpdate = true;
            }
            int dir = Projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
        }

        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 3000, 26, DelegateMethods.CastLight);
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