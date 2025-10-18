using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.UI;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Linq;
using Microsoft.Xna.Framework;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Barocrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Position = new Vector2(0, 115)
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            if (Main.dedServ)
                return;
            HelperMessage.New("Barocrab",
                "Oooh, a Barocrab! You should go approach it and say hello! I heard they�re friendly and know of an ancient art called... the \"Crab Secret\".",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
            HelperMessage.New("BarocrabRun",
                "Oh no it ran away! Now you'll never be able to learn the Crab Secret... Well, at least not until another one appears.",
                "FannySob",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type && n.aiStyle == NPCAIStyleID.Worm), cantBeClickedOff: true);
        }

        [JITWhenModsEnabled("CalamityMod")]
        public override void SetDefaults()
        {
            NPC.width = 312;
            NPC.height = 196;
            NPC.CloneDefaults(NPCID.GlowingSnail);
            NPC.catchItem = (short)ItemType<Baroclaw>();
            NPC.lavaImmune = false;
            AIType = NPCID.GlowingSnail;
            AnimationType = NPCID.GlowingSnail;
            NPC.HitSound = SoundID.NPCDeath41;
            NPC.DeathSound = SoundID.Item14;
            NPC.lifeMax = 2000;
            NPC.chaseable = false;
            NPC.rarity = 1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = false;
        }
        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreAI()
        {
            NPC.TargetClosest();
            if (Main.player[NPC.target].Distance(NPC.Center) < 160 || NPC.aiStyle == NPCAIStyleID.Worm)
            {
                NPC.aiStyle = NPCAIStyleID.Worm;
                NPC.noTileCollide = true;
                AIType = NPCID.TruffleWormDigger;
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Water && spawnInfo.Player.ZoneBeach)
            {
                return Terraria.ModLoader.Utilities.SpawnCondition.Ocean.Chance * 0.2f;
            }
            return 0f;
        }
    }
}