using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class TenebrisTidesDisc : ModProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/TenebrisTides";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tenebris Tides");
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 23;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.scale = 1.2f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
            for (int i = 0; i < 3; i++)
            {
                int projType = (Main.rand.NextBool() ? ModContent.ProjectileType<TenebreusTidesWaterSword>() : ModContent.ProjectileType<TenebreusTidesWaterSpear>());
                if (Projectile.owner == Main.myPlayer)
                {
                    CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, target.Center, Main.rand.NextBool(), 1000f, 1400f, 80f, 900f, Main.rand.NextFloat(25f, 35f), projType, Projectile.damage / 3, Projectile.knockBack * 0.5f, Projectile.owner);
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
            if (Timer >= 20)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y + 1000f), new Vector2(0f, -16f).RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<TenebreusTidesWaterSpear>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                Timer = 0;
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


