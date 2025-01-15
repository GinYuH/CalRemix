using CalamityMod.Rarities;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
    public class RadGlowstick : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Glowstick);
            Item.damage = 23;
            Item.shoot = ModContent.ProjectileType<RadGlowstickProjectile>();
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
    }
}
