using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Content.NPCs.Bosses.Pathogen
{
    public class PathogenShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pathogen's Shield");
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 170;
            NPC.height = 166;
            NPC.defense = 20;
            NPC.lifeMax = 8000;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            NPC.dontTakeDamage = true;
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci != null && carci.active && carci.type == ModContent.NPCType<Pathogen>())
            {
                NPC.position = carci.Center - NPC.Size / 2;
                NPC.rotation += 0.1f;
            }
            else
            {
                NPC.active = false;
            }
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
        public override bool CheckActive() => !(Main.npc[(int)NPC.ai[0]] != null && Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<Pathogen>());

        public override bool NeedSaving()
        {
            return true;
        }
    }
}
