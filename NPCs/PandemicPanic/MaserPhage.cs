using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Projectiles.Hostile;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.World;
using CalRemix.Biomes;

namespace CalRemix.NPCs.PandemicPanic
{
    public class MaserPhage : ModNPC
    {
        Entity target = null;

        public ref float Phase => ref NPC.ai[0];
        public ref float FireRate => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maser Phage");
            Main.npcFrameCount[NPC.type] = 18;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 1f;
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.width = 54; //324
            NPC.height = 80; //216
            NPC.defense = 22;
            NPC.lifeMax = 10000;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.scale = 4f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void AI()
        {
            if (target == null || !target.active)
            {
                target = PandemicPanic.BioGetTarget(false, NPC);
            }
            float speed = 5f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            bool slowDown = false;
            if (target != null && target.active && !(target is NPC ne && ne.life <= 0))
            {
                NPC.direction = target.position.X - NPC.position.X > 0 ? 1 : -1;
                if (Phase == 0f)
                {
                    FireRate += 1f;
                    if (FireRate >= 300f && Main.netMode != 1)
                    {
                        FireRate = 0f;
                        Phase = Main.rand.NextBool(3) ? 2 : 1;
                        NPC.netUpdate = true;
                    }
                }
                else if (Phase == 1f)
                {
                    slowDown = true;
                    FireRate += 1f;
                    if (FireRate % 5f == 0f)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center) * 8, ProjectileID.DeathLaser, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer);
                    }
                    if (FireRate >= 180f)
                    {
                        FireRate = 0f;
                        Phase = 0f;
                    }
                }
                else if (Phase == 2f)
                {
                    {
                        slowDown = true;
                        FireRate += 1f;
                        if (FireRate == 22)
                        {
                            SoundEngine.PlaySound(AresLaserCannon.TelSound, NPC.Center);
                            int amt = CalamityWorld.death ? 5 : CalamityWorld.revenge ? 4 : Main.expertMode ? 3 : 1;
                            for (int i = 0; i < amt; i++)
                            {
                                Vector2 dir = NPC.DirectionTo(target.Center).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, (float)(((float)i + 1f) / (float)amt)) + 0.01f);
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir, ModContent.ProjectileType<MaserDeathray>(), (int)(NPC.damage * 0.75f), 0f);
                                Main.projectile[p].ModProjectile<MaserDeathray>().NPCOwner = NPC.whoAmI;
                            }
                        }
                        if (FireRate >= 300f)
                        {
                            FireRate = 0f;
                            Phase = 0f;
                        }
                        NPC.velocity.X = 0;
                    }
                }
            }
            bool valid = target != null && target.active && !(target is NPC nee && nee.life <= 0);
            if (Phase != 2f)
            {
                if (!valid)
                {
                    NPC.velocity.X *= 0.9f;
                    if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                    {
                        NPC.velocity.X = 0f;
                    }
                }
                else
                {
                    if (NPC.direction > 0)
                    {
                        NPC.velocity.X = (NPC.velocity.X * 20f + speed) / 21f;
                    }
                    if (NPC.direction < 0)
                    {
                        NPC.velocity.X = (NPC.velocity.X * 20f - speed) / 21f;
                    }
                }
            }
            int width = 80;
            int height = 20;
            Vector2 collisionTile = new Vector2(NPC.Center.X - (float)(width / 2), NPC.position.Y + (float)NPC.height - (float)height);
            bool fall = false;
            if (valid && NPC.position.X < target.position.X && NPC.position.X + (float)NPC.width > target.position.X + (float)target.width && NPC.position.Y + (float)NPC.height < target.position.Y + (float)target.height - 16f)
            {
                fall = true;
            }
            if (fall)
            {
                NPC.velocity.Y += 0.5f;
            }
            else if (Collision.SolidCollision(collisionTile, width, height))
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y > -0.2)
                {
                    NPC.velocity.Y -= 0.025f;
                }
                else
                {
                    NPC.velocity.Y -= 0.2f;
                }
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
            }
            else
            {
                if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y < 0.1)
                {
                    NPC.velocity.Y += 0.025f;
                }
                else
                {
                    NPC.velocity.Y += 0.5f;
                }
            }
            if (NPC.velocity.Y > 10f)
            {
                NPC.velocity.Y = 10f;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X == 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
            else
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 11)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Equipped with powerful laser capabilities, these towering constructs relentlessly destroy large amounts of immune cells at once.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            Color color = NPC.GetAlpha(Color.Red * 0.2f);
            float scale = NPC.scale;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 8 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().phd)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Gray").Value, position, NPC.frame, Color.Red, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TheDestroyer, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TheDestroyer, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
