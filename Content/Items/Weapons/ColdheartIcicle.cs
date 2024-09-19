using CalRemix.Content.Projectiles.Weapons;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria.Audio;

namespace CalRemix.Content.Items.Weapons
{
    public class ColdheartIcicle : ModItem
    {
        public static readonly SoundStyle ColdheartIcicleSwing = new("CalRemix/Assets/Sounds/ColdheartIcicleSwing")
        {
            PitchVariance = 0.2f,
            Pitch = 0.5f,
        };

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.knockBack = 3f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.autoReuse = true;
            Item.UseSound = ColdheartIcicleSwing;

            Item.width = 24;
            Item.height = 24;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 36, 0, 0);

            Item.shoot = ModContent.ProjectileType<ColdheartIcicleProjectile>();
            Item.shootSpeed = 1.6f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WulfrumLeechDagger>(12)
                .AddIngredient<CryonicBar>(20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = 0f;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= 0;
            damage.Flat = 1;
        }
    }
}
