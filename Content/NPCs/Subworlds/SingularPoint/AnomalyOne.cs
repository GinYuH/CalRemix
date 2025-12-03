using CalRemix.Core.Biomes;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
    // [AutoloadBossHead]
    public class AnomalyOne : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public NPC MainHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC OrbHead
        {
            get => Main.npc[(int)NPC.localAI[1]];
            set => NPC.localAI[1] = value.whoAmI;
        }

        public bool PhaseTwo
        {
            get => NPC.localAI[2] == 1;
            set => NPC.localAI[2] = value.ToInt();
        }

        public enum PhaseType
        {
            SpawnAnim = 0,
            SineGas = 1,
            Flamethrower = 2,
            Knockout = 3
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 150;
            NPC.height = 150;
            NPC.lifeMax = 300000;
            NPC.damage = 340;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            NPC.behindTiles = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (target == null || !target.active || target.dead)
            {
                NPC.active = false;
                return;
            }
            if (!MainHead.active || MainHead.type != ModContent.NPCType<AnomalyTwo>())
            {
                int dHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyTwo>());
                if (dHead == -1)
                {
                    MainHead = Main.npc[dHead];
                    NPC.netUpdate = true;
                }
            }
            if (!OrbHead.active || OrbHead.type != ModContent.NPCType<AnomalyTwo>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyTwo>());
                if (oHead == -1)
                {
                    OrbHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            float phaseGate = 0.7f;
            if (!PhaseTwo && NPC.life < (NPC.lifeMax * phaseGate))
            {
                ChangePhase(PhaseType.Knockout);
            }
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnim:
                    {
                    }
                    break;
                case PhaseType.SineGas:
                    {
                    }
                    break;
                case PhaseType.Flamethrower:
                    {
                    }
                    break;
                case PhaseType.Knockout:
                    {
                        NPC.dontTakeDamage = true;
                        NPC.velocity = Vector2.Zero;
                    }
                    break;
            }
            Timer++;
        }

        public void ChangePhase(PhaseType phase)
        {
            CurrentPhase = phase;
            ExtraOne = 0;
            ExtraTwo = 0;
            Timer = 0;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //DrawGuy(spriteBatch, screenPos, drawColor);
            return false;
        }

        public void DrawGuy(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
    }
}