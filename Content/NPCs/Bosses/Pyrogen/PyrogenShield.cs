using CalamityMod;
using CalRemix.Core.Retheme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Bosses.Pyrogen
{
    public class PyrogenShield : ModNPC
    {
        public bool stopAi1 = false;
        public static Asset<Texture2D> BloomTexture = null;
        public static Asset<Texture2D> Glow = null;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrogen's Shield");
            Main.npcFrameCount[Type] = 6;
            if (!Main.dedServ)
            {
                BloomTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenShieldAura");
                Glow = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenShield_Glow");
            }
            this.HideFromBestiary();
        }

        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/RavagerRockPillarHit", 3);
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/CryogenShieldBreak");

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.width = 30;
            NPC.height = 24;
            NPC.damage = 220;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.DR_NERD(0.9f);
            NPC.defense = 60;
            NPC.LifeMaxNERB(220000, 242000, 842000); ;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, TorchID.Red);
            NPC pyro = Main.npc[(int)NPC.ai[0]];
            if (pyro.active && pyro.type == ModContent.NPCType<Pyrogen>())
            {
                NPC.Calamity().newAI[0] = pyro.ai[0];
                switch (NPC.Calamity().newAI[0])
                {
                    case 0 or 1 or 2: //default guarding behavior
                        {
                            NPC.damage = 220;
                            stopAi1 = false;
                            NPC.localAI[1] += 5f;
                            float distance = 50;
                            distance += pyro.width >= pyro.height ? pyro.width : pyro.height;
                            double deg = 24 * NPC.ai[1] + Main.GlobalTimeWrappedHourly + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;

                            NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            float rotOffset = 0;
                            NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() - MathHelper.PiOver2;
                            break;
                        }
                    case 3: //all players are pulled in with the boss; rotate out further in this case
                        {
                            if (!stopAi1) { 
                                NPC.localAI[1] += 1f;
                            }
                            Vector2 idealpos = NPC.Center;

                            float distance = MathHelper.Clamp(MathHelper.Lerp(50, 300, pyro.ai[2] / Pyrogen.BlackholeSafeTime), 50, 300);
                            distance += pyro.width >= pyro.height ? pyro.width : pyro.height;

                            double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly * 660 + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 4;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 4;
                            
                            if (NPC.localAI[1] <= Pyrogen.BlackholeSafeTime)
                            {
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            }
                            else if (NPC.localAI[1] < 600)
                            {
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                                NPC.damage = 200;
                            }

                            if (NPC.localAI[1] < Pyrogen.BlackholeSafeTime * 2)
                            {
                                NPC.damage = 0;
                            }

                            if (NPC.localAI[1] == 670) // repelling back out! returning to original distance...
                            {
                                distance = 100;
                                hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                                hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                                NPC.damage = 0;
                            }
                            if (pyro.ai[3] == 720) // done! restoring contact damage...
                            {
                                stopAi1 = true;
                                NPC.localAI[1] = 0;
                            }

                            NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            float rotOffset = 0;
                            NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() - MathHelper.PiOver2;
                            break;
                        }
                    case 4: //switching phase; default guarding behavior but without contact damage
                        {
                            NPC.damage = 0;
                            NPC.localAI[1] += 8f;
                            float distance = 100;
                            distance = pyro.width >= pyro.height ? pyro.width : pyro.height;
                            double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;

                            NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            float rotOffset = 0;
                            NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() - MathHelper.PiOver2;
                            break;
                        }
                    case 9: //switching phase; default guarding behavior but without contact damage
                        {
                            NPC.damage = 0;
                            NPC.localAI[1] += 8f;
                            float distance = 100;
                            distance = pyro.width >= pyro.height ? pyro.width : pyro.height;
                            double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;

                            NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            float rotOffset = 0;
                            NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() - MathHelper.PiOver2;
                            break;
                        }
                    default: //default guarding behavior
                        {
                            NPC.damage = 220;
                            NPC.localAI[1] += 8f;
                            float distance = 100;
                            distance = pyro.width >= pyro.height ? pyro.width : pyro.height;
                            double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;

                            NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                            float rotOffset = 0;
                            NPC.rotation = NPC.DirectionTo(pyro.Center).ToRotation() - MathHelper.PiOver2;
                        }
                        break;
                }
            }
            else
            {
                NPC.StrikeInstantKill();
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * (int)NPC.ai[2];
        }

        public override void ModifyTypeName(ref string typeName)
        {
            if (Main.zenithWorld)
            {
                typeName = CalamityUtils.GetTextValue("NPCs.CryogenShield.DisplayName");
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if ((projectile.penetrate == -1 || projectile.penetrate > 1) && !projectile.minion)
            {
                projectile.penetrate = 1; //piercing is disabled to prevent smacking the core offscript
            }
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.Calamity().newAI[0] == 4)
                return false;
            Texture2D sprite = TextureAssets.Npc[NPC.type].Value;
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(BloomTexture.Value, npcOffset, NPC.frame, Color.White with { A = 0 }, NPC.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(sprite.Width /2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(Glow.Value, npcOffset, NPC.frame, Color.White, NPC.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            //spriteBatch.EnterShaderRegion(BlendState.Additive);
            //spriteBatch.ExitShaderRegion();
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (NPC.Calamity().newAI[0] == 4)
                return false;
            return null;
        }
    }
}