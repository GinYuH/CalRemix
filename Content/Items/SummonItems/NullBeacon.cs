using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.SummonItems
{
    public class NullBeacon : ModItem
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

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<VoidBoss>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<VoidBoss>());
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NullOrb>())
                .AddIngredient(ModContent.ItemType<NullHolder>())
                .AddIngredient(ModContent.ItemType<NullPedestal>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}