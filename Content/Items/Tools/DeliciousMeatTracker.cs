using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Food;

namespace CalRemix.Content.Items.Tools
{
    public class DeliciousMeatTracker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 12;
            Item.useAnimation = 12;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.useTurn = true;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().deliciousMeatNoLife)
                return false;
            if (player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
                ConsumeDeliciousMeat(player, player.altFunctionUse);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddIngredient<DeliciousMeat>(2).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
        public static void ConsumeDeliciousMeat(Player player, int type)
        {
            CalRemixPlayer p = player.GetModPlayer<CalRemixPlayer>();
            if (type == 2)
            {
                while (player.ConsumeItem(ModContent.ItemType<DeliciousMeat>()) && !p.deliciousMeatNoLife)
                {
                    if (p.deliciousMeatPrestige > 99 && p.deliciousMeatRedeemed > 1000000)
                        p.deliciousMeatNoLife = true;
                    else if (p.deliciousMeatPrestige > 99 && p.deliciousMeatRedeemed > 1000000)
                    {
                        p.deliciousMeatPrestige++;
                        p.deliciousMeatRedeemed = 0;
                    }
                    else
                        p.deliciousMeatRedeemed++;
                }
            }
            else
            {
                if (player.ConsumeItem(ModContent.ItemType<DeliciousMeat>()))
                {
                    if (p.deliciousMeatPrestige > 99 && p.deliciousMeatRedeemed > 1000000)
                        p.deliciousMeatNoLife = true;
                    else if (p.deliciousMeatPrestige > 99 && p.deliciousMeatRedeemed > 1000000)
                    {
                        p.deliciousMeatPrestige++;
                        p.deliciousMeatRedeemed = 0;
                    }
                    else
                        p.deliciousMeatRedeemed++;
                    return;
                }
            }
        }
    }
}