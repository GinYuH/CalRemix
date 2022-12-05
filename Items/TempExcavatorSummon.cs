using CalRemix.Projectiles;
using CalRemix.NPCs.Bosses;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Items
{
    public class TempExcavatorSummon : ModItem
    {
        public override string Texture => "CalRemix/Items/KnowledgeExcavator";
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Excavator Beacon");
            Tooltip.SetDefault("Manually summons the Wulfrum Excavator\n" +
            "This is a debug item");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = 1;
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;

        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.WulfrumExcavatorHead>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.WulfrumExcavatorHead>());
            new SoundStyle("Terraria/Sounds/Roar");
            return true;
        }

    }
}
