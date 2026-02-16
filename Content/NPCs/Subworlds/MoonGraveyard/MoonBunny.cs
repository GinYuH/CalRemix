using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.Content.NPCs.Subworlds.MoonGraveyard
{
    public class MoonBunny : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Legendary Wolfmole");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            AIType = NPCID.Bunny;
            AnimationType = NPCID.Bunny;

            //NPC.catchItem = (short)ItemType<Items.Critters.Wolfmole>();
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.SpawnedFromStatue = true;
        }

        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZoneSnow)
                return 0f;

            return SpawnCondition.OverworldDaySnowCritter.Chance * 2f;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    //dust
                }
            }
        }
    }
}
