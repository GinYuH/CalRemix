using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalamityMod;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Abyss;
using CalamityMod.Items.Placeables.FurnitureAcidwood;

namespace CalRemix.Content.Items.SummonItems
{
    public class PoisonedSclera : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Poisoned Sclera");
            // Tooltip.SetDefault("Summons the Acidsighter when used in the sulphurous sea at night");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<AcidEye>()) && player.Calamity().ZoneSulphur && !Main.dayTime;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                CalRemixHelper.SpawnNPCOnPlayer(player.whoAmI, ModContent.NPCType<AcidEye>());
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Acidwood>(20)
                .AddIngredient<SulphurousSandstone>(15)
                .AddIngredient(ItemID.Seashell)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}