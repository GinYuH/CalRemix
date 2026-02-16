using System;
using System.IO;
using System.Linq;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.Other;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MonorianDeathraySmall : BaseLaserbeamProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public int OwnerIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 2400f;
        public override float Lifetime => TelegraphTime + 30;
        public override Color LaserOverlayColor => Color.Cyan;
        public override Color LightCastColor => Color.White;
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;

        public static int TelegraphTime => 60;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)Lifetime;
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
            if (Main.npc[OwnerIndex].active && Main.npc[OwnerIndex].type == ModContent.NPCType<MonorianSoul>())
            {
                Projectile.Center = Main.npc[OwnerIndex].Center;
            }
            else
            {
                Projectile.Kill();
                return;
            }

            if (Main.npc[OwnerIndex].ModNPC<MonorianSoul>().CurrentPhase != MonorianSoul.PhaseType.Metagross)
            {
                Projectile.Kill();
                return;
            }
        }

        public override float DetermineLaserLength()
        {
            return MathHelper.Lerp(0, MaxLaserLength, Utils.GetLerpValue(0, 10, Time, true));
        }

        public override void ExtraBehavior()
        {
            int fade = 10;
            if (Projectile.timeLeft < fade)
            {
                Projectile.Opacity = MathHelper.Lerp(1, 0, Utils.GetLerpValue(fade, 0, Projectile.timeLeft, true));
            }
            else
            {
                Projectile.Opacity = MathHelper.Lerp(0, 1, Utils.GetLerpValue(ContentSamples.ProjectilesByType[Type].timeLeft, ContentSamples.ProjectilesByType[Type].timeLeft - TelegraphTime, Projectile.timeLeft, true));
            }
            if (Time == TelegraphTime)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemLaserMachinegun with { Pitch = -0.2f }, Projectile.Center);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/SylvestaffStreak"));

            List<Vector2> points = new List<Vector2>();

            Vector2 start = Projectile.Center + Projectile.velocity.ToRotation().ToRotationVector2();
            Vector2 destination = Projectile.Center + Projectile.velocity.ToRotation().ToRotationVector2() * DetermineLaserLength();

            Vector2 dist = destination - start;

            points.Add(start);

            int pointAmt = 90;

            for (int i = 1; i < pointAmt; i++)
            {
                points.Add(start + dist / (float)pointAmt * i);
            }

            points.Add(destination);


            float width = MathHelper.Lerp(5, 60, Utils.GetLerpValue(0, 20, Time, true));

            Color back = Time > TelegraphTime ? Color.Lerp(Color.Cyan, Color.LightCyan, 0.5f + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 22)) : Color.Cyan;

            PrimitiveRenderer.RenderTrail(points, new((float f) => width, (float f) => back * Projectile.Opacity, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);
            if (Time > TelegraphTime)
                PrimitiveRenderer.RenderTrail(points, new((float f) => width * 0.33f, (float f) => Color.White * Projectile.Opacity, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Time > TelegraphTime;
        }
    }
}
