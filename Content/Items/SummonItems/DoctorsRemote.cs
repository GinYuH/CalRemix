using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using SubworldLibrary;
using CalRemix.Core.Subworlds;

namespace CalRemix.Content.Items.SummonItems
{
    public class DoctorsRemote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Disilphia>());
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(new EntitySource_BossSpawn(player), (int)(player.Center.X), (int)(player.position.Y - 3000f), ModContent.NPCType<Disilphia>(), 1);
                Main.npc[npc].timeLeft *= 20;
                CalamityUtils.BossAwakenMessage(npc);
            }
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Disilphia>());

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Mercury>(), 20)
                .AddIngredient(ModContent.ItemType<VoidSingularity>())
                .AddIngredient(ModContent.ItemType<RefinedCarnelianite>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}