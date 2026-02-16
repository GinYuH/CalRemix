using CalamityMod;
using CalamityMod.Tiles.Ores;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix
{
    public class Vernix : PhoenixAbstract
    {
        public override int damage => 90;
        public override int defense => 40;
        public override int health => 9000;
        public override int projType => ModContent.ProjectileType<PerennialFlowerMine>();
        public override int dustType => DustID.JungleSpore;

        public override void OnKill()
        {
            RemixDowned.downedVernix = true;

            CalamityUtils.SpawnOre(ModContent.TileType<PerennialOre>(), 12E-05, 0.65f, 0.85f, 5, 10, TileID.Dirt, TileID.Stone);

            string key = "Mods.CalamityMod.Status.Progression.PlantOreText";
            Color messageColor = Color.GreenYellow;
            CalamityUtils.DisplayLocalizedText(key, messageColor);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); crest
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); acc
            npcLoot.Add(ModContent.ItemType<BlossomBlast>(), 2 / 3);
        }
    }
}