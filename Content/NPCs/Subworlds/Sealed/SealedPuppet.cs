using CalamityMod;
using CalRemix.Core.Biomes;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class SealedPuppet : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Squirrel);
            NPC.width = 30;
            NPC.height = 54;
            AIType = NPCID.Squirrel;
            NPC.npcSlots = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 5000;
            NPC.defense = 10;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = [GetInstance<SealedFieldsBiome>().Type];
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0)
            {
                NPC.frameCounter += 0.075f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                int frame = (int)NPC.frameCounter;
                NPC.frame.Y = frame * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }

    public class SealedCitizen : SealedPuppet
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            SpawnModBiomes = [GetInstance<TurnipBiome>().Type];
        }
    }
    public class EvilSealedPuppet : SealedPuppet
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.lifeMax = 7000;
            NPC.defense = 20;
            NPC.damage = 100;
            SpawnModBiomes = [GetInstance<BadlandsBiome>().Type];
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}