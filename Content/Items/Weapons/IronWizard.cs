using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.NPCs.SupremeCalamitas;
using Terraria.Audio;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.Weapons
{
    public class IronWizard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Ionogen;
            Item.value = 0;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = SoundID.DD2_MonkStaffGroundImpact with { Pitch = -0.6f };
            Item.DamageType = DamageClass.Melee;
            Item.hammer = 80;
            Item.damage = 68;
            Item.knockBack = 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(Recipes.HMT1Bar, 10).
                AddRecipeGroup(RecipeGroupID.IronBar, 16).
                AddTile(TileID.Anvils).
                DisableDecraft().
                Register();
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.type == NPCID.Wizard)
            {
                target.dontTakeDamage = false;
                modifiers.SourceDamage *= 2222f;
            }
        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (target.type == NPCID.Wizard)
            {
                return true;
            }
            return base.CanHitNPC(player, target);
        }
    }
}
