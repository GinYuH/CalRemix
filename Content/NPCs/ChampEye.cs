using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.NPCs
{
    public class ChampEye : ModNPC
    {
        public override string Texture => "CalRemix/Content/NPCs/FloatingBiomass";
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 400;
            NPC.damage = 50;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        public override void AI()
        {
            NPC.rotation = NPC.velocity.ToRotation();
            if (!NPC.noTileCollide)
            {
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                    if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                    {
                        NPC.velocity.X = 2f;
                    }
                    if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                    {
                        NPC.velocity.X = -2f;
                    }
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                    if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                    {
                        NPC.velocity.Y = 1f;
                    }
                    if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                    {
                        NPC.velocity.Y = -1f;
                    }
                }
            }
            NPC.TargetClosest();
            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height))
            {
                if (State > 0f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    State = 0f;
                    Timer = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (State == 0f)
            {
                Timer += 1f;
            }
            if (Timer >= 150f)
            {
                State = 1f;
                Timer = 0f;
                NPC.netUpdate = true;
            }
            if (State == 0f)
            {
                NPC.alpha = 0;
                NPC.noTileCollide = false;
            }
            else
            {
                NPC.wet = false;
                NPC.alpha = 200;
                NPC.noTileCollide = true;
            }
            float num2 = 4f;
            float num3 = 1.5f;
            num2 *= 1f + (1f - NPC.scale);
            num3 *= 1f + (1f - NPC.scale);
            if (NPC.direction == -1 && NPC.velocity.X > 0f - num2)
            {
                NPC.velocity.X -= 0.1f;
                if (NPC.velocity.X > num2)
                {
                    NPC.velocity.X -= 0.1f;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X += 0.05f;
                }
                if (NPC.velocity.X < 0f - num2)
                {
                    NPC.velocity.X = 0f - num2;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < num2)
            {
                NPC.velocity.X += 0.1f;
                if (NPC.velocity.X < 0f - num2)
                {
                    NPC.velocity.X += 0.1f;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X -= 0.05f;
                }
                if (NPC.velocity.X > num2)
                {
                    NPC.velocity.X = num2;
                }
            }
            if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num3)
            {
                NPC.velocity.Y -= 0.04f;
                if (NPC.velocity.Y > num3)
                {
                    NPC.velocity.Y -= 0.05f;
                }
                else if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y += 0.03f;
                }
                if (NPC.velocity.Y < 0f - num3)
                {
                    NPC.velocity.Y = 0f - num3;
                }
            }
            else if (NPC.directionY == 1 && NPC.velocity.Y < num3)
            {
                NPC.velocity.Y += 0.04f;
                if (NPC.velocity.Y < 0f - num3)
                {
                    NPC.velocity.Y += 0.05f;
                }
                else if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y -= 0.03f;
                }
                if (NPC.velocity.Y > num3)
                {
                    NPC.velocity.Y = num3;
                }
            }
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
                NPC.TargetClosest();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
    }
}
