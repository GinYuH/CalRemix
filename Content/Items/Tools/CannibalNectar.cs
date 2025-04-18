using CalRemix.Content.NPCs.PandemicPanic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class CannibalNectar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cannibal Nectar");
            // Tooltip.SetDefault("Stops the Pandemic Panic\n'Smells like strawberry'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return PandemicPanic.IsActive;
        }
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Shatter, player.position);
            PandemicPanic.EndEvent();
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Glass, 20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
