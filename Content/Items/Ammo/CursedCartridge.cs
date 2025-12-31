using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Ammo
{
    public class CursedCartridge : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 24));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 46;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 8f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = Item.buyPrice(silver: 1);
            Item.shoot = ModContent.ProjectileType<CursedBullet>();
            Item.shootSpeed = 21f;
            Item.ammo = AmmoID.Bullet;
        }
    }
}
