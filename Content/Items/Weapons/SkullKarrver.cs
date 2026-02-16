using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod;
using Terraria.Audio;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
    public class SkullKarrver : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = 57;
            Item.knockBack = 12f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.Heal(10);
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, player.Center);
            for (int i = 0; i < 20; i++)
            {
                GeneralParticleHandler.SpawnParticle(new BloodParticle(target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(8, 22), 20, Main.rand.NextFloat(0.8f, 1.2f), Color.Red));
            }
        }
    }
}
