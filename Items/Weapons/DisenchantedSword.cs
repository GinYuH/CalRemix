using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.NPCs;
using Terraria.Audio;
using CalamityMod;

namespace CalRemix.Items.Weapons
{
    public class DisenchantedSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disenchanted Sword");
            Tooltip.SetDefault("Can be enchanted by defeating an advanced evil foe...");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.EnchantedSword);
            Item.shoot = ProjectileID.None;
        }

        public override void HoldItem(Player player)
        {
            Enchant();
        }

        public override void UpdateInventory(Player player)
        {
            Enchant();
        }

        public void Enchant()
        {
            if (DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator)
            {
                int modType = Item.prefix;
                Item.SetDefaults(ItemID.EnchantedSword);
                Item.prefix = modType;
            }
        }
    }
}
