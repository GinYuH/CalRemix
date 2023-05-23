using CalamityMod.Items;
using CalRemix.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class RockStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Metamorphic Stone");
            Tooltip.SetDefault("Summons an earth elemental to fight for you");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.earthele = true;
            int brimmy = ProjectileType<EarthElementalMinion>();

            var source = player.GetSource_Accessory(Item);

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 126;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }
        }
    }
}
