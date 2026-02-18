using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Content.Items.Weapons
{
    public class GuardiansWrath : ModItem
    {
        public static SoundStyle ThrowSound = new SoundStyle("CalRemix/Assets/Sounds/GuardiansWrathThrow", 2);
        public static SoundStyle HitSound = new SoundStyle("CalRemix/Assets/Sounds/GuardiansWrathHit", 2);
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = Item.sellPrice(gold: 20);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 3460;
            Item.knockBack = 10;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<GuardiansWrathStab>();
            Item.shootSpeed = 10;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<GuardiansWrathProj>()] > 0)
                return false;
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<GuardiansWrathProj>()] < 1)
                {
                    SoundEngine.PlaySound(ThrowSound, player.Center);
                    Projectile.NewProjectile(source, position, velocity * 3, ModContent.ProjectileType<GuardiansWrathProj>(), damage, knockback, player.whoAmI);
                }
                return false;
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<GuardiansWrathProj>()] > 0)
            {
                return false;
            }
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<GuardiansWrathProj>()] > 0)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "Missing").Value, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
                return false;
            }
            return true;
        }
    }
}
