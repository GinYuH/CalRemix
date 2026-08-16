using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI;
using CalRemix.Content.Items.Misc;
using CalamityMod;
using Terraria.ID;
using CalamityMod.Sounds;
using CalRemix.Core.Subworlds;
using CalamityMod.Particles;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Coffargus : DialogueNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/Coffargus") with { PitchVariance = 0.75f };

        public Vector2 asbestosLocation
        {
            get => new Vector2(NPC.ai[2], NPC.ai[3]);
            set
            {
                NPC.ai[2] = value.X;
                NPC.ai[3] = value.Y;
            }
        }

        public override SoundStyle TextSound => talkSound;

        public override int TextSpeed => 6;

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 60;
            NPC.lifeMax = 100;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = Carcinogen.HitSound;
            NPC.DeathSound = Carcinogen.DeathSound;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            NPC.dontTakeDamageFromHostiles = true;
        }

        public override string GetDialogue()
        {
            if (State == 1)
                return "Repeat";
            return "Dialogue";
        }

        public override void OnEnd(string key)
        {
            State = 1;
        }

        public override void AI()
        {
            base.AI();
            NPC.TargetClosest(false);
            NPC.spriteDirection = NPC.direction;
            Timer++;
            if (RemixDowned.downedCarcinogen)
                NPC.active = false;
            if (asbestosLocation == Vector2.Zero && CalRemixWorld.postGenUpdate)
            {
                int bestos = ModContent.TileType<AsbestosPlaced>();
                bool sb = false;
                bool left = (NPC.Center.X / 16) < (Main.maxTilesX / 2);
                if (left)
                {
                    for (int i = 0; i < Main.maxTilesX; i += 20)
                    {
                        if (sb)
                            break;
                        for (int j = (int)NPC.Bottom.Y / 16; j < Main.UnderworldLayer; j += 20)
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (t.TileType == bestos)
                            {
                                asbestosLocation = new Vector2(i * 16, j * 16);
                                sb = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = Main.maxTilesX; i > 0; i -= 20)
                    {
                        if (sb)
                            break;
                        for (int j = (int)NPC.Bottom.Y / 16; j < Main.UnderworldLayer; j += 20)
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (t.TileType == bestos && t.HasTile)
                            {
                                asbestosLocation = new Vector2(i * 16, j * 16);
                                sb = true;
                                break;
                            }
                        }
                    }
                }
            }
            else if (asbestosLocation != Vector2.Zero && Timer % 120 == 0 && !CalamityPlayer.areThereAnyDamnBosses && State == 1)
            {
                Vector2 pos = NPC.Bottom + new Vector2(40, -40);
                int iterAmt = 500;
                Vector2 top = pos;
                Vector2 bottom = Vector2.Zero;
                for (int i = 0; i < 500; i++)
                {
                    Vector2 pt = Vector2.Lerp(pos, asbestosLocation, i / 499f);
                    if (pt.Y < Main.LocalPlayer.Center.Y - Main.screenHeight)
                    {
                        top = pt;
                    }
                    if (pt.Y > Main.LocalPlayer.Center.Y + Main.screenHeight && bottom == Vector2.Zero)
                    {
                        bottom = pt;
                    }
                }
                SoundEngine.PlaySound(BetterSoundID.ItemMissileFireSqueak, NPC.Center);
                for (int i = 0; i < 100; i++)
                {
                    Dust d = Dust.NewDustPerfect(Vector2.Lerp(top, bottom, i / 99f), DustID.Smoke, Vector2.Zero, Scale: 3);
                    Dust d2 = Dust.NewDustPerfect(Vector2.Lerp(top, bottom, i / 99f), DustID.InfernoFork, Vector2.Zero, Scale: 0.7f);
                    d.noGravity = true;
                    d2.noGravity = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D face = ModContent.Request<Texture2D>(Texture + "_Face").Value;
            Texture2D hand = ModContent.Request<Texture2D>(Texture + "_Hand").Value;

            float eyeOff = MathHelper.Lerp(-4, 4, Utils.GetLerpValue(NPC.Left.X, NPC.Right.X, Target.Center.X, true));

            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float speed = 10f;
            float eyeoff = 0;
            Vector2 scale = Vector2.One + new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * speed), MathF.Sin(Main.GlobalTimeWrappedHourly * speed)) * 0.025f;
            if (!NPCDialogueUI.IsBeingTalkedTo(NPC))
                scale = Vector2.One;
            else
                eyeoff = MathF.Sin(Main.GlobalTimeWrappedHourly * 10) * 6;

            spriteBatch.Draw(texture, NPC.Bottom - screenPos, null, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            spriteBatch.Draw(face, NPC.Bottom - screenPos + Vector2.UnitY * eyeoff, null, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            Vector2 handPos = NPC.Bottom + eyeOff * Vector2.UnitX + new Vector2(40, -40);
            if (State == 1)
                spriteBatch.Draw(hand, handPos - screenPos, null, drawColor, handPos.DirectionTo(asbestosLocation).ToRotation(), new Vector2(0, hand.Height / 2), scale * NPC.scale, fx, 0f);
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<RustedShardProjectile>())
                return true;
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Asbestos>());
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}
