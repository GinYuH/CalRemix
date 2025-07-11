using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using Terraria.Audio;
using CalamityMod.Buffs.StatBuffs;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Accessories
{
    public class SoftScarf : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 20;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.resistCold = true;
            player.Calamity().rage += 0.1f;

            if (player.Calamity().rage >= player.Calamity().rageMax && !player.Calamity().rageModeActive)
            {
                // Rage duration isn't calculated here because the buff keeps itself alive automatically as long as the player has Rage left.
                player.AddBuff(ModContent.BuffType<RageMode>(), 2);

                // Play Rage Activation sound
                if (player.whoAmI == Main.myPlayer)
                    SoundEngine.PlaySound(CalamityMod.CalPlayer.CalamityPlayer.RageActivationSound);

                // TODO -- Rage should provide glowy red afterimages to the player for the duration.
                // If Shattered Community is equipped, the afterimages are magenta instead.
                int rageDustID = 235;
                int dustCount = 132;
                float minSpeed = 4f;
                float maxSpeed = 11f;
                for (int i = 0; i < dustCount; ++i)
                {
                    float speed = (float)Math.Sqrt(Main.rand.NextFloat(minSpeed * minSpeed, maxSpeed * maxSpeed));
                    Vector2 dustVel = Main.rand.NextVector2Unit() * speed;
                    Dust d = Dust.NewDustPerfect(player.Center, rageDustID, dustVel);
                    d.noGravity = !Main.rand.NextBool(4); // 25% of dust has gravity
                    d.noLight = false;
                    d.scale = Main.rand.NextFloat(0.9f, 2.1f);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RefinedCarnelianite>(), 4)
                .AddIngredient(ModContent.ItemType<TurnipMesh>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
