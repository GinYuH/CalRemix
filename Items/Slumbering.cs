using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using CalRemix.NPCs.Bosses.BossScule;
using CalRemix.Retheme;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class Slumbering : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Slumbering");
            Tooltip.SetDefault("[c/FF0000:This world is not what it seems. Every crevice among the graves only whisper lies.]\n" +
                                "[c/FF0000:Do not tread the path carved out for you, instead seek truth in the unknown.]\n" +
                                "[c/FF0000:The world will reveal itself to you, but first I shall do so as well.]\n" +
                                "[c/FF0000:Use this and attempt my trial.]\n" +
                                "[c/FF0000:In the future, I may offer another trial. Keep in this in case that happen.]");
            //"After defeating what this world can offer, use this via the opposite \"input\" to start my final test for you.");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ModContent.RarityType<CalamityRed>();
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && !NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
            {
                int type = ModContent.NPCType<TheCalamity>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LoreAwakening>().
                Register();
        }
    }
}
