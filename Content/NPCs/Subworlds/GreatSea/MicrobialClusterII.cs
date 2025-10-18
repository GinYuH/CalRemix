using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class MicrobialClusterII : ModNPC
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public const int ChargeRate = 120;
        public const int SlowdownTime = 45;
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.damage = 0;
            NPC.width = 24;
            NPC.height = 24;
            NPC.lifeMax = 5;
            NPC.aiStyle = AIType = -1;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 0.1f;
        }
        public override void AI()
        {
            if (NPC.collideX || NPC.collideY)
            {
                NPC.velocity *= -1f;
                NPC.netUpdate = true;
            }
            DelegateMethods.v3_1 = Color.Turquoise.ToVector3() * 2f;
            Utils.PlotTileLine(NPC.Center, NPC.Center + NPC.velocity * 10f, 8f, DelegateMethods.CastLightOpen);
            NPC.ai[0]++;
            if (NPC.ai[0] % SlowdownTime > SlowdownTime - SlowdownTime)
            {
                NPC.velocity *= 0.98f;
            }
            if (NPC.ai[0] % SlowdownTime == SlowdownTime - 1)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(-Vector2.UnitY).RotatedByRandom(MathHelper.PiOver4) * 4f;
            }
            if (NPC.ai[0] % 32f == 31f)
            {
                Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Clentaminator_Cyan);
                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1f, 2f);
                dust.noGravity = true;
                dust.scale = 1.6f;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.InSulphur() && spawnInfo.Water ? 0.4f : 0f;

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 6; k++)
                {
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Clentaminator_Cyan);
                    dust.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1f, 2f);
                    dust.scale = 1.2f;
                }
            }
        }
    }
}
