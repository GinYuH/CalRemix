using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
    public class TheFirstFracturer : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 66;
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileID.SkyFracture;
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                Vector2 cursor = (player.Center.Distance(Main.MouseWorld) < 160) ? Main.MouseWorld : player.Center + player.DirectionTo(Main.MouseWorld) * 160f;
                Vector2 pos = cursor + new Vector2(Main.rand.NextFloat(100, 120), Main.rand.NextFloat(-40, 40));
                Projectile p = Projectile.NewProjectileDirect(source, pos, pos.DirectionTo(cursor) * velocity.Length(), type, damage / 3, 0);
                p.DamageType = DamageClass.Melee;
                p.timeLeft = 90;
                Vector2 pos2 = cursor + new Vector2(Main.rand.NextFloat(-120, -100), Main.rand.NextFloat(-40, 40));
                Projectile p2 = Projectile.NewProjectileDirect(source, pos2, pos2.DirectionTo(cursor) * velocity.Length(), type, damage / 3, 0);
                p.DamageType = DamageClass.Melee;
                p2.timeLeft = 90;
                Vector2 pos3 = cursor + new Vector2((Main.rand.NextBool()) ? Main.rand.NextFloat(100, 120) : Main.rand.NextFloat(-120, -100), Main.rand.NextFloat(-40, 40));
                Projectile p3 = Projectile.NewProjectileDirect(source, pos3, pos3.DirectionTo(cursor) * velocity.Length(), type, damage / 3, 0);
                p.DamageType = DamageClass.Melee;
                p3.timeLeft = 90;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BladeofGrass).
                AddIngredient(ItemID.LightShard, 2).
                AddIngredient(ItemID.SoulofLight, 22).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
