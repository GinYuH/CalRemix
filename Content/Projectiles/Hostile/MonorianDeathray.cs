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

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MonorianDeathray : BaseLaserbeamProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public int OwnerIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 2400f;
        public override float Lifetime => 300;
        public override Color LaserOverlayColor => Color.Cyan;
        public override Color LightCastColor => Color.White;
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", AssetRequestMode.ImmediateLoad).Value;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
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

            if (Main.npc[OwnerIndex].ModNPC<MonorianSoul>().CurrentPhase != MonorianSoul.PhaseType.Laser)
            {
                Projectile.Kill();
                return;
            }
        }

        public override float DetermineLaserLength()
        {
            int gem = NPC.FindFirstNPC(ModContent.NPCType<MonorianGemBoss>());

            if (gem == -1)
            {
                return MaxLaserLength;
            }
            else
            {
                NPC gemNPC = Main.npc[gem];
                float point = 0f;
                if (Collision.CheckAABBvLineCollision(gemNPC.Hitbox.TopLeft(), gemNPC.Hitbox.Size(), Main.npc[OwnerIndex].Center, Main.npc[OwnerIndex].Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * MaxLaserLength, 20, ref point))
                {
                    return gemNPC.Distance(Main.npc[OwnerIndex].Center);
                }
                else return MaxLaserLength;
            }
        }

        public override void ExtraBehavior()
        {
            float laserSpeed = 0.01f;
            RotationalSpeed = Projectile.ai[2] == 0 ? -laserSpeed : laserSpeed;
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

            PrimitiveRenderer.RenderTrail(points, new((float f) => 90, (float f) => Color.Cyan, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);
            PrimitiveRenderer.RenderTrail(points, new((float f) => 40, (float f) => Color.White, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), pointAmt + 1);

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }
    }
}
