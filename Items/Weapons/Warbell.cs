using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Buffs;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.Audio;

namespace CalRemix.Items.Weapons
{
    public class Warbell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warbell");
            Tooltip.SetDefault("Boosts minion damage by 10% on enemy hits");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 15;
            Item.knockBack = 4f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.AddBuff(ModContent.BuffType<Warath>(), 300);
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
            SoundEngine.PlaySound(BetterSoundID.ItemBell, target.Center);
        }
    }
}
