using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class NamelessMurasamaSpin : ModProjectile
    {
        public VertexStrip TrailDrawer;
        Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityMod/Items/Weapons/UHFMurasama";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cigar Cinder");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = 170;
            Projectile.height = 170;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.6f;
        }

        public override void AI()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.rotation += 0.5f * Owner.direction;
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 60 && Projectile.ai[0] % 30 == 0)
            {
                SoundEngine.PlaySound(NamelessMurasama.LongCoolSlash with { PitchVariance = 0.2f, Pitch = -0.2f }, Owner.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Owner.DirectionTo(Main.MouseWorld) * 20, ModContent.ProjectileType<NamelessVortex>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
            }
            StickToOwner();
            AdjustPlayerValues();
            if (!Owner.controlUseTile || Owner.HeldItem.type != ModContent.ItemType<NamelessMurasama>() || Owner.CCed)
            {
                Projectile.Kill();
            }
        }
        public void AdjustPlayerValues()
        {
            Projectile.spriteDirection = Projectile.direction;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.direction * Projectile.velocity).ToRotation();

            // Decide the arm rotation for the owner.
            float armRotation = Projectile.rotation - (Owner.direction == 1 ? MathHelper.PiOver2 : -MathHelper.PiOver2);
            Owner.SetCompositeArmFront(Math.Abs(armRotation) > 0.01f, Player.CompositeArmStretchAmount.Full, armRotation);
        }

        public void StickToOwner()
        {
            // Glue the sword to its owner. This applies a handful of offsets to make the blade look like it's roughly inside of the owner's hand.
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Projectile.Center -= Projectile.velocity.SafeNormalize(Vector2.UnitY) * new Vector2(20, 15f + Projectile.scale * 8f);

            // Set the owner's held projectile to this and register a false item time calculation.
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);
        }

        public Color TrailColorFunction(float completionRatio)
        {
            float opacity = (float)Math.Pow(Utils.GetLerpValue(1f, 0.45f, completionRatio, true), 4D) * Projectile.Opacity * 0.48f;
            return Color.Lerp(Color.Violet, Color.Magenta, MathHelper.Clamp(completionRatio * 1.4f, 0f, 1f)) * opacity;
        }

        public float TrailWidthFunction(float completionRatio) => Projectile.height * (1f - completionRatio) * 0.8f;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            Vector2 origin = Projectile.spriteDirection == 1 ? new Vector2(0, frame.Height) : new Vector2(frame.Width, frame.Height);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            SpriteEffects direction = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Draw the afterimage trail.
            TrailDrawer ??= new();
            GameShaders.Misc["EmpressBlade"].UseImage0("Images/Extra_201");
            GameShaders.Misc["EmpressBlade"].UseImage1("Images/Extra_193");
            GameShaders.Misc["EmpressBlade"].UseShaderSpecificData(new Vector4(1f, 0f, 0f, 0.6f));
            GameShaders.Misc["EmpressBlade"].Apply(null);
            TrailDrawer.PrepareStrip(Projectile.oldPos, Projectile.oldRot, TrailColorFunction, TrailWidthFunction, Projectile.Size * 0.5f - Main.screenPosition, Projectile.oldPos.Length, true);
            TrailDrawer.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            // Draw the blade.
            Main.EntitySpriteDraw(texture, drawPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation + 0.2f, origin, Projectile.scale, direction, 0);

            // Reset textures for shaders, since they're only defined once at load-time in vanilla.
            GameShaders.Misc["EmpressBlade"].UseImage0("Images/Extra_209");
            GameShaders.Misc["EmpressBlade"].UseImage1("Images/Extra_210");
            return false;
        }
    }
}