using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class YellowLightHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 5f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<YellowLightChestplate>() && (legs.type == ModContent.ItemType<YellowLightLeggings>() || legs.type == ModContent.ItemType<EnchantedYellowLightLeggings>());
        }

        public override void UpdateArmorSet(Player player)
        {
            string ret = "Multiplies the effectiveness of dye stats by 5";
            if (!CalRemixWorld.dyeStats)
            {
                ret += "\nDye stats are currently disabled by Anomaly 109, which means this bonus is currently useless";
            }
            player.setBonus = ret;
            player.Remix().lightArmor = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LightResidue>(13).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class YellowLightChestplate : ModItem
    {
        public override void SetDefaults()
        {
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            Item.width = 30;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LightResidue>(15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class YellowLightLeggings : ModItem
    {
        public override void SetDefaults()
        {
            ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LightResidue>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class EnchantedYellowLightLeggings : ModItem
    {
        public override string Texture => "CalRemix/Content/Items/Armor/YellowLightLeggings";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<YellowLightLeggings>();
        }
        public override void SetDefaults()
        {
            ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
        }
    }
}
