using CalamityMod.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    // jungle
    public class Vinewrath : StormbowAbstract
    {
        public override int damage => 22;
        public override int crit => 22;
        public override float shootSpeed => 22;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.BeeHive };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Orange;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.JungleSpores, 22).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Stinger, 16).
                AddIngredient(ItemID.Vine, 7).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    // dungeon
    public class Watercooler : StormbowAbstract
    {
        public override int damage => 42;
        public override int useTime => 20;
        public override SoundStyle useSound => SoundID.Item3;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.WetBomb };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    // nights edge
    public class ThreeOClock : StormbowAbstract
    {
        public override int damage => 46;
        public override int crit => 8;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<ClockArrowForNightsEdgeBow>() };
        public override int arrowAmount => 2;
        public override OverallRarity overallRarity => OverallRarity.Orange;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // from the sky
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                ShootArrowsFromPoint(player, source, idealLocation, new Vector2(0, 15));
            }
            // from the fround
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.Y = player.Center.Y + 800 + (100 * (i * 0.75f));
                ShootArrowsFromPoint(player, source, idealLocation, new Vector2(0, -15));
            }
            // left
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.X -= 1250;
                ShootArrowsFromPoint(player, source, idealLocation, new Vector2(15, 0));
            }
            // right
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.X += 1250;
                ShootArrowsFromPoint(player, source, idealLocation, new Vector2(-15, 0));
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WorldFeeder>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<Watercooler>().
                AddIngredient<Vinewrath>().
                AddIngredient<Fruminous>().
                AddIngredient<PurifiedGel>(10).
                AddTile(TileID.DemonAltar).
                Register();

            CreateRecipe().
                AddIngredient<OfVericourse>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<Watercooler>().
                AddIngredient<Vinewrath>().
                AddIngredient<Fruminous>().
                AddIngredient<PurifiedGel>(10).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
    #endregion
    #region HM
    // true nights edge
    public class FourOClock : ThreeOClock
    {
        public override int damage => 75;
        public override OverallRarity overallRarity => OverallRarity.Yellow;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ThreeOClock>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.SoulofFright, 20).
                AddIngredient(ItemID.SoulofMight, 20).
                AddIngredient(ItemID.SoulofSight, 20).
                AddIngredient(ItemID.SoulofLight, 20).
                AddIngredient(ItemID.SoulofNight, 20).
                AddIngredient(ItemID.SoulofFlight, 20).
                AddIngredient<EssenceofEleum>(20).
                AddIngredient<EssenceofHavoc>(20).
                AddIngredient<EssenceofSunlight>(20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    // true excalivur
    public class TrueExcaliburStormbow : StormbowAbstract
    {
        public override int damage => 100;
        public override int useTime => 62;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.HolyArrow };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Yellow;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int amountOfEvil = Main.rand.Next(2, 30);
            ShootArrowsLikeStormbow(player, source, amountOfEvil, new List<int>{ ProjectileID.LightDisc });
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExcaliburStormbow>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.ChlorophyteBar, 30).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    // forbidden fragment
    public class Legendscribe : StormbowAbstract
    {
        public override int damage => 285;
        public override int useTime => 100;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<SandstoneProjectile>() };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Yellow;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.SandElementalBanner, 1).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    #endregion

    public class ClockArrowForNightsEdgeBow : ModProjectile
    {
        public override string Texture => "CalamityMod/Items/Ammo/SproutingArrow";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 150;

        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 1.1f;
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 0, 255, 127);
    }
}
