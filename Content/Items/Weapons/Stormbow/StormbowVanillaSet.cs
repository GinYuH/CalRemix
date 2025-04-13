using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
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
}
