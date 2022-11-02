using CalamityMod.Rarities;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
	public class NoxusSprayer : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
			DisplayName.SetDefault("Noxus Sprayer");
			Tooltip.SetDefault("Shoots a stream of chaos mist that vaporizes anyone that touches it\n" + "'Kills 99.99% of lesser beings'");
		}
		public override void SetDefaults() 
		{
			Item.width = 62;
			Item.height = 32;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.value = Item.sellPrice(0,0,0,10);
            Item.useTime = 6;
			Item.useAnimation = 6;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<ChaosMist>();
			Item.shootSpeed = 40f;
		}
        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 1.5f);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
			{
				position += muzzleOffset;
			}
		}
	}
}
