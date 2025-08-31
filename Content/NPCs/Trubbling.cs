using CalamityMod;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Trubbling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 38;
            NPC.CloneDefaults(NPCID.GoblinScout);
            NPC.catchItem = (short)ItemType<Items.Critters.TrubblingItem>();
            NPC.lavaImmune = false;
            AIType = NPCID.GoblinScout;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = new SoundStyle("CalRemix/Assets/Sounds/Funguscream") { Pitch = 0.5f, Volume = 0.8f };
            NPC.lifeMax = 20;
            NPC.defense = 1;
            NPC.chaseable = false;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
        }
        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void AI()
        {
            NPC.spriteDirection = -NPC.direction;

        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y < 2)
            {
                if (NPC.velocity.X != 0)
                {
                    NPC.frameCounter += 1;
                    if (NPC.frameCounter > 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 7)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else if (NPC.velocity.X == 0)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneGlowshroom)
                return SpawnCondition.UndergroundMushroom.Chance * 0.05f;
            if (spawnInfo.Player.ZoneGlowshroom)
                return SpawnCondition.OverworldMushroom.Chance * 0.05f;
            return 0;
        }
    }
}