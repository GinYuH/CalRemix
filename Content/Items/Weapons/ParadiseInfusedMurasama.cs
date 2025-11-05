using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Cooldowns;
using Terraria.Audio;

namespace CalRemix.Content.Items.Weapons
{
    public class ParadiseInfusedMurasama : ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 100;
            Item.value = Item.sellPrice(gold: 5);
            Item.shoot = ModContent.ProjectileType<ParadiseMurasamaSlash>();
            Item.rare = ItemRarityID.Red;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 && player.ownedProjectileCounts[base.Item.shoot] > 0)
            {
                return false;
            }
            if (player.altFunctionUse == 2 && player.HasCooldown(ParadiseHealCooldown.ID))
            {
                return false;
            }
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!player.HasCooldown(ParadiseHealCooldown.ID))
                {
                    player.HealEffect((int)(player.statLifeMax2 * 0.2f));
                    player.Heal((int)(player.statLifeMax2 * 0.2f));
                    SoundEngine.PlaySound(BetterSoundID.ItemManaCrystal with { Pitch = -0.7f }, player.Center);
                    player.AddCooldown(ParadiseHealCooldown.ID, CalamityUtils.SecondsToFrames(60));
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Muramasa).
                AddIngredient<ParadiseBlade>().
                AddIngredient<GildedShard>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
