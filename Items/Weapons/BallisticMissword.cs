using CalamityMod.Rarities;
using CalRemix.Projectiles.Weapons;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons
{
    public class BallisticMissword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ballistic Missword");
            Tooltip.SetDefault("Has a chance to explode when swung, damaging the wielder");
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 720;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 46;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 46;
            Item.useTurn = true;
            Item.knockBack = 10.5f;
            Item.UseSound = BetterSoundID.ItemInfernoExplosion;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(100);
            Item.rare = RarityHelper.Hydrogen;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(300))
            {
                player.itemAnimation = 0;
                Vector2 pos = player.Center + player.Center.DirectionTo((Vector2)player.HandPosition).SafeNormalize(Vector2.UnitY) * 80;
                int p = Projectile.NewProjectile(player.GetSource_FromThis(), pos, Vector2.Zero, ProjectileID.GrenadeIII, (int)(Item.damage * 0.1f), Item.knockBack, player.whoAmI);
                player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, p), Item.damage, 0);
                Main.projectile[p].Kill();
                SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaExplosionSound, player.position);

                Gore.NewGore(Item.GetSource_FromThis(), pos, Main.rand.NextVector2Circular(10, 10), Mod.Find<ModGore>("Missword1").Type, 1f);
                Gore.NewGore(Item.GetSource_FromThis(), pos, Main.rand.NextVector2Circular(10, 10), Mod.Find<ModGore>("Missword2").Type, 1f);
                Gore.NewGore(Item.GetSource_FromThis(), pos, Main.rand.NextVector2Circular(10, 10), Mod.Find<ModGore>("Missword3").Type, 1f);
            }
        }
    }
}