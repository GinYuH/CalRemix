using CalamityMod;
using CalRemix.Biomes;
using CalRemix.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.NPCs.Bosses.Pyrogen
{
    public class PyrogenShield : ModNPC
    {
        public override string Texture => "CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrogen Shield");
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 24;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 20;
            NPC.defense = 1;
        }

        public override void AI()
        {
            NPC pyro = Main.npc[(int)NPC.ai[0]];
            if (pyro.active && pyro.type == ModContent.NPCType<Pyrogen>())
            {
                NPC.localAI[1] += 1f;
                float distance = 100;
                distance = pyro.width >= pyro.height ? pyro.width : pyro.height;
                double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly * 660 + NPC.localAI[1];
                double rad = deg * (Math.PI / 180);
                float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;

                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                float rotOffset = 0;
                switch (NPC.ai[2])
                {
                    case 0:
                        rotOffset = -MathHelper.PiOver2;
                        break;
                    case 1:
                        rotOffset = -MathHelper.PiOver2;
                        break;
                    case 2:
                        rotOffset = MathHelper.PiOver4;
                        break;
                    case 3:
                        rotOffset = -MathHelper.PiOver2 - MathHelper.PiOver4;
                        break;
                }
                NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() + rotOffset;                
            }
            else
            {
                NPC.StrikeInstantKill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = TextureAssets.Npc[NPC.type].Value;
            switch (NPC.ai[2])
            {
                case 0:
                    sprite = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield1").Value;
                    break;
                case 1:
                    sprite = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield2").Value;
                    break;
                case 2:
                    sprite = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield3").Value;
                    break;
                case 3:
                    sprite = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield4").Value;
                    break;
            }
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(sprite, npcOffset, null, NPC.GetAlpha(drawColor), NPC.rotation, sprite.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}