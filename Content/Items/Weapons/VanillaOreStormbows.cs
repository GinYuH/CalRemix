using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class CopperStormbow : AbstractStormbowClass
    {
        public override int damage => 6;
        public override int crit => 4;
        public override float shootSpeed => 12;
        public override int useTime => 32;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.WoodenArrowFriendly };
        public override int arrowAmount => 3;
        public override OverallRarity overallRarity => OverallRarity.White;
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
}
