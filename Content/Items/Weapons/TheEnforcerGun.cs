using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Ranged;

namespace CalRemix.Content.Items.Weapons;

public class TheEnforcerGun : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("The Enforcer");
        // Tooltip.SetDefault("Has a chance to cause Bleeding");
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ModContent.ItemType<CrackshotColt>());
        Item.damage = 4;
    }
}