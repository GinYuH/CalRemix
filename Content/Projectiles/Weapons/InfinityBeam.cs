using System;
using System.IO;
using System.Linq;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.Other;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Weapons;
using Terraria.Audio;
using CalamityMod.Particles;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class InfinityBeam : BaseLaserbeamProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public int OwnerIndex
        {
            get => (int)Projectile.owner;
            set => Projectile.owner = value;
        }
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 2400f;
        public override float Lifetime => 3000000;
        public override Color LaserOverlayColor => Main.DiscoColor;
        public override Color LightCastColor => Main.DiscoColor;
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AttachToSomething()
        {
            Player p = Main.player[OwnerIndex];
            if (p.active && !p.CCed && p.HeldItem.type == ModContent.ItemType<InfinityPebble>() && p.channel)
            {
                Projectile.Center = Main.player[OwnerIndex].Center;
                Projectile.timeLeft = 10;
                // The beam comes out of the tip of the crystal, not the side
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.spriteDirection = Projectile.direction;

                // The crystal is a holdout projectile, so change the player's variables to reflect that
                p.ChangeDir(Projectile.direction);
                p.heldProj = Projectile.whoAmI;
                p.itemTime = 2;
                p.itemAnimation = 2;

                // Multiplying by projectile.direction is required due to vanilla spaghetti.
                p.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            }
            else
            {
                Projectile.Kill();
                return;
            }
        }

        public override float DetermineLaserLength() => MaxLaserLength;

        public override void ExtraBehavior()
        {
            if (Main.player[Projectile.owner] == Main.LocalPlayer)
                Projectile.velocity = Main.player[Projectile.owner].DirectionTo(Main.MouseWorld);

            if (Time > 70 && Time % 5 == 0)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemMagicStar, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4) * Main.rand.NextBool().ToDirectionInt()) * 18, ModContent.ProjectileType<InfinitySoulBolt>(), (int)(Projectile.damage * 0.25f), Projectile.knockBack, Projectile.owner);
            }
            if (Time % 5 == 0)
            {
                GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(Projectile.Center + Projectile.velocity * 50, Projectile.velocity, Color.Orange, new Vector2(0.5f, 1f), Projectile.velocity.ToRotation(), 0.1f, Time > 70 ? 1f : 0.75f, 10));
                if (!Main.player[Projectile.owner].CheckMana(Main.player[Projectile.owner].HeldItem.mana, true))
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
                return false;

            Main.spriteBatch.ExitShaderRegion();

            Vector2 laserEnd = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * LaserLength;
            Vector2[] baseDrawPoints = new Vector2[8];
            for (int i = 0; i < baseDrawPoints.Length; i++)
                baseDrawPoints[i] = Vector2.Lerp(Projectile.Center, laserEnd, i / (float)(baseDrawPoints.Length - 1f));

            // Select textures to pass to the shader, along with the electricity color.
            GameShaders.Misc["CalamityMod:ArtemisLaser"].UseColor(Color.Cyan);
            GameShaders.Misc["CalamityMod:ArtemisLaser"].UseImage1("Images/Extra_189");
            GameShaders.Misc["CalamityMod:ArtemisLaser"].UseImage2("Images/Misc/Perlin");

            PrimitiveRenderer.RenderTrail(baseDrawPoints, new(LaserWidthFunction, LaserColorFunction, shader: GameShaders.Misc["CalamityMod:ArtemisLaser"]), 64);
            return false;
        }
        public float LaserWidthFunction(float f, Vector2 v)
        {
            float basescal = Projectile.scale * Projectile.width + 180;
            float pinch = 0.3f;
            if (f < pinch)
            {
                basescal = MathHelper.Lerp(0, basescal,CalamityUtils.ExpOutEasing(Utils.GetLerpValue(0, pinch, f, true), 1));
            }
            return basescal;
        }

        public static Color LaserColorFunction(float completionRatio, Vector2 v)
        {
            return CalamityUtils.MulticolorLerp((1 - completionRatio) + Main.GlobalTimeWrappedHourly * 0.4f, Color.Red, Color.Cyan, Color.Purple, Color.Teal, Color.DarkRed);
        }
    }
}
