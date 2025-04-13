using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons.Stormbow;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Alice Bows
    public class Stormbow : StormbowAbstract
    {
        public override int damage => 12;
        public override int crit => 8;
        public override int arrowAmount => 4;
        public override OverallRarity overallRarity => OverallRarity.Blue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Cobweb, 50).
                AddTile(TileID.Loom).
                Register();
        }
    }
    public class GiantStormbow : StormbowAbstract
    {
        public override int damage => 16;
        public override int crit => 8;
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GiantBow).
                AddIngredient<Stormbow>().
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
    #endregion
    public class ElectricEel : StormbowAbstract
    {
        public override int useTime => 8;
        public override SoundStyle useSound => SoundID.Item11;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.Bullet };
        public override int arrowAmount => 3;
        public override OverallRarity overallRarity => OverallRarity.Green;
    }

    public class SoulOfFlightstorm : StormbowAbstract
    {
        public override int damage => 56;
        public override int crit => 12;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.Item13;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.ZapinatorLaser };
        public override int arrowAmount => 6;
        public override OverallRarity overallRarity => OverallRarity.Pink;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SoulofFlight, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
