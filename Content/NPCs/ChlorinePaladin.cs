using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Critters;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs;
using Terraria.Audio;
using CalamityMod.Events;
using CalamityMod.World;
using CalamityMod.NPCs.SlimeGod;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;

namespace CalRemix.Content.NPCs
{
    [AutoloadBossHead]
    public class ChlorinePaladin : ModNPC
    {
        public ref float AITimer => ref NPC.ai[0];
        public ref float AIMode => ref NPC.ai[1];
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.damage = 200;
            NPC.width = 150;
            NPC.height = 92;
            NPC.scale = 1.1f;
            NPC.defense = 12;
            NPC.LifeMaxNERB(7500, 9000, 160000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            NPC.knockBackResist = 0f;
            NPC.value = 0f;
            NPC.Opacity = 0.8f;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            AITimer = 0;
            AIMode = 1;
        }
        public override void AI()
        {
            bool bossRush = BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool death = CalamityWorld.death || NPC.localAI[1] == 1f || bossRush;

            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            // Despawn safety, make sure to target another player if the current player target is too far away
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 9600)
                NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            if (lifeRatio <= 0.5f && Main.netMode != NetmodeID.MultiplayerClient && expertMode)
            {

                SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);
                Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X - 30, (int)spawnAt.Y, ModContent.NPCType<SplitCrimulanPaladin>());
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X + 30, (int)spawnAt.Y, ModContent.NPCType<SplitCrimulanPaladin>());

                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
                return;

            }

            // jump if it is time
            if (AIMode == 0 && AITimer <= 0)
            {
                NPC.velocity = new Vector2(0, -45f);
                AITimer = 50;
                AIMode = 1;
            }
            // slow down over time to create a slightly nicer effect
            else if (AIMode == 1 && AITimer >= 0)
            {
                NPC.velocity.Y *= 0.97f;
            }
            // keep jumping until offscreen and it is time
            else if (AIMode == 1 && AITimer <= 0 && Vector2.Distance(NPC.Center, player.Center) < 800)
            {
                NPC.velocity = new Vector2(0, -45f);
                AITimer = 50;
            }
            // teleport if it is time
            else if (AIMode == 1 && AITimer <= 0 && Vector2.Distance(NPC.Center, player.Center) > 800)
            {
                NPC.velocity = new Vector2(0, 1);
                float awesomeNewPosition = player.position.X + Main.rand.Next(-750, 750);
                NPC.position.X = awesomeNewPosition;
                AIMode = 2;
                NPC.noGravity = true;
            }
            // fall
            else if (AIMode == 2 && NPC.velocity.Y > 0)
            {
                NPC.velocity.Y += 1.3f;

                // the evil... the EVIL IN ME.... GRAAAAAAAGHHHHHHHH I CANNOT STOP IT 
                Particle smoke = new HeavySmokeParticle(NPC.Center, NPC.velocity * Main.rand.NextFloat(-0.2f, -0.6f), Color.DarkCyan * 0.65f, 22, Main.rand.NextFloat(2.4f, 2.55f), 0.3f, Main.rand.NextFloat(-0.2f, 0.2f), false, required: true);
                GeneralParticleHandler.SpawnParticle(smoke);
            }
            // slam and restart
            // NPC.oldPosition == NPC.position is an evil hack to see if the npc is standing still
            else if (AIMode == 2 && NPC.oldPosition.Y == NPC.position.Y)
            {
                NPC.noGravity = false;
                SoundEngine.PlaySound(SoundID.AbigailCry, NPC.Center);
                for (int ii = 0; ii < 22; ii++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X - NPC.width / 2, NPC.position.Y + (NPC.height / 3) * 2), NPC.width * 2, NPC.height / 3, DustID.BlueCrystalShard);
                    dust.noGravity = false;
                    dust.velocity *= 5f;
                    dust.scale *= 4.0f;
                }
                Particle pulse = new DirectionalPulseRing(NPC.Center + new Vector2(0, NPC.height / 2), Vector2.Zero, Color.DarkCyan, new Vector2(4f, 2f), Main.rand.NextFloat(-0.2f, 0.2f), 0.01f, 1.5f, 22);
                GeneralParticleHandler.SpawnParticle(pulse);

                AIMode = 0;
                // gets faster at low hp
                AITimer = Main.rand.Next(200, 400) * lifeRatio;
            }

            AITimer--;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            ReLogic.Content.Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            float velocityClamped = NPC.velocity.Y * 0.2f;
            if (velocityClamped < 0.75 && velocityClamped > -0.75)
            {
                velocityClamped = 1;
            }
            float velocityClampedEvil = NPC.velocity.Y * 0.2f;
            if (velocityClampedEvil < 0.75 && velocityClampedEvil > -0.75)
            {
                velocityClampedEvil = 1;
            }
            else
            {
                velocityClampedEvil *= -1;
            }

            Main.EntitySpriteDraw(texture.Value, NPC.Bottom - Main.screenPosition, null, Color.White * .8f, NPC.rotation, texture.Size() * new Vector2(0.5f, 1f), NPC.scale * new Vector2(Utils.Clamp(velocityClamped, 0.5f, 1.5f), Utils.Clamp(velocityClampedEvil, 0.5f, 1.5f)), 0, 0);
            return false;
        }
    }
}

