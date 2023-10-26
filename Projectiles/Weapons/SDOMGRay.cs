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

namespace CalRemix.Projectiles.Weapons
{
    public class SDOMGRay : ModProjectile
    {
        private const float MAX_CHARGE = 50f;
        private const float MOVE_DISTANCE = 104;

        public ref float Distance => ref Projectile.ai[0];
        public ref float Timer => ref Projectile.ai[1];
        public ref float Charge => ref Projectile.localAI[0];
        public ref float Sound => ref Projectile.localAI[1];
        public Player Owner => Main.player[Projectile.owner];
        public bool IsAtMaxCharge => Charge == MAX_CHARGE;

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
            if (IsAtMaxCharge)
                DrawLaser(Main.spriteBatch, TextureAssets.Projectile[Type].Value, Main.player[Projectile.owner].Center, Projectile.velocity, 10, -1.57f, 4f, (int)MOVE_DISTANCE);
            return false;
        }
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step,  float rotation = 0f, float scale = 1f, int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                var origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsAtMaxCharge) return false;
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.Center, Owner.Center + unit * Distance, 88, ref point);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
        }
        public override void AI()
        {
            if (Owner is null) return;
            Timer++;
            if (Timer % 300 == 0)
            {
                if (Owner.HasItem(ModContent.ItemType<PlasmaGrenade>()))
                {
                    SoundEngine.PlaySound(SoundID.Item96, Owner.Center); 
                    Owner.ConsumeItem(ModContent.ItemType<PlasmaGrenade>(), true);

                    for (int k = 0; k < 10; k++)
                    {
                        Dust dust = Dust.NewDustDirect(Owner.Center, 1, 1, DustID.Electric);
                        dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * (5f + Main.rand.Next(6));
                        dust.noGravity = true;
                        dust.scale = Main.rand.Next(10, 20) * 0.05f;
                    }
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath56, Owner.Center);
                    Projectile.Kill();
                }
            }
            Projectile.position = Owner.Center + Projectile.velocity * MOVE_DISTANCE;
            Projectile.timeLeft = 2;

            UpdatePlayer(Owner);
            ChargeLaser(Owner);
            if (Charge < MAX_CHARGE) return;
            if (Sound != 1)
            {
                SoundEngine.PlaySound(SoundID.Zombie104, Owner.Center);
                Sound = 1;
            }
            SetLaserPosition(Owner);
            SpawnDusts(Owner);
            CastLights();
            Owner.AddBuff(BuffID.Slow, 10);
            Owner.AddBuff(BuffID.OnFire, 10);
            Owner.AddBuff(BuffID.Oiled, 10);
        }

        private void SpawnDusts(Player player)
        {
            Vector2 dustPos = player.Center + Projectile.velocity * Distance;

            for (int i = 0; i < 2; ++i)
            {
                float num1 = Projectile.velocity.ToRotation() + (Main.rand.NextBool(2)? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Dust.NewDustDirect(dustPos, 0, 0, DustID.Electric, dustVel.X, dustVel.Y);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }
        private void SetLaserPosition(Player player)
        {
            for (Distance = MOVE_DISTANCE; Distance <= 2200f; Distance += 5f)
            {
                var start = player.Center + Projectile.velocity * Distance;
                if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
            }
        }

        private void ChargeLaser(Player player)
        {
            if (!player.Calamity().mouseRight || !player.active || player.dead)
                Projectile.Kill();
            else
            {
                if (Charge < MAX_CHARGE)
                    Charge++;
            }
        }

        private void UpdatePlayer(Player player)
        {
            if (Projectile.owner == Main.myPlayer)
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
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
        }
        public override bool ShouldUpdatePosition() => false;
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}