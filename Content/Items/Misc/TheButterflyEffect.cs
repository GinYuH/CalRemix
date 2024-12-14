using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class TheButterflyEffect : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.value = 0;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.FunnyButterfly>();
        }
        public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<FunnyButterfly>());
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Morpho>().
                AddIngredient<TheDreamingGhost>().
                AddIngredient<Rock>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}