using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Sounds;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace CalRemix.Items.Weapons
{
	public class OnyxGunblade : ModItem
	{
        private int shootCount = 1;
        public override void SetStaticDefaults() 
		{
            SacrificeTotal = 1;
            DisplayName.SetDefault("Onyx Gunblade");
            Tooltip.SetDefault("If you are reading this, there is a solid chance you are using the Calamity Fandom Wiki\n"+"Do not use it, use the official wiki on calamitymod.wiki.gg instead for up to date information regarding calamity\n"+"As for remix, download the recipe browser mod, it should give a good amount of information regarding the addon's content.");
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LeadBroadsword, 2).
                AddIngredient(ItemID.Obsidian, 30).
                AddIngredient(ItemID.MusketBall, 99).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
