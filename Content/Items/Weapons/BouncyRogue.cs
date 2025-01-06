using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    [Autoload(false)]
    public class BouncyRogue : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "BouncyRogue";

        public int itemType;
        protected override bool CloneNewInstances => true;

        public override string Name => ItemLoader.GetItem(itemType).Mod.Name + "/" + ItemLoader.GetItem(itemType).Name + "Bouncy";
        public override string Texture => ItemLoader.GetItem(itemType).Texture;

        public BouncyRogue(int type)
        {
            itemType = type;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = ItemLoader.GetItem(itemType).Item.ResearchUnlockCount;
            try
            {
                DisplayName.SetDefault("Bouncy " + ItemLoader.GetItem(itemType).DisplayName.Value);
                Tooltip.SetDefault(ItemLoader.GetItem(itemType).Tooltip.Value + "\nNow with extra bounciness!");
            }
            catch
            {

            }
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(itemType);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => ItemLoader.GetItem(itemType).Shoot(player, source, position, velocity, type, damage, knockback);

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[itemType].Value, position, frame, Color.Magenta, 0, origin, scale, SpriteEffects.None, 0f);

            return false;
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(TextureAssets.Item[itemType].Value, Item.position - Main.screenPosition, null, Color.Magenta, 0, TextureAssets.Item[itemType].Size() / 2, scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void AddRecipes()
        {
            if (Item.maxStack == 1)
                Recipe.Create(Type).AddIngredient(itemType).AddIngredient(ItemID.PinkGel).AddTile(TileID.WorkBenches).Register();
            else
                Recipe.Create(Type, 100).AddIngredient(itemType, 100).AddIngredient(ItemID.PinkGel).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class BouncyProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool Bouncy = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse usesource && !string.IsNullOrWhiteSpace(usesource.Item.Name))
            {
                if (usesource.Item.ModItem != null)
                {
                    if (usesource.Item.ModItem is BouncyRogue)
                    {
                        Bouncy = true;
                        projectile.tileCollide = true;
                    }
                }
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile p2)
            {
                if (p2.GetGlobalProjectile<BouncyProj>().Bouncy)
                {
                    Bouncy = true;
                    projectile.tileCollide = true;
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.TryGetGlobalProjectile(out BouncyProj p))
            {
                if (p.Bouncy)
                {
                    if (projectile.velocity.X != oldVelocity.X)
                    {
                        projectile.velocity.X = -oldVelocity.X * 1.001f;
                    }
                    if (projectile.velocity.Y != oldVelocity.Y)
                    {
                        projectile.velocity.Y = -oldVelocity.Y * 1.001f;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
