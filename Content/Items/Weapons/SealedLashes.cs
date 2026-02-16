using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class SealedLashes : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<SealedLashesProj>(), 84, 2, 10);
            Item.rare = ItemRarityID.Cyan;
            Item.channel = true;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.maxStack = 1;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.6f, 0.8f), type, damage, knockback, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<RottedTendril>(), 12).
                AddIngredient(ModContent.ItemType<TurnipMesh>(), 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}