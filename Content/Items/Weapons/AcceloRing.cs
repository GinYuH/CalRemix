using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class AcceloRing : ModItem
    {
        public static SoundStyle AcceloRingSpikeSound = new SoundStyle("CalRemix/Assets/Sounds/AcceloRingSpike");
        public static SoundStyle AcceloRingUseSound = new SoundStyle("CalRemix/Assets/Sounds/AcceloRingUse");
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 74;
            Item.damage = 127;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.UseSound = AcceloRingSpikeSound with { Volume = 0.1f };
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AcceloRingProjectile>();
            Item.shootSpeed = 14f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = Main.MouseWorld;
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                int dist = Main.rand.Next(60, 100);
                Vector2 spawnPos = pos + Main.rand.NextVector2CircularEdge(dist, dist);
                float ornament = 0;
                if (Main.rand.NextBool(20))
                {
                    ornament = Main.rand.Next(1, 4);
                }
                Projectile.NewProjectile(source, spawnPos, spawnPos.DirectionTo(pos) * 2, type, damage, knockback, Main.myPlayer, ai1: ornament);
            }
            return false;
        }
    }
}
