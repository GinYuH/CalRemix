using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Ammo
{
    public class PlaguedSolution : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plagued Solution");
			// Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Plague");
		}

		public override void SetDefaults()
		{
			Item.shoot = ModContent.ProjectileType<PlaguedSpray>() - 145;
			Item.ammo = AmmoID.Solution;
			Item.width = 12;
			Item.height = 14;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.consumable = true;
		}

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			return player.itemAnimation >= player.HeldItem.useAnimation - 3;
		}
	}
}