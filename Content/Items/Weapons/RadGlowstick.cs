using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Materials;

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
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).
                AddIngredient<SulphuricScale>(7).
                AddIngredient(ItemID.Glowstick, 100).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
