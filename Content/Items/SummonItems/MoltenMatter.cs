using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalamityMod;
using CalamityMod.Items.Placeables;
using CalamityMod.Rarities;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.Items.Placeables.Plates.Molten;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.SummonItems
{
    public class MoltenMatter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Matter");
            Tooltip.SetDefault("Summons Pyrogen when used in the Underworld");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Pyrogen>()) && player.ZoneUnderworldHeight;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<Pyrogen>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MoltenNavyplate>(20)
                .AddIngredient<MoltenCinderplate>(20)
                .AddIngredient<MoltenHavocplate>(20)
                .AddIngredient<MoltenPlagueplate>(20)
                .AddIngredient<MoltenElumplate>(20)
                .AddIngredient<MoltenOnyxplate>(20)
                .AddTile(ModContent.TileType<AncientConsole>())
                .Register();
        }
    }
}