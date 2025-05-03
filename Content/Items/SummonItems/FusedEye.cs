using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Bosses.Poly;

namespace CalRemix.Content.Items.SummonItems
{

    public class FusedEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fused Eye");
            /* Tooltip.SetDefault("Summons Polyphemalus\n" +
                "Enrages during the day"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            // If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss
            return !NPC.AnyNPCs(ModContent.NPCType<Polyphemalus>()) && !Main.dayTime;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<Polyphemalus>());
            }

            return true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.BlackLens)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}