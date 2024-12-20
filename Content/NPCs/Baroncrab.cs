using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.UI;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Linq;
using CalRemix.Core.Biomes;
using System.Collections.Generic;
using CalRemix.Content.Items.Placeables;
using Microsoft.Xna.Framework;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Baroncrab : ModNPC
    {
        public static List<int> baronTiles = new List<int>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Position = new Vector2(0, 115)
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            baronTiles.Add(ItemType<BaronBrine>());
            baronTiles.Add(ItemType<TanzaniteGlass>());
            baronTiles.Add(ItemType<Brinerack>());
            baronTiles.Add(ItemType<BanishedPlating>());
            baronTiles.Add(ItemType<Baronsand>());
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
            NPC.HitSound = SoundID.NPCDeath41;
            NPC.DeathSound = SoundID.Item14;
            NPC.lifeMax = 2001;
            NPC.chaseable = false;
            NPC.rarity = 1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BaronStraitBiome>().Type };
        }
        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("Despite their profound knowledge, some Barocrabs seek for higher knowledge. To achieve this, they attune with the Baron Strait's Tanzanite to achieve a new status."),
            });
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, 1, 0, 0.2f);
            NPC.catchItem = baronTiles[(int)Main.rand.Next(0, baronTiles.Count - 1)];
            NPC.TargetClosest();
            if (Main.player[NPC.target].Distance(NPC.Center) < 160 || NPC.aiStyle == NPCAIStyleID.Worm)
            {
                NPC.aiStyle = NPCAIStyleID.Worm;
                NPC.noTileCollide = true;
                AIType = NPCID.TruffleWormDigger;
            }
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 4)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome<BaronStraitBiome>())
            {
                return 1f;
            }
            return 0f;
        }
    }
}