using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.Items.Weapons
{
    public class AwokenGastropodEye : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemBow;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 59;
            Item.knockBack = 7f;
            Item.mana = 12;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<AwokenGastropodEyeProj>();
            Item.shootSpeed = 40f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<GastropodEye>()).
                AddIngredient(ModContent.ItemType<Gastrogel>(), 20).
                AddIngredient(ModContent.ItemType<GildedShard>(), 5).
                AddIngredient(ModContent.ItemType<MonorianGemShards>(), 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
