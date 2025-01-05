using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    [Autoload(false)]
    public class StickyRogue : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "StickyRogue";

        public int itemType;
        protected override bool CloneNewInstances => true;

        public override string Name => ItemLoader.GetItem(itemType).Mod.Name + "/" + ItemLoader.GetItem(itemType).Name + "Sticky";
        public override string Texture => ItemLoader.GetItem(itemType).Texture;

        public StickyRogue(int type)
        {
            itemType = type;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = ItemLoader.GetItem(itemType).Item.ResearchUnlockCount;
            try
            {
                DisplayName.SetDefault("Sticky " + ItemLoader.GetItem(itemType).DisplayName.Value);
                Tooltip.SetDefault(ItemLoader.GetItem(itemType).Tooltip.Value + "\nNow with extra stickiness!");
            }
            catch
            {

            }
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(itemType);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[itemType].Value, position, frame, Color.Cyan, 0, origin, scale, SpriteEffects.None, 0f);
            
            return false;
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(TextureAssets.Item[itemType].Value, Item.position - Main.screenPosition, null, Color.Cyan, 0, TextureAssets.Item[itemType].Size() / 2, scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void AddRecipes()
        {
            if (Item.maxStack == 1)
                Recipe.Create(Type).AddIngredient(itemType).AddIngredient(ItemID.Gel, 100).AddTile(TileID.WorkBenches).Register();
            else
                Recipe.Create(Type, 100).AddIngredient(itemType, 100).AddIngredient(ItemID.Gel).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class StickyProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool sticky = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse usesource && !string.IsNullOrWhiteSpace(usesource.Item.Name))
            {
                if (usesource.Item.ModItem != null)
                {
                    sticky = usesource.Item.ModItem is StickyRogue;
                    projectile.tileCollide = true;
                }
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile p2)
            {
                if (p2.GetGlobalProjectile<StickyProj>().sticky)
                {
                    sticky = true;
                    projectile.tileCollide = true;
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.TryGetGlobalProjectile(out StickyProj p))
            {
                if (p.sticky)
                {
                    projectile.velocity = Vector2.Zero;
                    return false;
                }
            }
            return true;
        }
    }
}
