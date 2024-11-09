using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.World;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.BossScule
{
    [AutoloadBossHead]
    public class TheCalamity : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        private const float EndTime = 4800;
        public Player Target => Main.player[NPC.target];
        public override bool CheckActive() => Target.HasBuff(ModContent.BuffType<Calamitized>());
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
        }
        public override void AI()
        {
            if (Target.whoAmI < 0 || Target.whoAmI == 255 || Target.dead || !Target.active)
                NPC.TargetClosest();
            if (Target.HasBuff(ModContent.BuffType<Calamitized>()))
            {
                NPC.velocity.X += 1.1f * ((Target.Center.X > NPC.Center.X) ? -1 : 1);
                NPC.EncourageDespawn(10);
                return;
            }
            switch (State)
            {
                case 0:
                    if (NPC.alpha < 255)
                        NPC.alpha++;
                    else
                        Timer++;
                    string og = string.Empty;
                    if (Timer == 0 && Target.HasItem(ModContent.ItemType<Ogscule>()))
                        og = "Is that... me? Anyway, ";
                    if (Timer == 60)
                        Talk(og + "I will test your abilities. Good Luck.", Color.Red);
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
                        Talk("Your dodging skills are proficient, but how will you fare against the ones after me?", Color.Red);
                    if (Timer == 2400)
                        Talk("Your foes will often hold predictable patterns; see past them and strike.", Color.Red);
                    if (Timer == 3600)
                        Talk("Keep avoiding anything that seems dangerous.", Color.Red);
                    if (Timer == EndTime)
                    {
                        foreach (Projectile p in Main.projectile)
                        {
                            if (p.type == ModContent.ProjectileType<CalamityLaser>() || p.type == ModContent.ProjectileType<DarkVein>() || p.type == ModContent.ProjectileType<Darkscule>())
                                p.active = false;
                        }
                        Talk("You have passed the test. Well done.", Color.Red);
                        NPC.dontTakeDamage = false;
                    }
                    if (Timer == EndTime + 180)
                        Talk("Strike me down and take your reward.", Color.Red);
                    if (Timer < 2400 && Timer % 60f == 0 && Main.netMode != NetmodeID.Server)
                    {
                        Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 600f;
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 4.5f, ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
                    }
                    if (Timer >= 1200 && Timer < 3600 && Timer % 180f == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 400f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<DarkVein>(), 0, 0);
                        }
                    }
                    if (Timer >= 2400 && Timer < EndTime && Timer % 120f == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 800f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 44f, ModContent.ProjectileType<CalamityLaser>(), 0, 0);
                        }
                    }
                    if (Timer >= 3600 && Timer < EndTime && Timer % 180f == 0 && Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 pos = Target.Center - Main.rand.NextVector2Unit().RotatedByRandom(MathHelper.ToRadians(360f)) * 600f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Target.Center) * 2.2f, ModContent.ProjectileType<Darkscule>(), 0, 0);
                        }
                    }
                    break;
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        private void Teleport(Vector2 pos)
        {
            NPC.Center = pos;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, draw, sourceRectangle, new Color(255, 0, 0, NPC.alpha), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossScule/CalamityGlow", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(glow, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 0, 0, NPC.alpha), NPC.rotation, glow.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            Texture2D eye = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossScule/CalamityEye", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(eye, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 0, 0, NPC.alpha), NPC.rotation, eye.Size() / 2f, NPC.scale * 0.2f, SpriteEffects.None, 0f);
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (!Terraria.Graphics.Effects.Filters.Scene["BloodMoon"].IsActive())
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("BloodMoon", Main.player[Main.myPlayer].position);
            }
            Terraria.Graphics.Effects.Filters.Scene["BloodMoon"].GetShader().UseIntensity(2/255f * NPC.alpha);
        }
        public override void OnKill()
        {
            RemixDowned.downedCalamity = true;
            CalRemixWorld.UpdateWorldBool();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CalamitousCertificate>()));
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<CalamityRelic>());
        }
        private static void Talk(string text, Color color)
        {
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            else if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
        }
    }
}
