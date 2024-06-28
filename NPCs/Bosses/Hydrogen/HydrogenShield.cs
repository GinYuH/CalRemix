using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.NPCs.Bosses.Hydrogen
{
    public class HydrogenShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrogen's Shield");
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 190;
            NPC.height = 190;
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
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void AI()
        {
            NPC.dontTakeDamage = true;
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci != null && carci.active && carci.type == ModContent.NPCType<Hydrogen>())
            {
                NPC.position = carci.Center - NPC.Size / 2;
                if (carci.ai[0] == 0)
                {
                    NPC.scale = 1f;
                    NPC.rotation += 0.025f;
                }
                else
                {
                    NPC.scale = 1f;
                    NPC.rotation += 0.1f;
                }
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
        public override bool CheckActive() => !(Main.npc[(int)NPC.ai[0]] != null && Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<Hydrogen>());
    }
}
