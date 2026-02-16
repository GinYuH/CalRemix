using CalRemix.Content.Projectiles.Hostile.RajahProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class BaneOfTheBunny : ModItem
	{
		public override void SetStaticDefaults()
		{
            //Tooltip.SetDefault(@"");
		}

		public override void SetDefaults()
		{
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee;
            Item.width = 92; 
            Item.height = 92;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.useTime = 20;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.shoot = ModContent.ProjectileType<BaneS>();
            Item.shootSpeed = 4f;
            Item.rare = 8;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 15;
                Item.useAnimation = 15;
                Item.UseSound = SoundID.Item1;
                Item.useStyle = 5;
                Item.shoot = ModContent.ProjectileType<BaneS>();
                Item.shootSpeed = 10f;
                Item.autoReuse = true;
            }
            else
            {
                Item.useAnimation = 13;
                Item.useTime = 13;
                Item.UseSound = SoundID.Item1;
                Item.useStyle = 1;
                Item.shoot = ModContent.ProjectileType<BaneT>();
                Item.shootSpeed = 10f;
                Item.autoReuse = true;
            }
            return base.CanUseItem(player);
        }
    }
}