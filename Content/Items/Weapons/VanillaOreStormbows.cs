using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    public class CopperStormbow : AbstractStormbowClass
    {
        public override int damage => 6;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CopperBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Amethyst, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class TinStormbow : AbstractStormbowClass
    {
        public override int damage => 7;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TinBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Topaz, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class IronStormbow : AbstractStormbowClass
    {
        public override int damage => 5;
        public override int crit => 20;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IronBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class LeadStormbow : AbstractStormbowClass
    {
        public override int damage => 6;
        public override int crit => 22;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LeadBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class SilverStormbow : AbstractStormbowClass
    {
        public override int damage => 8;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SilverBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Sapphire, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class TungstenStormbow : AbstractStormbowClass
    {
        public override int damage => 9;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TungstenBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Emerald, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class GoldStormbow : AbstractStormbowClass
    {
        public override int damage => 10;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GoldBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Ruby, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class PlatinumStormbow : AbstractStormbowClass
    {
        public override int damage => 11;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Diamond, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }

    public class SpacePinball : AbstractStormbowClass
    {
        public override int damage => 34;
        public override int useTime => 12;
        public override SoundStyle useSound => SoundID.Item12;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.GreenLaser };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Blue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.ManaCrystal, 2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class WorldFeeder : AbstractStormbowClass
    {
        public override int damage => 32;
        public override int useTime => 12;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.UnholyArrow };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Blue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DemoniteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class OfVericourse : AbstractStormbowClass
    {
        public override int damage => 46;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.UnholyArrow };
        public override int arrowAmount => 4;
        public override OverallRarity overallRarity => OverallRarity.Blue;
    }
    // the hellstone bow, Fruminous, uses its own logic
    #endregion
    #region HM
    public class CobaltStormbow : AbstractStormbowClass
    {
        public override int damage =>54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<CobaltKunaiProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CobaltBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class PalladiumStormbow : AbstractStormbowClass
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<PalladiumJavelinProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PalladiumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class MythrilStormbow : AbstractStormbowClass
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<MythrilKnifeProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MythrilBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class OrichalcumStormbow : AbstractStormbowClass
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<OrichalcumSpikedGemstoneProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.OrichalcumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class AdamantiteStormbow : AbstractStormbowClass
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AdamantiteThrowingAxeProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TitaniumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class TitaniumStormbow : AbstractStormbowClass
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<TitaniumShurikenProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AdamantiteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }

    public class ExcaliburStormbow : AbstractStormbowClass
    {
        public override int damage => 100;
        public override int useTime => 62;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.HolyArrow };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Pink;
    }
    // chlorophyte? not sure what you're talking about...
    #endregion
}
