using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class AuricSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Auric Sword");
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.knockBack = 6;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(100);
            Item.rare = ItemRarityID.Orange;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation = (Vector2)player.HandPosition;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<AuricSoul>(), 46).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}