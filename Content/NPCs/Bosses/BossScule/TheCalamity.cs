using CalamityMod.World;
using CalamityMod;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Lore;
using Terraria.GameContent.Bestiary;
using System.IO;

namespace CalRemix.Content.NPCs.Bosses.BossScule
{
    [AutoloadBossHead]
    public class TheCalamity : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        private float EndTime = Main.zenithWorld? 6000 : 4800;
        private float Attack1 = Main.zenithWorld ? 20f : 60f;
        private float Attack2 = Main.zenithWorld ? 80f : 180f;
        private float Attack3 = Main.zenithWorld ? 70f : 120f;
        private float Attack4 = Main.zenithWorld ? 90f : 180f;

        public Player Target => Main.player[NPC.target];
        public override bool CheckActive() => AllDead;
        public bool AllDead = true;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.damage = 220;
            NPC.width = 128;
            NPC.height = 128;
            NPC.defense = 130;
            NPC.lifeMax = 22;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.canGhostHeal = false;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = null;
            NPC.value = 0;
            NPC.netAlways = true;
            NPC.dontTakeDamage = true;
            NPC.alpha = 0;
            Music = CalRemixMusic.TheCalamity;

        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AllDead);
            writer.Write(NPC.dontTakeDamage);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AllDead = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value), 
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement()]);
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC.lifeMax = 22;
            NPC.life = 22;
            if (!Main.dedServ)
            {
                Music = CalRemixMusic.TheCalamity;
                Main.newMusic = Music;
                Main.musicFade[Main.curMusic] = 0f;
                Main.musicFade[Main.newMusic] = 1f;
            }
            if (Target.whoAmI < 0 || Target.whoAmI == 255 || Target.dead || !Target.active)
            {
                NPC.TargetClosest();
                NPC.Center = new Vector2(Target.Center.X, Target.Center.Y - 256);
            }
            foreach (Player p in Main.player)
            {
                if (p.active)
                    p.Remix().calamitizedCounter = CalamitySetCounter();
            }
        }
        public override void AI()
        {
            if (Target.DeadOrGhost || Target.HasBuff(ModContent.BuffType<Calamitized>()))
                NPC.TargetClosest();
            AllDead = true;
            foreach (Player p in Main.player)
            {
                if (p.active && !p.dead && !p.HasBuff(ModContent.BuffType<Calamitized>()))
                {
                    AllDead = false;
                    NPC.netUpdate = true;
                }
            }
            if (AllDead)
            {
                NPC.velocity.X += 1.1f * ((Target.Center.X > NPC.Center.X) ? -1 : 1);
                NPC.EncourageDespawn(10);
                return;
            }
            NPC.direction = (Target.Center.X < NPC.Center.X) ? -1 : 1;
            NPC.spriteDirection = NPC.direction;
            switch (State)
            {
                case 0:
                    if (NPC.alpha < 255)
                        NPC.alpha++;
                    else
                        Timer++;
                    if (Timer == 60)
                        Talk("1");
                    if (Timer >= 180)
                    {
                        Timer = 0;
                        State = 1;
                    }
                    break;
                case 1:
                    if (NPC.Distance(Target.Center) > 1120f)
                        Teleport(new Vector2(Target.Center.X, Target.Center.Y - 256));
                    if (Timer <= EndTime + 180)
                    {
                        Timer++;
                    }
                    if (Timer == 1200)
                        Talk("2");
                    if (Timer == 2400)
                        Talk("3");
                    if (Main.zenithWorld && Timer == 4500)
                        Talk("4");
                    if (Timer == 3600)
                    {
                        if (!Main.zenithWorld)
                            Talk("4");
                        else
                            Talk("5");
                    }
                    if (Main.zenithWorld && Timer == 4500)
                        Talk("6");
                    if (Timer == EndTime)
                    {
                        foreach (Projectile p in Main.projectile)
                        {
                            if (p.type == ModContent.ProjectileType<CalamityLaser>() || p.type == ModContent.ProjectileType<DarkVein>() || p.type == ModContent.ProjectileType<Darkscule>())
                                p.active = false;
                        }
                        if (!Main.zenithWorld)
                            Talk("5");
                        else
                            Talk("7");
                        NPC.dontTakeDamage = false;
                        NPC.netUpdate = true;
                    }
                    if (Timer == EndTime + 180)
                    {
                        if (!Main.zenithWorld)
                            Talk("6");
                        else
                            Talk("8");
                    }
                    if (Timer < 2400 && Timer % Attack1 == 0 && Main.netMode != NetmodeID.Server)
                    {
                        Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 600f;
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Main.zenithWorld? pos.DirectionTo(Target.Center) * 9f : pos.DirectionTo(Target.Center) * 4.5f, ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
                        if (Main.zenithWorld) for (int i = 0; i < 3; i++) Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 14f, ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
                    }
                    if (Timer >= 1200 && Timer < 3600 && Timer % Attack2 == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; Main.zenithWorld? i < 4 : i < 2; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 400f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<DarkVein>(), 0, 0);
                        }

                    }
                    if (Timer >= 2400 && Timer < EndTime && Timer % Attack3 == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; Main.zenithWorld ? i < 4 : i < 3; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 800f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Main.zenithWorld ? pos.DirectionTo(Target.Center) * 88f : pos.DirectionTo(Target.Center) * 44f, ModContent.ProjectileType<CalamityLaser>(), 0, 0);
                        }
                    }
                    if (Timer >= 3600 && Timer < EndTime && Timer % Attack4 == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; Main.zenithWorld ? i < 6 : i < 2; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 600f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Main.zenithWorld ? pos.DirectionTo(Target.Center) * 8.8f : pos.DirectionTo(Target.Center) * 2.2f, ModContent.ProjectileType<Darkscule>(), 0, 0);
                        }
                    }
                    if (Timer >= 4500 && Timer < EndTime && Timer % 100f == 0 && Main.netMode != NetmodeID.Server)
                    {
                        Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 600f;
                        for (int i = 0; i < 5; i++)
                        {
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 88f, ModContent.ProjectileType<CalamityLaser>(), 0, 0);

                        }
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 8.8f, ModContent.ProjectileType<Darkscule>(), 0, 0);
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<DarkVein>(), 0, 0);
                    }

                    break;
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        private void Teleport(Vector2 pos)
        {
            NPC.Center = pos;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Target.HasItem(ModContent.ItemType<Ogscule>()) ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossScule/TheCalamityWorm").Value : TextureAssets.Npc[Type].Value;
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
            SpriteEffects spriteEffects = (NPC.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, draw, sourceRectangle, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            Texture2D eye = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossScule/CalamityEye").Value;
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY) + (Target.HasItem(ModContent.ItemType<Ogscule>()) ? Vector2.UnitY * NPC.height * -0.65f : Vector2.Zero), null, new Color(255, 0, 0, 255), NPC.rotation, eye.Size() / 2f, NPC.scale * 0.2f, SpriteEffects.None, 0f);
            return false;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Timer > 0 && State < 1 || State >= 1)
            {
                if (!Terraria.Graphics.Effects.Filters.Scene["BloodMoon"].IsActive())
                {
                    Terraria.Graphics.Effects.Filters.Scene.Activate("BloodMoon", Main.player[Main.myPlayer].position);
                }
                else
                    Terraria.Graphics.Effects.Filters.Scene["BloodMoon"].GetShader().UseIntensity(2 / 255f * NPC.alpha);
            }
        }
        public override void OnKill()
        {
            RemixDowned.downedCalamity = true;
            CalRemixWorld.UpdateWorldBool();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TheCalamityBag>()));
            npcLoot.AddNormalOnly(ItemDropRule.Common(ModContent.ItemType<CalamitousCertificate>()));
            npcLoot.AddNormalOnly(ItemDropRule.Common(ModContent.ItemType<InfraredSights>()));
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<CalamityRelic>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedCalamity, ModContent.ItemType<KnowledgeCalamity>(), desc: DropHelper.FirstKillText);
        }
        private static void Talk(string value)
        {
            string s = Main.zenithWorld ? "GFB" : string.Empty;
            CalRemixHelper.GetNPCDialog($"TheCalamity.{s}{value}", Color.Red);
        }
        internal static int CalamitySetCounter()
        {
            int counter = 3;
            if (CalamityWorld.death || Main.getGoodWorld)
                counter = 0;
            if (Main.zenithWorld) // may god have mercy on your soul
                counter = 6;
            else if (CalamityWorld.revenge)
                counter = 1;
            else if (Main.expertMode)
                counter = 2;
            return counter;
        }
    }
}
