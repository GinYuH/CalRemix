using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class LaevateinnProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/Laevateinn";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.scale = 1.6f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);

            SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Projectile.Center);
            // now where could this have come from
            int pointsOnStar = 6;
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < pointsOnStar; i++)
                {
                    float angle = MathHelper.Pi * 1.5f - i * MathHelper.TwoPi / pointsOnStar;
                    float nextAngle = MathHelper.Pi * 1.5f - (i + 3) % pointsOnStar * MathHelper.TwoPi / pointsOnStar;
                    if (k == 1)
                        nextAngle = MathHelper.Pi * 1.5f - (i + 2) * MathHelper.TwoPi / pointsOnStar;
                    Vector2 start = angle.ToRotationVector2();
                    Vector2 end = nextAngle.ToRotationVector2();
                    int pointsOnStarSegment = 4;
                    for (int j = 0; j < pointsOnStarSegment; j++)
                    {
                        Vector2 shootVelocity = Vector2.Lerp(start, end, j / (float)pointsOnStarSegment) * 10;
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(Projectile.damage * 0.1f), 0f, Projectile.owner);
                        Main.projectile[p].DamageType = DamageClass.Melee;
                    }
                }
            }
        }
        public override void AI()
        {
            if (Owner.channel)
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            Timer++;
            if (Timer % 10 == 0)
            {
                SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Projectile.Center);
                float timer = Timer * 0.05f;
                Vector2 spawnPos = Owner.MountedCenter + Vector2.UnitX.RotatedBy(Projectile.rotation) * 120;
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(Projectile.damage * 0.1f), 0f, Projectile.owner, ai0: 50);
                Main.projectile[p].DamageType = DamageClass.Melee;
                Vector2 spawnPos2 = Owner.MountedCenter - Vector2.UnitX.RotatedBy(Projectile.rotation) * 120;
                p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos2, Vector2.Zero, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(Projectile.damage * 0.1f), 0f, Projectile.owner, ai0: 50);
                Main.projectile[p].DamageType = DamageClass.Melee;
            }
            Projectile.Center = Owner.Center;
            Projectile.rotation += Owner.direction * 0.2f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}


