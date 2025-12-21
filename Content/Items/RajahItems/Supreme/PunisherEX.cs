using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems.Supreme
{
    public class PunisherEX : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Avenger");
            //Tooltip.SetDefault(@"The Punisher EX");
        }

        public override void SetDefaults()
        {
            Item.useStyle = 5;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.autoReuse = true;
            Item.knockBack = 7f;
            Item.width = 30;
            Item.height = 10;
            Item.damage = 500;
            Item.shoot = Terraria.ModLoader.ModContent.ProjectileType<Projectiles.Hostile.RajahProjectiles.Supreme.PunisherEX>();
            Item.shootSpeed = 15f;
            Item.UseSound = SoundID.Item1;
            Item.rare = 9;
            Item.expert = true; Item.expertOnly = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noUseGraphic = true;
        }
    }
}