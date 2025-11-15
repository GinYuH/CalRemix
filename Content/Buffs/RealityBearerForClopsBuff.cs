using CalamityMod.NPCs;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class RealityBearerForClopsBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<RealityBearerForClopsNPC>().realityBearer = true;
        }
    }

    public class RealityBearerForClopsNPC : GlobalNPC
    {
        public bool realityBearer = false;

        public override bool InstancePerEntity => true;

        #region not buff related
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!CalamityGlobalNPC.GetDownedBossVariable(NPCID.Deerclops))
            {
                TextureAssets.Npc[NPCID.Deerclops] = ModContent.Request<Texture2D>("CalRemix/Assets/Textures/Cheerclops");
            }
            else
            {
                TextureAssets.Npc[NPCID.Deerclops] = ModContent.Request<Texture2D>("Terraria/Images/NPC_668");
            }

                return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Deerclops && !CalamityGlobalNPC.GetDownedBossVariable(NPCID.Deerclops))
            {
                npc.AddBuff(ModContent.BuffType<RealityBearerForClopsBuff>(), 22);
            }

            return true;
        }
        #endregion

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (realityBearer)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Extra_39").Value;
                if (ModLoader.HasMod("ThoriumMod"))
                    texture = ModContent.Request<Texture2D>("ThoriumMod/Buffs/RealityBearer_Texture").Value;
                float facingOffset = 0;
                float crownOffset = 50 + (float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 20);

                if (npc.type == NPCID.Deerclops)
                {
                    facingOffset = 30;

                    if (npc.frame.Y == 13 || npc.frame.Y == 14 || npc.frame.Y == 18)
                    {
                        crownOffset += 25;
                        facingOffset -= 30;
                    }
                    else if (npc.frame.Y == 15 || npc.frame.Y == 16 || npc.frame.Y == 17)
                    {
                        crownOffset -= 30;
                        facingOffset += 20;
                    }

                    if (npc.frame.Y >= 19)
                        facingOffset = 4;

                    facingOffset = npc.spriteDirection == 1 ? facingOffset : -facingOffset;
                }

                Vector2 centered = new Vector2(npc.Center.X + facingOffset, npc.position.Y - crownOffset) - screenPos;
                Main.EntitySpriteDraw(texture, centered, null, Color.White, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff<RealityBearerForClopsBuff>())
                realityBearer = false;
        }
    }
}