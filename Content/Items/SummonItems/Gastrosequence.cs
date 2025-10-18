using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.Items.SummonItems
{
    public class Gastrosequence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<MonorianGastropodAscended>()) && player.InModBiome<VoidForestBiome>();
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<MonorianGastropodAscended>());
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Gastrogel>(10)
                .AddIngredient<GildedShard>(5)
                .AddIngredient<LightResidue>(50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}