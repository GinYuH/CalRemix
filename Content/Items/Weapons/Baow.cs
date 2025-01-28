using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria.Audio;

namespace CalRemix.Content.Items.Weapons
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
                Projectile projectile = Projectile.NewProjectileDirect(source, pos, v, type, damage, knockback, player.whoAmI);

                SoundEngine.PlaySound(SoundID.Item9, player.Center);
                for (int i = 0; i < 8; i++)
                {
                    Vector2 spinPoint = Vector2.UnitX * 0f;
                    spinPoint += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 0.25f)) * new Vector2(1f, 4f);
                    spinPoint = spinPoint.RotatedBy(projectile.velocity.ToRotation());
                    Vector2 pos2 = pos + spinPoint;
                    Vector2 speed = velocity * 0f + spinPoint.SafeNormalize(Vector2.UnitY) * 1f;
                    Dust dust = Dust.NewDustDirect(pos2, 0, 0, DustID.DungeonSpirit, speed.X, speed.Y);
                    dust.scale = 1.5f;
                    dust.noGravity = true;
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
                AddIngredient(ItemID.DarkShard).
                AddIngredient(ItemID.LightShard).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
