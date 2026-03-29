using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalRemix.Content.Items.Placeables.Subworlds.TheGray;
using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest
{
    public class BloodyTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = AresBody.EnragedSound with { Pitch = -2, Volume = 2 };
            Item.useTurn = true;
            Item.useAnimation = 14;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<WhisperingDeath>();
            Item.buffTime = CalamityUtils.SecondsToFrames(60);
            Item.width = 12;
            Item.height = 12;
        }

        public override bool OnPickup(Player player)
        {
            if (player.ItemSpace(Item).CanTakeItem)
            CalamityUtils.BroadcastLocalizedText("Mods.CalRemix.StatusText.BloodyTooth", Color.Crimson);
            return true;
        }
    }
}