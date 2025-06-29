using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    // aerialite
    public class DoubleHelix : StormbowAbstract
    {
        public override int damage => 46;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<FeatherLarge>() };
        public override int arrowAmount => 3;
        public override OverallRarity overallRarity => OverallRarity.Orange;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // from the sky
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                Vector2 speed = Vector2.Zero;
                speed.Y += shootSpeed;
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    idealLocation.X -= 200;
                    speed.X += 5;
                }
                else
                {
                    idealLocation.X += 200;
                    speed.X -= 5;
                }
                ShootArrowsFromPoint(player, source, idealLocation, speed);
            }
            // from the fround
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.Y = player.Center.Y + 800 + (100 * (i * 0.75f));
                Vector2 speed = Vector2.Zero;
                speed.Y -= shootSpeed;
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    idealLocation.X -= 200;
                    speed.X += 5;
                }
                else
                {
                    idealLocation.X += 200;
                    speed.X -= 5;
                }
                ShootArrowsFromPoint(player, source, idealLocation, speed);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
    #endregion
    #region HM
    // Cryonic
    public class IcarusStormbow : StormbowAbstract
    {
        public override int damage => 30;
        public override int crit => 12;
        public override int useTime => 20;
        public override SoundStyle useSound => SoundID.Item13;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<DaedalusCrystalShot>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Lime;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryonicBar>(30).
                AddIngredient(ItemID.DaedalusStormbow, 1).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Hellforge).
                Register();
        }
    }
    // perennial
    public class PerennialMillenial : StormbowAbstract
    {
        public override int damage => 28;
        public override int crit => 12;
        public override int useTime => 8;
        public override SoundStyle useSound => SoundID.Item13;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.GoldenShowerFriendly };
        public override int arrowAmount => 8;
        public override OverallRarity overallRarity => OverallRarity.Lime;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PerennialBar>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    // Scoria
    public class BoilingSkies : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 12;
        public override SoundStyle useSound => SoundID.Item20;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<ForbiddenSunProjectile>() };
        public override OverallRarity overallRarity => OverallRarity.Lime;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ScoriaBar>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    // astral
    public class BlightSky : StormbowAbstract
    {
        public override int damage => 74;
        public override int crit => 12;
        public override int useTime => 7;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AstralCrystal>() };
        public override int arrowAmount => 6;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StarryBlight>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<AstralBar>(30).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
    #endregion
    #region Post-ML
    // cosmolite
    public class BigEater : DoubleHelix
    {
        public override int damage => 140;
        public override int crit => 16;
        public override int useTime => 12;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<VanquisherArrowProj>() };
        public override int arrowAmount => 8;
        public override OverallRarity overallRarity => OverallRarity.DarkBlue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
    #endregion
}
