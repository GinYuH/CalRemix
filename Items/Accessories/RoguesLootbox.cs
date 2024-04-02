using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Items.Accessories
{
    public class RoguesLootbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Rogue's Lootbox");
            Tooltip.SetDefault("+30 maximum stealth\n"+
            "12 % increased rogue damage, and 12 % increased rogue crit chance\n" +
            "Vastly reduces enemy aggression, even in the abyss\n" +
            "Stealth generates 50 % faster when standing still\n" +
            "Mobile stealth generation exponentially accelerates while not attacking\n" +
            "Stealth strikes have a 100 % critical hit chance\n" +
            "Stealth strikes only expend 50 % of your max stealth\n" +
             "A copy of your weapon falls downward from the sky each use which deals 30 % of the weapon's damage\n" +
            "A burst of homing souls, bouncing jewels, and fireballs are released once the copy projectile dies\n" +
            "Sun Tornadoes are summoned on stealth strike hits with a cap of 2 at once\n" +
            "Grants the ability to evade attacks in a blast of darksun light, which inflicts extreme damage in a wide area\n" +
            "Evading an attack grants full stealth but has a 20 second cooldown\n" +
            "This cooldown is shared with all other dodges and reflects\n" +
            "Hit the spectral veil key to summon an eclipse aura that nullifies damage for 5 seconds with a 20 second cooldown"); 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.stealthGenStandstill += 0.25f;
            modPlayer.rogueStealthMax += 0.1f;
            modPlayer.eclipseMirror = true;
            modPlayer.stealthStrikeHalfCost = true;
            player.GetCritChance<ThrowingDamageClass>() += 6;
            player.GetDamage<ThrowingDamageClass>() += 0.06f;
            player.aggro -= 700;
            player.buffImmune[ModContent.BuffType<Dragonfire>()] = true;
            player.luck += 0.2f;
            player.Calamity().thiefsDime = true;
            player.GetModPlayer<CalRemixPlayer>().roguebox = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EclipseMirror>(1).
                AddIngredient<DragonScales>(1).
                AddIngredient<EtherealExtorter>(1).
                AddIngredient<VeneratedLocket>(1).
                AddIngredient<GloveOfPrecision>(1).
                AddIngredient<GloveOfRecklessness>(1).
                AddIngredient<SandCloak>(1).
                AddIngredient<ScuttlersJewel>(1).
                AddIngredient<SpectralVeil>(1).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
