using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class FractalCrawler : ModProjectile
    {
        public ref float Jump => ref Projectile.ai[0];
        public ref float Flying => ref Projectile.ai[1];
        public ref float Attack => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        public NPC Target;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 38;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void AI()
        {
            Projectile.CheckMinionCondition(ModContent.BuffType<FractalCrawlerBuff>(), Owner.Remix().fractalCrawler);
            NPC target = Projectile.Center.MinionHoming(560f, Owner, true);
            Jump++;
            if (target != null)
            {
                if (target.CanBeChasedBy())
                {
                    Target = target;
                    Flying = 0;
                    Attack++;
                    if (Attack >= 20 && Projectile.Center.Distance(target.Center) <= 240)
                    {
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.Center.DirectionTo(target.Center) * 12f, ProjectileID.CrystalShard, Projectile.damage, Projectile.knockBack);
                        p.DamageType = DamageClass.Summon;
                        Attack = 0;
                    }
                    if (Math.Abs(target.position.X - Projectile.position.X) > 240)
                    {
                        Projectile.velocity.X = ((target.Center - Projectile.Center) / 72).X;
                    }
                }
            }
            else
            {
                Target = null;
                if (Projectile.Center.Distance(Owner.Center) > 800 && Flying != 1)
                    Flying = 1;
                else if (Projectile.Center.Distance(Owner.Center) <= 300 && Flying == 1)
                    Flying = 0;
            }
            if (Flying == 1)
            {
                Projectile.velocity = ((Owner.Center - Projectile.Center) / 72);
                Projectile.tileCollide = false;
            }
            else
            {
                Gravity();
                Projectile.tileCollide = true;
            }
            Projectile.direction = (Projectile.velocity.X < 0) ? -1 : 1;
            Projectile.spriteDirection = -Projectile.direction;
        }

        public void Gravity()
        {
            if (Projectile.velocity.Y < 9.8 && Flying == 0)
                Projectile.velocity.Y += 0.25f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Flying == 0)
            {
                Projectile.velocity.X *= 0.9f;
                if (Target != null)
                {
                    if (Jump >= 60 && Projectile.Top.Y > Target.Bottom.Y)
                    {
                        Projectile.velocity.Y = Main.rand.NextFloat(-8f, -10f);
                        Jump = 0;
                    }
                }
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (Flying == 0)
                fallThrough = Projectile.Bottom.Y < Owner.Top.Y;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffect = (Projectile.spriteDirection > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, (Flying == 1) ? 50 : lightColor.A), Projectile.rotation, texture.Size() / 2f, Projectile.scale, spriteEffect, 0f);
            return false;
        }
    }
}
