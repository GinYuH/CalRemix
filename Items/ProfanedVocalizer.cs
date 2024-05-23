using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class ProfanedVocalizer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Profaned Vocalizer");
            Tooltip.SetDefault("Converts Primordial Albatrossan into an understandable language while in your inventory" +
                                "\nUsing this makes Primordial Albatross sounds");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted && Main.netMode != NetmodeID.MultiplayerClient)
				SoundEngine.PlaySound(Yharon.RoarSound with { Pitch = 1.0f }, player.Center);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Megaphone).
                AddIngredient<DivineGeode>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
