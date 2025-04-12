using System.Collections.Generic;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.NPCs.Bosses.Noxus;
using CalRemix.Content.NPCs;
using CalRemix.Core;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.SummonItems
{
    public class Genesis : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = null;
            Item.value = 0;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override bool CanUseItem(Player player) =>
            !NPC.AnyNPCs(ModContent.NPCType<NoxusEgg>()) && !NPC.AnyNPCs(ModContent.NPCType<EntropicGod>()) && !NPC.AnyNPCs(ModContent.NPCType<NoxusEggCutscene>());

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int noxusID = player.altFunctionUse == 2 || !RemixDowned.downedNoxegg ? ModContent.NPCType<NoxusEgg>() : ModContent.NPCType<EntropicGod>();

                // If the player is not in multiplayer, spawn Noxus.
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, noxusID);

                // If the player is in multiplayer, request a boss spawn.
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: noxusID);
            }

            return true;
        }

        public override bool AltFunctionUse(Player player) => RemixDowned.downedNoxegg;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var toolip = tooltips.Find(l => l.Text.Contains("Summons"));
            if (RemixDowned.downedNoxegg)
                toolip.Text = Language.GetTextValue($"Mods.{Mod.Name}.Items.{Name}.AlternateTooltip");
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).
                AddTile<DraedonsForge>().
                AddIngredient(ItemID.StoneBlock, 50).
                AddIngredient(ModContent.ItemType<ShadowspecBar>(), 10).
                AddIngredient(ModContent.ItemType<Rock>()).
                Register();
        }
    }
}
