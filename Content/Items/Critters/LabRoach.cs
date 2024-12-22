using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRemix.Content.Items.Critters
{
    public class LabRoach : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lab Roach");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = 9999;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.value = Terraria.Item.sellPrice(copper: 6);
            Item.bait = 90;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.LabRoach>();
        }

        public override void AddRecipes()
        {
            foreach (var i in ContentSamples.ItemsByType)
            {
                if (i.Value == null)
                    continue;
                if (i.Value.makeNPC > 0 && i.Value.type != Type)
                {
                    CreateRecipe().
                        AddIngredient(i.Value.type).
                        AddIngredient(ModContent.ItemType<DubiousPlating>(), 20).
                        AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20).
                        AddTile(ModContent.TileType<AgedLaboratoryContainmentBox>()).
                        DisableDecraft().
                        Register();
                }
            }
        }
    }
}