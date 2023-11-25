using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Sounds;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;

namespace CalRemix.NPCs
{
    public class WulfrumMine : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public ref float Turn => ref NPC.ai[2];
        public bool Supercharged => NPC.life <= NPC.lifeMax / 2;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<KamiFlu>()] = true;
            Main.npcFrameCount[Type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 30;
            NPC.lifeMax = 350;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(silver: 50, copper: 50);
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = CommonCalamitySounds.WulfrumNPCDeathSound;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
        }
        public override void AI()
        {
            if (Supercharged)
            {
                if (State < 4)
                {
                    Timer = 0;
                    State = 5;
                }
                NPC.rotation += 0.066f;
                Timer++;
                if (Timer > 300)
                {
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ProjectileID.ClusterRocketI, NPC.damage * 2, 0f);
                    proj.friendly = false;
                    proj.hostile = true;
                    proj.DamageType = DamageClass.Default;
                    proj.Kill();
                    NPC.StrikeNPC(hit: NPC.CalculateHitInfo(NPC.life, NPC.direction));
                }
                if (Timer % 9 == 0 && Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 shoot = NPC.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90) * i) * 12f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + shoot, shoot, ProjectileID.SaucerLaser, NPC.damage, 0f);
                    }
                }
            }
            else
            {
                if (NPC.rotation < MathHelper.ToRadians(45) && State == 0)
                    NPC.rotation += 0.022f;
                else if (NPC.rotation >= MathHelper.ToRadians(45) && State == 0)
                    State = 1;
                if (NPC.rotation > 0 && State == 2)
                    NPC.rotation -= 0.022f;
                else if (NPC.rotation <= 0 && State == 2)
                    State = 3;
                if (State == 1)
                {
                    Timer++;
                    if (Timer > 60)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2 shoot = NPC.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90) * i) * 12f;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + shoot, shoot, ProjectileID.SaucerLaser, NPC.damage, 0f);
                            }
                        }
                        State = 2;
                        Timer = 0;
                    }
                }
                if (State == 3)
                {
                    Timer++;
                    if (Timer > 60)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2 shoot = NPC.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90) * i) * 12f;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + shoot, shoot, ProjectileID.SaucerLaser, NPC.damage, 0f);
                            }

                        }
                        State = 0;
                        Timer = 0;
                    }
                }
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Timer > 300 && State >= 5 && Main.rand.NextBool(5))
                Dust.NewDust(NPC.Center, 1, 1, DustID.Smoke, SpeedY: -4f, newColor: Color.DarkGray);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, hit.HitDirection, -1f);
            if (NPC.life > 0)
                return;
            for (int j = 0; j < 15; j++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, hit.HitDirection, -1f);
            if (!Main.dedServ)
            {
                int num = Main.rand.Next(1, 4);
                for (int k = 0; k < num; k++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("WulfrumEnemyGore" + Main.rand.Next(1, 11)).Type);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * (Supercharged ? 1 : 0);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A prototypical project, abandoned before its completion, the Wulfrum Mine was originally designed to perform the functions of each of the other Wulfrum constructs. However, the only thing it truly proved adept at was violently exploding, vaporizing whatever it caught in its radius.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur || !spawnInfo.Player.ZoneDirtLayerHeight)
                return 0f;
            return SpawnCondition.Underground.Chance * (Main.hardMode ? 0.01f : 0.1f) * (NPC.AnyNPCs(ModContent.NPCType<WulfrumAmplifier>()) ? 5.5f : 1f);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<EnergyCore>(), 1, 5, 5);
            npcLoot.Add(ModContent.ItemType<WulfrumMetalScrap>(), 1, 10, 15);
            npcLoot.Add(ModContent.ItemType<WulfrumBattery>(), new Fraction(7, 100));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle rect = new(0, NPC.frame.Y, texture.Width, texture.Height / 2);
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), rect, Color.Lerp(Color.White, Color.Red, Utils.GetLerpValue(0, 300, Timer, true)), NPC.rotation, rect.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
