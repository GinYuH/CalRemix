using CalamityMod;
using CalamityMod.Sounds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds
{
    public class Car : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 200;
            NPC.height = 80;
            NPC.defense = 0;
            NPC.lifeMax = 2500000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound;
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
            NPC.dontTakeDamage = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                foreach (Player p in Main.ActivePlayers)
                {
                    if (p.Distance(NPC.Center) < 100)
                    {
                        NPC.ai[0] = 1;
                        NPC.ai[1] = p.whoAmI;
                        break;
                    }
                }
            }
            else if (NPC.ai[0] == 1)
            {
                Player person = Main.player[(int)NPC.ai[1]];
                if (!person.active)
                {
                    NPC.ai[0] = 0;
                    return;
                }
                int speed = 4;
                float acc = 1.1f;
                int maxSpeed = 10;
                bool moving = person.controlLeft || person.controlRight;
                if (moving)
                {
                    if (NPC.velocity.X == 0)
                    {
                        NPC.velocity.X = person.controlLeft ? -speed : speed;

                    }
                    else
                    {
                        if ((NPC.velocity.X > 0 && person.controlLeft) || (NPC.velocity.X < 0 && person.controlRight))
                        {
                            NPC.velocity.X *= -1;
                        }
                        else
                        {
                            NPC.velocity.X *= acc;
                            if (Math.Abs(NPC.velocity.X) > maxSpeed)
                            {
                                NPC.velocity.X = person.controlLeft ? -maxSpeed : maxSpeed;
                            }
                        }
                    }
                }
                else
                {
                    NPC.velocity.X *= 0.9f;
                }
                person.Center = NPC.Center + new Vector2(0, -20) + NPC.velocity;
                person.velocity = Vector2.Zero;
                person.direction = 1;
                NPC.rotation += NPC.velocity.X * 0.03f;

                if (!SubworldSystem.AnyActive() && person.controlUseItem)
                {
                    NPC.ai[0] = 0;
                }
            }
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D wheel = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Car_Wheel").Value;
            float offY = 8;
            spriteBatch.Draw(tex, NPC.Bottom - screenPos - Vector2.UnitY * offY, null, NPC.GetAlpha(drawColor), 0, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);
            float wheelRotation = NPC.rotation;
            spriteBatch.Draw(wheel, NPC.Bottom - screenPos + new Vector2(-60, -10 - offY), null, NPC.GetAlpha(drawColor), wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), NPC.scale, 0, 0);
            spriteBatch.Draw(wheel, NPC.Bottom - screenPos + new Vector2(80, -10 - offY), null, NPC.GetAlpha(drawColor), wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), NPC.scale, 0, 0);

            return false;
        }
    }
}