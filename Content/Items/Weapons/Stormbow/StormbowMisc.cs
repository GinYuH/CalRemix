using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
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
    #endregion
    #region HM
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
    public class SB90 : StormbowAbstract
    {
        public override int damage => 5;
        public override int useTime => 1;
        public override SoundStyle useSound => SoundID.Item11;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.BoneArrow };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
    }
    public class StarryBlight : StormbowAbstract
    {
        public override int damage => 47;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.StarCloakStar };
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StarblightSoot>(25).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.FallenStar, 40).
                AddIngredient<AstralClay>(7).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class TheSkyfall : StormbowAbstract
    {
        public override int damage => 10;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.PoisonDart };
        public override OverallRarity overallRarity => OverallRarity.Lime;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.MowTheLawn;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LunarTabletFragment, 2).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class SilverWater : StormbowAbstract
    {
        public override int damage => 36;
        public override int useTime => 8;
        public override SoundStyle useSound => CommonCalamitySounds.LargeWeaponFireSound with { Pitch = 2 };
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<MercuryRocketFriendly>() };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Red;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<MercuryCoatedSubcinium>(), 5).
                AddIngredient(ModContent.ItemType<Mercury>(), 30).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    #endregion
    #region Post-ML

    #endregion
}
