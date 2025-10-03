using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod;
using CalRemix.Content.Items.Materials;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;

namespace CalRemix.Content.Items.Weapons
{
    public class Drabbler : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.UseSound = BetterSoundID.ItemSlapHandSmack;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = 215;
            Item.knockBack = 12f;
        }

        public override void HoldItem(Player player)
        {
            //player.moveSpeed *= 4;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.Heal(10);
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, player.Center);
            for (int i = 0; i < 20; i++)
            {
                GeneralParticleHandler.SpawnParticle(new BloodParticle(target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(8, 22), 20, Main.rand.NextFloat(0.8f, 1.2f), Color.MediumVioletRed));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Doubler>()).
                AddIngredient(ModContent.ItemType<SkullKarrver>()).
                AddIngredient(ModContent.ItemType<VoidSingularity>()).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
