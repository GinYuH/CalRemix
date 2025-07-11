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
    public class MonorianGem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemBow;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 47;
            Item.knockBack = 3f;
            Item.mana = 12;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MonorianGemProj>();
            Item.shootSpeed = 40f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<MonorianGemShards>(), 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
