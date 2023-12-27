using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using System.IO;
using Microsoft.Xna.Framework;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.OldDuke;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.World;

namespace CalRemix.NPCs.Bosses.Acideye
{
    [AutoloadBossHead]
    public class Acideye : ModNPC
    {
        private Player Target => Main.player[NPC.target];
        public ref float Wait => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Wait2 => ref NPC.ai[2];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Irradiated>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<SulphuricPoisoning>()] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new()
            {
                Scale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Frame = 1;
        }
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.width = 110;
            NPC.height = 110;
            NPC.lifeMax = 4000;
            NPC.defense = Main.hardMode ? 40 : 15;
            NPC.damage = Main.hardMode ? 120 : 70;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 20);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
            if (!Main.dedServ)
            {
                if (ModLoader.HasMod("CalamityModMusic"))
                    Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/OldDuke");
                else
                    Music = MusicID.Boss5;
            }
        }
        public override void AI()
        {
            NPC.TargetClosest();
            float life = (float)NPC.life / (float)NPC.lifeMax;
            bool stage2 = CalamityWorld.revenge ? life <= 0.75f : Main.expertMode ? life <= 0.7f : life <= 0.65f;
            bool p2 = CalamityWorld.death ? life <= 0.65f : (CalamityWorld.revenge ? life <= 0.60f : (Main.expertMode ? life <= 0.55f : life <= 0.5f));
            bool p3 = CalamityWorld.death ? life <= 0.5f : (CalamityWorld.revenge ? life <= 0.45f : (Main.expertMode ? life <= 0.4f : life <= 0.3f));
            if (!NPC.HasValidTarget)
            {
                NPC.velocity.Y--;
                return;
            }
            if (!p2)
                NPC.velocity = (new Vector2(Target.Center.X - NPC.Center.X, Target.Center.Y - 176 - NPC.Center.Y) / 50f).ClampMagnitude(0, 12);
            else
                NPC.velocity = (new Vector2(Target.Center.X - NPC.Center.X, Target.Center.Y - 128 - NPC.Center.Y) / 50f).ClampMagnitude(0, 12);
            NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() - MathHelper.Pi / 2;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
                return;
            for (int k = 0; k < 20; k++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f, 100, default, 3.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f, 100, default, 2f);
                dust.velocity *= 2f;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            float life = (float)NPC.life / (float)NPC.lifeMax;
            bool p2 = CalamityWorld.death ? life <= 0.65f : (CalamityWorld.revenge ? life <= 0.60f : (Main.expertMode ? life <= 0.55f : life <= 0.5f));
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                if (!p2)
                {
                    if (NPC.frame.Y > frameHeight * 1)
                        NPC.frame.Y = 0;
                    else
                        NPC.frame.Y += frameHeight;
                }
                else
                {
                    if (NPC.frame.Y > frameHeight * 4)
                        NPC.frame.Y = frameHeight * 3;
                    else
                        NPC.frame.Y += frameHeight;
                }
                NPC.frameCounter = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                new FlavorTextBestiaryInfoElement("A large eye mutated by the Sulphurous Sea. Defeating this may allow its prey to come out of hiding.")
            });
        }
    }
}
