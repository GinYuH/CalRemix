using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class ClamorNoxus : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 74;
            Item.damage = 270;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstralStarMagic>();
            Item.shootSpeed = 21f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float shootSpeed = Item.shootSpeed;
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float x = (float)Main.mouseX + Main.screenPosition.X - pos.X;
            float y = (float)Main.mouseY + Main.screenPosition.Y - pos.Y;
            if (player.gravDir == -1f)
            {
                y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - pos.Y;
            }
            float magnitude = (float)Math.Sqrt(x * x + y * y);
            if ((float.IsNaN(x) && float.IsNaN(y)) || (x == 0f && y == 0f))
            {
                x = player.direction;
                y = 0f;
            }
            bool noxling = false;
            for (int i = 0; i < 5; i++)
            {
                float kbMultiplier = 1f;
                if (i <= 1)
                {
                    type = ModContent.ProjectileType<AstralStarMagic>();
                }
                else if (Main.rand.NextBool(10) && !noxling && player.ownedProjectileCounts[ModContent.ProjectileType<Noxling>()] < 1)
                {
                    type = ModContent.ProjectileType<Noxling>();
                    noxling = true;
                }
                else
                {
                    type = Main.rand.Next(new int[] { ModContent.ProjectileType<AlphaDraconisStar>(), ProjectileID.HallowStar });
                }
                if (type == ModContent.ProjectileType<Noxling>())
                {
                    kbMultiplier = 0f;
                }
                pos = new Vector2(player.Center.X + (float)player.width * 0.5f + (float)Main.rand.Next(201) * (0f - (float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.Center.X), player.MountedCenter.Y - 600f);
                pos.X = (pos.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                pos.Y -= 100 * i;
                x = (float)Main.mouseX + Main.screenPosition.X - pos.X;
                y = (float)Main.mouseY + Main.screenPosition.Y - pos.Y;
                if (y < 0f)
                {
                    y *= -1f;
                }

                if (y < 20f)
                {
                    y = 20f;
                }
                magnitude = (float)Math.Sqrt(x * x + y * y);
                magnitude = shootSpeed / magnitude;
                x *= magnitude;
                y *= magnitude;
                Vector2 v = new Vector2(x + (float)Main.rand.Next(-30, 31) * 0.02f, y + (float)Main.rand.Next(-30, 31) * 0.02f);
                Projectile proj = Projectile.NewProjectileDirect(source, pos, v, type, damage, (int)(knockback * kbMultiplier), player.whoAmI);
                proj.DamageType = Item.DamageType;
                proj.tileCollide = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AlphaDraconis>().
                AddIngredient<Starmada>().
                AddIngredient<ExoPrism>(10).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
