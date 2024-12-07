using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
	public class TheCodseeker : ModItem
	{
		public override string Texture => "Terraria/Images/Item_2289";
        private static Texture2D TrueTexture => ModContent.Request<Texture2D>("CalRemix/Content/Items/Tools/TheCodseeker").Value;
        public override void SetStaticDefaults() 
		{
			ItemID.Sets.CanFishInLava[Item.type] = true;
        }
		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.WoodFishingPole);
			Item.fishingPole = 95;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<TyrantBobber>();
        }
		public override bool CanUseItem(Player player) => player.HeldItem == Item;
        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            lineOriginOffset = new Vector2(43, -30);
            lineColor = new Color(193, 78, 78);
        }
        public override void HoldItem(Player player) 
		{
			player.accFishingLine = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TrueTexture, position, new Rectangle(0, 0, TrueTexture.Width, TrueTexture.Height), drawColor, 0, origin, scale, SpriteEffects.None,0);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodFishingPole).
                AddIngredient<TyrantShield>().
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}