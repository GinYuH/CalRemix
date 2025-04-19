using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalRemix.Content.NPCs.Bosses.Hypnos
{
    public class HypnosPlug : ModNPC
    {
        public bool initialized = false;
        NPC hypnos;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("XP-00 Hypnos Plug");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 20000;
            NPC.LifeMaxNERB(250000, 328125);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.damage = 1;
            NPC.HitSound = null;
            NPC.DeathSound = CalamityMod.Sounds.CommonCalamitySounds.ExoDeathSound with { Pitch = 0.6f, Volume = 0.6f };
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 14;
            NPC.height = 14;
            NPC.defense = 20;
        }

        public override void AI()
        {
			hypnos = Main.npc[(int)NPC.ai[0]];

			int startneuron = 0;
			int heighoffset = 20;
			int heighoffsetin = 30;
			int innerdist = 70;
			int outerdist = 80;

			Vector2 leftleftplug = new Vector2(hypnos.Center.X - outerdist, hypnos.Center.Y + heighoffset);
			Vector2 leftplug = new Vector2(hypnos.Center.X - innerdist, hypnos.Center.Y + heighoffsetin);
			Vector2 rightplug = new Vector2(hypnos.Center.X + innerdist, hypnos.Center.Y + heighoffsetin);
			Vector2 rightrightplug = new Vector2(hypnos.Center.X + outerdist, hypnos.Center.Y + heighoffset);

			Vector2 pluglocation = hypnos.Center;


			switch (NPC.ai[1])
			{
				case 0:
					pluglocation = leftleftplug;
					startneuron = 0;
					NPC.rotation = -(float)Math.PI / 2;
					break;
				case 1:
					pluglocation = leftplug;
					startneuron = 3;
					NPC.rotation = -(float)Math.PI / 2;
					break;
				case 2:
					pluglocation = rightplug;
					NPC.rotation = (float)Math.PI / 2;
					startneuron = 6;
					break;
				case 3:
					pluglocation = rightrightplug;
					NPC.rotation = (float)Math.PI / 2;
					startneuron = 9;
					break;
			}
			if (NPC.ai[2] == 0)
			{
				
				for (int i = 0; i < 3; i++)
				{
					if (!(Main.getGoodWorld && i == 1))
					{
						//if (Main.netMode != NetmodeID.MultiplayerClient)
							NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AergiaNeuron>(), 0, (int)NPC.ai[0], startneuron + i, 0, NPC.whoAmI);
					}
				}
				NPC.ai[2] = 1;
			}
			//if (!initialized)
   //         {
   //             hypnos = Main.npc[(int)NPC.ai[0]];
   //         }
            if (!hypnos.active)
            {
                NPC.active = false;
            }
            NPC.damage = 0;

            
            

            NPC.position = pluglocation;
            
        }

		public override void OnKill()
		{
			base.OnKill();
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(initialized);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            initialized = reader.ReadBoolean();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 8;
                Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoHitSound, NPC.Center);
            }
        }
    }
}