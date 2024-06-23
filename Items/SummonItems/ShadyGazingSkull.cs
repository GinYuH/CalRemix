using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.UI.DraedonSummoning;
using CalamityMod.World;
using Microsoft.Xna.Framework;

namespace CalRemix.Items.SummonItems
{

    public class ShadyGazingSkull : ModItem
    {
        public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<Draedon>());
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shady Gazing Skull");
            Tooltip.SetDefault("Summons an unknown Exo Mech");

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

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                CalamityWorld.DraedonSummonCountdown = 260;
                CalamityWorld.DraedonSummonPosition = player.Center + new Vector2(0f, -100f);
                SoundEngine.PlaySound(in CodebreakerUI.SummonSound, CalamityWorld.DraedonSummonPosition);
            }
            return true;
        }
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