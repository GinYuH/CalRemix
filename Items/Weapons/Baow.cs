using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Items.Weapons
{
	public class Baow : ModItem
    {
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Baow");
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 5, silver: 20);
            Item.useTime = 18; 
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 27;
			Item.knockBack = 3f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 14f;
			Item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                Vector2 pos = player.Center + player.DirectionTo(Main.MouseWorld) * (player.Distance(Main.MouseWorld) * 2);
                Vector2 v = -velocity;
                Projectile.NewProjectile(source, pos.X, pos.Y, v.X, v.Y, type, damage, knockback, player.whoAmI);
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(pos, 1, 1, DustID.MagicMirror, Scale: 1.5f + Main.rand.NextFloat());
                    dust.velocity = Vector2.Normalize(v).RotatedByRandom(MathHelper.ToRadians(45f));
                    dust.noGravity = false;
                }
            }
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, -0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IceBow).
                AddIngredient(ItemID.MoltenFury, 2).
                AddIngredient(ItemID.LivingFireBlock, 23).
                AddIngredient(ItemID.IceBlock, 23).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
