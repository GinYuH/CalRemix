using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.NPCs.SupremeCalamitas;
using Terraria.Audio;

namespace CalRemix.Items.Weapons
{
    public class CalamitySword : ModItem
    {
        public static SoundStyle AshesofCalamity = new SoundStyle("CalRemix/Sounds/AshesofCalamity");
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calamity Sword");
            Tooltip.SetDefault("Fires Calamity Beams\n'Contains the Ashes of Calamity'");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemDeathSickle;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 582;
            Item.knockBack = 10f;
            Item.shoot = ModContent.ProjectileType<CalamityBeam>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TerraBlade).
                AddIngredient(ModContent.ItemType<AshesofCalamity>(), 15).
                AddIngredient(ModContent.ItemType<AuricBar>(), 5).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.type == ModContent.NPCType<SupremeCalamitas>())
            {
                modifiers.SourceDamage *= 2222f;
                SoundEngine.PlaySound(AshesofCalamity, target.Center);
            }
        }
    }
}
