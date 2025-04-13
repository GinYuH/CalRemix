using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class WoodenStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Wood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class BorealWoodStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BorealWood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class PalmWoodStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PalmWood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class RichMahoganyStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RichMahogany, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class EbonWoodStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ebonwood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class ShadewoodStormbow : AbstractStormbowClass
    {
        public override int damage => 2;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Shadewood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class AshWoodStormbow : AbstractStormbowClass
    {
        public override int damage => 3;
        public override int useTime => 40;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.FireArrow };
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AshWood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class PearlwoodStormbow : AbstractStormbowClass
    {
        public override int damage => 1;
        public override int useTime => 40;
        public override int arrowAmount => 2;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Pearlwood, 300).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
