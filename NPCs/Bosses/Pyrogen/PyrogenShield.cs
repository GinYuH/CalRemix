using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
//using CalamityMod.CalPlayer;

namespace CalRemix.NPCs.Bosses.Pyrogen
{
    public class PyrogenShield : ModNPC
    {
        public bool stopAi1 = false;
        public override string Texture => "CalRemix/NPCs/Bosses/Pyrogen/PyrogenShield1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrogen's Shield");
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
            NPC pyro = Main.npc[(int)NPC.ai[0]];
            if (pyro.active && pyro.type == ModContent.NPCType<Pyrogen>())
            {
                NPC.Calamity().newAI[0] = pyro.ai[0];
                switch (NPC.Calamity().newAI[0])
                {
                    case 0 or 1 or 2: //default guarding behavior
                        {
                            stopAi1 = false;
                            NPC.localAI[1] += 1f;
                            float distance = 100;
                            distance = pyro.width >= pyro.height ? pyro.width : pyro.height;
                            double deg = 24 * NPC.ai[1] + Main.GlobalTimeWrappedHourly * 420 + NPC.localAI[1];
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
                            break;
                        }
                    case 3: //all players are pulled in with the boss; rotate out further in this case
                        {
                            if (!stopAi1) { 
                                NPC.localAI[1] += 1f;
                            }
                            Vector2 idealpos = NPC.Center;

                            float distance = 300;
                            distance = pyro.width >= pyro.height ? pyro.width : pyro.height;

                            double deg = 22.5 * NPC.ai[1] + Main.GlobalTimeWrappedHourly * 660 + NPC.localAI[1];
                            double rad = deg * (Math.PI / 180);
                            float hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 4;
                            float hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 4;
                            
                            if (NPC.localAI[1] <= 40)
                            {
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                                NPC.damage = 0;
                            }
                            else if (NPC.localAI[1] < 600)
                            {
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                                NPC.damage = 200;
                            }

                            if (NPC.localAI[1] == 700) // repelling back out! returning to original distance...
                            {
                                distance = 100;
                                hyposx = pyro.Center.X - (int)(Math.Cos(rad) * distance) - NPC.width / 2;
                                hyposy = pyro.Center.Y - (int)(Math.Sin(rad) * distance) - NPC.height / 2;
                                NPC.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
                                NPC.damage = 0;
                            }
                            if (pyro.ai[3] == 720) // done! restoring contact damage...
                            {
                                NPC.damage = 220;
                                stopAi1 = true;
                                NPC.localAI[1] = 0;
                            }

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
                            break;
                        }
                    default: //default guarding behavior
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
                        break;
                }
            }
            else
            {
                NPC.StrikeInstantKill();
            }
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