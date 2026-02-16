using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.Items.Weapons
{
    public class Delirium : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 100;
            Item.knockBack = 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<NauseatingPowder>(), 4).
                AddIngredient(ModContent.ItemType<SealToken>(), 4).
                AddIngredient(ModContent.ItemType<VoidSingularity>(), 1).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.friendly)
                        continue;
                    if (n.dontTakeDamage)
                        continue;
                    if (n.Distance(player.Center) > 640)
                        continue;
                    n.AddBuff(BuffID.OnFire3, 300);
                    n.AddBuff(BuffID.CursedInferno, 300);
                    n.AddBuff(BuffID.Ichor, 300);
                    n.AddBuff(BuffID.Frostburn2, 300);
                    n.AddBuff(ModContent.BuffType<Plague>(), 300);
                    n.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
                    n.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
                }
            }
            return true;
        }
    }
}
