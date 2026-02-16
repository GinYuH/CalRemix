using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Subworlds.Sealed;

namespace CalRemix.Content.Items.SummonItems
{
    public class StrangePuppet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<SealedPuppet>());
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bass, 20)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}