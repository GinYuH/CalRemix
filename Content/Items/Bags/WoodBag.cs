using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;
using CalamityMod.Items.Placeables;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.Bags
{
    public class WoodBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 50);
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemID.Wood, 1, 16, 24);
            itemLoot.AddIf(() => !WorldGen.crimson || Main.hardMode, ItemID.Ebonwood, 3, 16, 24);
            itemLoot.AddIf(() => WorldGen.crimson || Main.hardMode, ItemID.Shadewood, 3, 16, 24);
            itemLoot.Add(ItemID.PalmWood, 3, 16, 24);
            itemLoot.Add(ItemID.RichMahogany, 3, 16, 24);
            itemLoot.Add(ItemID.BorealWood, 3, 16, 24);
            itemLoot.AddIf(() => NPC.downedBoss2, ItemID.AshWood, 3, 16, 24);
            itemLoot.AddIf(() => Main.hardMode, ItemID.Pearlwood, 3, 16, 24);
            itemLoot.AddIf(() => Main.hardMode, ModContent.ItemType<AstralMonolith>(), 3, 16, 24);
            itemLoot.AddIf(() => RemixDowned.downedAcidsighter, ModContent.ItemType<Acidwood>(), 5, 16, 24);
            itemLoot.AddIf(() => NPC.downedGoblins, ItemID.DynastyWood, 5, 16, 24);
            itemLoot.AddIf(() => NPC.downedHalloweenTree, ItemID.SpookyWood, 10, 16, 24);
        }
    }
}
