using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.World;
using System;
using CalamityMod.Particles;
using Terraria.Audio;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class YellowEcho : ModNPC
    {
        public ref float Timer => ref NPC.Calamity().newAI[0];

        public ref float State => ref NPC.Calamity().newAI[1];

        public Vector2 Anchor
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[3]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[3] = value.Y;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 140;
            NPC.width = 50;
            NPC.height = 90;
            NPC.defense = 10;
            NPC.lifeMax = 10000;
            NPC.value = 2000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = BetterSoundID.ItemMagicMount;
            NPC.DeathSound = BetterSoundID.ItemBubblePop;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BarrensBiome>().Type };
            NPC.alpha = 50;
            NPC.rarity = 1;
            NPC.chaseable = false;
        }

        public override void AI()
        {
            if (Anchor == Vector2.Zero)
            {
                Anchor = NPC.Center;
            }
            Lighting.AddLight(NPC.Center, 2, 2, 0);
            NPC.TargetClosest(true);
            if (State == 0)
            {
                Timer++;
                if ((Timer % 150 == 0))
                {
                    NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.2f, 1.1f);
                }
                if (NPC.Center.Y < (Anchor.Y - 200) || NPC.Center.Y > (Anchor.Y + 200))
                {
                    NPC.velocity.Y *= -1;
                }
                if (NPC.justHit && State == 0)
                {
                    State = 1;
                    Timer = 0;
                }
                NPC.spriteDirection = NPC.direction = -NPC.velocity.X.DirectionalSign();
            }
            else if (State == 1)
            {
                NPC.chaseable = true;
                Timer++;
                CalamityUtils.SmoothMovement(NPC, 20, Main.player[NPC.target].Center - NPC.Center, 2, 0.1f, true);
                
                if (Timer % 90 > 50)
                {
                    int projAmt = CalamityWorld.death ? 4 : 2;
                    for (int i = 0; i < projAmt; i++)
                    {
                        Vector2 pos = NPC.Center + Vector2.UnitX.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * 200;
                        for (int j = 0; j < 2; j++)
                        {
                            Vector2 partPos = pos + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 20;
                            GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(partPos, partPos.DirectionTo(pos), Main.rand.NextFloat(0.8f, 1f), Color.Yellow, 10, 0.8f));
                        }
                    }
                }
                if (Timer % 90 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int projAmt = CalamityWorld.death ? 4 : 2;
                        SoundEngine.PlaySound(BetterSoundID.ItemManaCrystal with { Pitch = 0.8f }, NPC.Center);
                        for (int i = 0; i < projAmt; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + Vector2.UnitX.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * 200, Vector2.Zero, ModContent.ProjectileType<LightOrb>(), CalRemixHelper.ProjectileDamage(80, 120), 1, ai0: NPC.target);
                        }
                    }
                }
                NPC.spriteDirection = -NPC.direction;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "Glow").Value;
            float scale = MathF.Sin(Main.GlobalTimeWrappedHourly) * 0.02f;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, Color.White * NPC.Opacity, NPC.rotation, tex.Size() / 2, NPC.scale + scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            for (int i = 0; i < 5; i++)
            spriteBatch.Draw(glow, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, tex.Size() / 2, NPC.scale + scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.ExitShaderRegion();

            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<LightResidue>(), 1, 15, 35);
            npcLoot.Add(ModContent.ItemType<Mikado>());
        }
    }
}
