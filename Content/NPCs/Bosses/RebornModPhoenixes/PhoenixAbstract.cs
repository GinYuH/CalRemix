using CalamityMod;
using CalamityMod.Items.Placeables.Ores;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using tModPorter;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes
{
    public abstract class PhoenixAbstract : ModNPC
    {
        public virtual int damage => 30;
        public virtual int defense => 10;
        public virtual int health => 9000;
        public virtual int projType => ModContent.ProjectileType<BrimstoneBall>();
        public virtual int dustType => DustID.Torch;
        public virtual int invulThreshold => 600;

        public Vector2 targetPos;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 10;
            NPC.aiStyle = -1;
            NPC.width = 100;
            NPC.height = 126;
            NPC.damage = 30;
            NPC.defense = 18;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 9000;
            NPC.scale = 1.4f;
            NPC.friendly = false;
            NPC.boss = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.value = 200000;
            NPC.behindTiles = false;

            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = true;
        }

        public void ExtraAI()
        {
            //Main.NewText("ai 0: " + NPC.ai[0]);
            //Main.NewText("ai 1: " + NPC.ai[1]);
            //Main.NewText("ai 2: " + NPC.ai[2]);
            //Main.NewText("ai 3: " + NPC.ai[3]);
        }

        public virtual void ShootProjectiles()
        {
            Vector2 vector8 = targetPos;
            float num48 = 50f;
            int damage = 14;
            int type = projType;
            SoundEngine.PlaySound(SoundID.Item17, vector8);
            float rotation = (float)Math.Atan2(vector8.Y - 80 - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y - 80, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
            Main.projectile[num54].timeLeft = 300;
            num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y - 80, (float)((Math.Cos(rotation + 0.4) * num48) * -1), (float)((Math.Sin(rotation + 0.4) * num48) * -1), type, damage, 0f, Main.myPlayer);
            Main.projectile[num54].timeLeft = 300;
            num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y - 80, (float)((Math.Cos(rotation - 0.4) * num48) * -1), (float)((Math.Sin(rotation - 0.4) * num48) * -1), type, damage, 0f, Main.myPlayer);
            Main.projectile[num54].timeLeft = 300;
        }

        public override void AI()
        {
            NPC.netUpdate = true;
            NPC.ai[2]++;
            NPC.ai[1]++;
            if (NPC.ai[0] > 0) NPC.ai[0] -= invulThreshold * 0.002f; // defaults to 1.2 w/ 600
            if (NPC.ai[0] < 0) NPC.ai[0] = 0;
            targetPos = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
            Vector2 vector8 = targetPos;
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Main.player[NPC.target].position.Y > (Main.maxTilesY - 250) * 16)
            {
                NPC.TargetClosest(true);
            }
            Color color = new Color();
            int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, dustType, NPC.velocity.X, NPC.velocity.Y, 200, color, 0.5f + (NPC.ai[0] / invulThreshold * 600) / 75);
            Main.dust[dust].noGravity = true;

            ExtraAI();

            if (NPC.ai[3] == 0)
            {
                NPC.alpha = 0;
                NPC.dontTakeDamage = false;
                NPC.damage = 30;
                if (NPC.ai[2] < 600)
                {
                    if (Main.player[NPC.target].position.X < vector8.X)
                    {
                        if (NPC.velocity.X > -8) { NPC.velocity.X -= 0.22f; }
                    }
                    if (Main.player[NPC.target].position.X > vector8.X)
                    {
                        if (NPC.velocity.X < 8) { NPC.velocity.X += 0.22f; }
                    }

                    if (Main.player[NPC.target].position.Y < vector8.Y + 300)
                    {
                        if (NPC.velocity.Y > 0f) NPC.velocity.Y -= 0.8f;
                        else NPC.velocity.Y -= 0.07f;
                    }
                    if (Main.player[NPC.target].position.Y > vector8.Y + 300)
                    {
                        if (NPC.velocity.Y < 0f) NPC.velocity.Y += 0.8f;
                        else NPC.velocity.Y += 0.07f;
                    }

                    if (NPC.ai[1] >= 0 && NPC.ai[2] > 120 && NPC.ai[2] < 600)
                    {
                        ShootProjectiles();
                        NPC.ai[1] = -90;
                    }
                }
                else if (NPC.ai[2] >= 600 && NPC.ai[2] < 1200)
                {
                    NPC.velocity.X *= 0.98f;
                    NPC.velocity.Y *= 0.98f;
                    if ((NPC.velocity.X < 2f) && (NPC.velocity.X > -2f) && (NPC.velocity.Y < 2f) && (NPC.velocity.Y > -2f))
                    {
                        float rotation = (float)Math.Atan2((vector8.Y) - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), (vector8.X) - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        NPC.velocity.X = (float)(Math.Cos(rotation) * 25) * -1;
                        NPC.velocity.Y = (float)(Math.Sin(rotation) * 25) * -1;
                    }
                }
                else NPC.ai[2] = 0;
            }
            else
            {
                NPC.ai[3]++;
                NPC.alpha = 200;
                NPC.damage = 10;
                NPC.dontTakeDamage = true;
                if (Main.player[NPC.target].position.X < vector8.X)
                {
                    if (NPC.velocity.X > -6) { NPC.velocity.X -= 0.22f; }
                }
                if (Main.player[NPC.target].position.X > vector8.X)
                {
                    if (NPC.velocity.X < 6) { NPC.velocity.X += 0.22f; }
                }
                if (Main.player[NPC.target].position.Y < vector8.Y)
                {
                    if (NPC.velocity.Y > 0f) NPC.velocity.Y -= 0.8f;
                    else NPC.velocity.Y -= 0.07f;
                }
                if (Main.player[NPC.target].position.Y > vector8.Y)
                {
                    if (NPC.velocity.Y < 0f) NPC.velocity.Y += 0.8f;
                    else NPC.velocity.Y += 0.07f;
                }
                if (NPC.ai[3] == 100)
                {
                    NPC.ai[3] = 1;
                    float healAmt = health * (1 / 36f); // defaults to 250 with default hp value 9000
                    NPC.life += (int)healAmt;

                    // this isnt part of the og, but im adding it for player visibility
                    NPC.HealEffect((int)healAmt);

                    if (NPC.life > NPC.lifeMax) NPC.life = NPC.lifeMax;
                }
                if (NPC.ai[1] >= 0)
                {
                    NPC.ai[3] = 0;
                    for (int num36 = 0; num36 < 40; num36++)
                    {
                        Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, dustType, 0, 0, 0, color, 3f);
                    }
                }
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y -= 0.04f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                    return;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int num = 1;
            if (!Main.dedServ)
            {
                num = TextureAssets.Npc[Type].Value.Height / Main.npcFrameCount[NPC.type];
            }
            if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
            NPC.rotation = NPC.velocity.X * 0.08f;
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter >= 4.0)
            {
                NPC.frame.Y = NPC.frame.Y + num;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= num * Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = 0;
            }
            if (NPC.ai[3] == 0)
            {
                NPC.alpha = 0;
            }
            else
            {
                NPC.alpha = 200;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            NPC.ai[0] += (float)hit.Damage;
            if (NPC.ai[0] > invulThreshold)
            {
                NPC.ai[3] = 1;
                Color color = new Color();
                for (int num36 = 0; num36 < 50; num36++)
                {
                    int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, DustID.TintableDust, 0, 0, 100, color, 3f);
                }
                for (int num36 = 0; num36 < 20; num36++)
                {
                    int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, dustType, 0, 0, 100, color, 3f);
                }
                NPC.ai[1] = -300;
                NPC.ai[0] = 0;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
