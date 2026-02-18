using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.IO;

using Microsoft.Xna.Framework.Graphics;
using CalamityMod;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class BunnyBrawler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bunny Brawler");
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 76;
            NPC.aiStyle = -1;
            NPC.damage = 120;
            NPC.defense = 60;
            NPC.lifeMax = 400;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 0f;
            NPC.HitSound = SoundID.NPCHit14;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.Herpling;
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            bool isDead = NPC.life <= 0;
            if (isDead)          //this make so when the npc has 0 life(dead) he will spawn this
            {

            }
            for (int m = 0; m < (isDead ? 35 : 6); m++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }
        public bool SetLife = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(SetLife);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SetLife = reader.ReadBoolean(); //Set Lifex
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.velocity.Y < 0)
            {
                NPC.frame.Y = frameHeight;
            }
            else if(NPC.velocity.Y > 0)
            {
                NPC.frame.Y = frameHeight * 2;
            }
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void PostAI()
        {
            for (int m = NPC.oldPos.Length - 1; m > 0; m--)
            {
                NPC.oldPos[m] = NPC.oldPos[m - 1];
            }
            NPC.oldPos[0] = NPC.position;

            if (NPC.AnyNPCs(ModContent.NPCType<Rajah>()) ||
                   NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 5;
                }
                else
                {
                    NPC.alpha = 0;
                }
            }
            else
            {
                NPC.dontTakeDamage = true;
                if (NPC.alpha < 255)
                {
                    NPC.alpha += 5;
                }
                else
                {
                    NPC.active = false;
                }
            }
        }
    }
    public class BunnyBrawler2 : BunnyBrawler
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/RajahBoss/BunnyBrawler";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 170;
            NPC.defense = 100;
            NPC.lifeMax = 1600;
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage /= 2;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                BaseDrawing.DrawAfterimage(spriteBatch, TextureAssets.Npc[NPC.type].Value, 0, NPC, 1f, 1f, 10, true, 0f, 0f, CalamityUtils.MulticolorLerp(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Blue, Color.Red, Color.Green));
            }
            return false;
        }
    }
}