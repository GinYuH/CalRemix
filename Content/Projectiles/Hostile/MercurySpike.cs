using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercurySpike : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
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

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.Center.X;
                Projectile.localAI[1] = Projectile.Center.Y;
                Projectile.netUpdate = true;
            }
            Vector2 start = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            if (Projectile.alpha > 0 && Projectile.ai[1] < 40)
            Projectile.alpha -= 10;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.ai[1]++;
            float startShoot = 90;
            float endShoot = 100;
            if (Projectile.ai[1] == startShoot)
            {
                SoundEngine.PlaySound(Exoblade.SwingSound with { Pitch = 1, MaxInstances = 0, Volume = 2 }, Projectile.Center);
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] > 90 && Projectile.ai[1] < 110)
            {
                Projectile.Center = Vector2.Lerp(start, start + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * (CalamityWorld.death ? 800 : 600), CalamityUtils.ExpInEasing(Utils.GetLerpValue(startShoot, endShoot, Projectile.ai[1], true), 1));
            }
            if (Projectile.ai[1] > 190)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            float easeIn = 5;
            float easeOut = 10;
            float maxWidth = 1.6f;
            float scaleX = MathHelper.Lerp(0, maxWidth, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, easeIn, Projectile.ai[1], true), 1));
            if (Projectile.ai[1] >= easeIn)
            {
                scaleX = MathHelper.Lerp(maxWidth, 1f, CalamityUtils.SineOutEasing(Utils.GetLerpValue(easeIn, easeOut, Projectile.ai[1], true), 1));
            }

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, new Vector2(tex.Width / 2f, 16), new Vector2(scaleX, 1) * Projectile.scale, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float nothing = 0;
            // 1070 is the height of the spike
            // 44 is width of the spike
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * Projectile.scale * /*Projectile.ai[1] **/ 1070, 22f, ref nothing);
        }

    }
}