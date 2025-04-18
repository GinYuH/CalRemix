using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.Items.Weapons.Melee;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class TotalityTidesDisc : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/TotalityTides";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Totality Tides");
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Timer = 1f;
        }
        public override void AI()
        {
            if (Owner.channel)
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            Projectile.Center = Main.MouseWorld;
            Projectile.rotation += Owner.direction * 0.2f;

            ExoSpawn(Projectile.Center.DirectionTo(Owner.Center) * -1);
            Timer++;
            if (Timer >= 300)
                Timer = 0;
        }

        private void ExoSpawn(Vector2 vector)
        {
            if (Timer % 15 == 0)
            {
                SoundEngine.PlaySound(Exoblade.SwingSound);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vector * 10f, ModContent.ProjectileType<Exobeam>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
            }
            if (Timer % 30 == 0)
            {
                Projectile arrow = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vector * 10f, ModContent.ProjectileType<ExoCrystalArrow>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                arrow.DamageType = DamageClass.MeleeNoSpeed;
                arrow.ai[0] = 1f;
            }
            if (Timer % 150 == 0)
            {
                Projectile vortex = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vector * 5f, ModContent.ProjectileType<ExoVortex>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                vortex.DamageType = DamageClass.MeleeNoSpeed;
                vortex.scale = 2f;
            }
            if (Timer % 300 == 0 && Owner.ownedProjectileCounts[ModContent.ProjectileType<TotalityEnergy>()] < 1)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vector * 10f, ModContent.ProjectileType<TotalityEnergy>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            }
            if (Timer % 60 == 0)
            {
               Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vector * 10f, ModContent.ProjectileType<TotalityScythe>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            }

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


