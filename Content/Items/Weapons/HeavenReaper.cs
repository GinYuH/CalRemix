using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class HeavenReaper : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 4035;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 78;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 78;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = BetterSoundID.ItemCast;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.shoot = ModContent.ProjectileType<AuricProbe>();
            Item.shootSpeed = 20;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 22;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation = (Vector2)player.HandPosition;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(BetterSoundID.ItemDeathSickle with { Pitch = -1 }, player.Center);
            for (int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(6f, 10f), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}