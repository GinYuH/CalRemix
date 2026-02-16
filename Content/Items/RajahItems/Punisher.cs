using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class Punisher : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Punisher");
        }

        public override void SetDefaults()
        {
            Item.useStyle = 5;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.autoReuse = true;
            Item.knockBack = 6.5f;
            Item.width = 30;
            Item.height = 10;
            Item.damage = 90;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hostile.RajahProjectiles.Punisher>();
            Item.shootSpeed = 15f;
            Item.UseSound = SoundID.Item1;
            Item.rare = 8;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noUseGraphic = true;
        }
    }
}