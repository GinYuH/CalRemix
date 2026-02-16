using System.Collections.Generic;
using Terraria.ID;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class WoodenStormbow : StormbowAbstract
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
    public class BorealWoodStormbow : StormbowAbstract
    {
        public override int damage => 3;
        public override int useTime => 38;
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
    public class PalmWoodStormbow : StormbowAbstract
    {
        public override int damage => 3;
        public override int useTime => 38;
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
    public class RichMahoganyStormbow : StormbowAbstract
    {
        public override int damage => 3;
        public override int useTime => 38;
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
    public class EbonWoodStormbow : StormbowAbstract
    {
        public override int damage => 5;
        public override int useTime => 37;
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
    public class ShadewoodStormbow : StormbowAbstract
    {
        public override int damage => 5;
        public override int useTime => 37;
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
    public class AshWoodStormbow : StormbowAbstract
    {
        public override int damage => 6;
        public override int useTime => 36;
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
    public class PearlwoodStormbow : StormbowAbstract
    {
        public override int damage => 1;
        public override int useTime => 41;
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
