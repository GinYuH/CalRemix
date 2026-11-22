using CalamityMod;
using CalamityMod.Items.Potions;
using CalamityMod.Rarities;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalRemix.Content.Items.Potions.Recovery
{
    public class MiracleRoot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.OrangeBloodroot;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SuperHealingPotion);
            Item.consumable = true;
            Item.healLife = 0;
            Item.buffType = 0;
            Item.potion = false;
            Item.rare = ItemRarityID.Red;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffID.PotionSickness) && !player.HasBuff(ModContent.BuffType<MiracleSickness>());
        }

        public override bool? UseItem(Player player)
        {
            player.Heal(Math.Max(200, player.statLifeMax2 / 3));
            player.AddBuff(ModContent.BuffType<MiracleSickness>(), CalRemixHelper.MinutesToFrames(1));
            player.AddBuff(BuffID.PotionSickness, player.potionDelayTime);
            player.ConsumeItem(Type);
            return true;
        }
    }
}