using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalamityMod;
using CalamityMod.NPCs.ExoMechs;
using CalRemix.Content.NPCs;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.Audio;
using CalRemix.Core.World;
using CalRemix.Core.Subworlds;

namespace CalRemix.Content.Items.SummonItems
{
    public class TurnipSprout : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Draedon>()) || NPC.AnyNPCs(ModContent.NPCType<HypnosDraedon>()))
            {
                return true;
            }
            if (RemixDowned.downedDraedon && player.Distance(SealedSubworldData.tentPos) < 1000)
            {
                return true;
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (NPC.AnyNPCs(ModContent.NPCType<Draedon>()) || NPC.AnyNPCs(ModContent.NPCType<HypnosDraedon>()))
                {
                    SoundEngine.PlaySound(SoundID.Thunder);
                    player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(CalRemixHelper.LocalText("DeathReasons.DraedonSprout").Format(player.name))), 56, 1);
                    Main.NewText(CalRemixHelper.LocalText("StatusText.DraedonSprout").Value, Draedon.TextColorEdgy);
                }
                else
                {
                    NPC.NewNPC(Item.GetSource_FromThis(), (int)SealedSubworldData.tentPos.X, (int)SealedSubworldData.tentPos.Y, ModContent.NPCType<DreadonFriendly>(), ai1: 1);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenSealedTear>(), 6)
                .AddIngredient(ModContent.ItemType<TurnipMesh>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}