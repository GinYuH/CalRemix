using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.SummonItems
{
    public class ExtremelyStrangePuppet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
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
                //CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<TemporalAbnormality>());
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StrangePuppet>())
                .AddIngredient(ModContent.ItemType<AbnormalEye>())
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}