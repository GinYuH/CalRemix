using CalamityMod.Items;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using Terraria.ModLoader.IO;
using CalRemix.Content.Items.Materials;
using Terraria.ID;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TwistedNetheriteHelmet : ModItem
    {
        public int souls;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twisted Netherite Helmet");
        }
        public override void SaveData(TagCompound tag)
        {
            tag["TwistedNetheriteSouls"] = souls;
        }
        public override void LoadData(TagCompound tag)
        {
            souls = tag.Get<int>("TwistedNetheriteSouls");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 20;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TwistedNetheriteChestplate>() && legs.type == ModContent.ItemType<TwistedNetheriteLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().twistedNetheriteBoots)
            {
                player.setBonus = "Collect souls from fallen enemies and stores them into the helmet\n" +
                                  "Knockback immunity\n" +
                                  "All attacks inflict wither\n" +
                                  "Using the armor bonus key consumes all souls to unleash a massive explosion\n" +
                                  "Soul explosion and wither get stronger with each soul collected\n" +
                                  $"[c/99ffff:{souls} souls stored]";
                player.GetModPlayer<CalRemixPlayer>().twistedNetherite = true;
                player.noKnockback = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TwistedNetheriteBar>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
