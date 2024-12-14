using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Misc;
using CalamityMod.Particles;
using System.IO;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Buffs;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs
{
    [AutoloadBossHead]
    public class FunnyButterfly : ModNPC
    {
        public ref float State => ref NPC.ai[1];
        public Entity Target = null;
        public int Timer = 0;
        public int DrawCooldown = 0;

        private const int FollowDistance = 400;
        private const int MaxOld = 10;

        public int[] oldDir = new int[MaxOld];
        public int[] oldFrames = new int[MaxOld];
        public Vector2[] oldPos = new Vector2[MaxOld];
        public float[] oldRot = new float[MaxOld];
        public Color[] oldColors = new Color[MaxOld];
        public override string BossHeadTexture => "CalRemix/Content/Items/Misc/TheButterflyEffect";
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Timer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Timer = reader.ReadInt32();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TeleportationImmune[Type] = true;
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Butterfly);
            NPC.aiStyle = -1;
            NPC.lifeMax = 1;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath50;
            NPC.dontTakeDamage = true;
            NPC.catchItem = ItemID.None;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (CalRemixWorld.butterflyEffect == null)
                CalRemixWorld.butterflyEffect = NPC;
            else
                NPC.active = false;
            SoundEngine.PlaySound(SoundID.Item161 with { Pitch = -1f });
            SoundEngine.PlaySound(SCalAltar.SummonSound with { Pitch = 0.5f });
            GeneralParticleHandler.SpawnParticle(new PulseRing(NPC.Center, new Vector2(0, 0), Main.DiscoColor, 1f, 3.5f, 10));
            GeneralParticleHandler.SpawnParticle(new PulseRing(NPC.Center, new Vector2(0, 0), Main.DiscoColor, 2f, 3.5f, 10));
        }
        public override void AI()
        {
            if (Timer < 300)
            {
                Timer++;
                return;
            }
            else
            {
                NPC.aiStyle = NPCAIStyleID.Butterfly;
                AIType = NPCID.Butterfly;
                NPC.catchItem = ModContent.ItemType<TheButterflyEffect>();
            }
            if (Target != null)
            {
                if (Target is NPC)
                {
                    NPC n = Target as NPC;
                    if (!n.active || n.life <= 0 || !Collision.CanHitLine(n.Center, 1, 1, NPC.Center, 1, 1))
                        Target = null;
                }
                else if (Target is Player)
                {
                    Player p = Target as Player;
                    if (!p.active || p.statLife <= 0 || p.DeadOrGhost || p.Distance(NPC.Center) >= FollowDistance || !Collision.CanHitLine(p.Center, 1, 1, NPC.Center, 1, 1))
                        Target = null;
                }
            }
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.whoAmI == NPC.whoAmI || npc.type == Type || npc.life <= 0)
                    continue;
                if (npc.Hitbox.Intersects(NPC.Hitbox) && npc.life > 0)
                {
                    DeathAshParticle.CreateAshesFromNPC(npc);
                    NPC.HitInfo hi = npc.CalculateHitInfo(0, 0, knockBack: 0);
                    hi.InstantKill = true;
                    npc.StrikeNPC(hi);
                    npc.active = false;
                    continue;
                }
                if (Target == null || Target is Player)
                {
                    if (Collision.CanHitLine(npc.Center, 1, 1, NPC.Center, 1, 1))
                        Target = npc;
                    continue;
                }
                if (npc.Distance(NPC.Center) < Target.Distance(NPC.Center) && Collision.CanHitLine(npc.Center, 1, 1, NPC.Center, 1, 1))
                    Target = npc;
            }
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.statLife <= 0 || player.DeadOrGhost)
                    continue;
                if (player.Distance(NPC.Center) < FollowDistance)
                    player.AddBuff(ModContent.BuffType<DestructivePresence>(), 60);
                if (player.Hitbox.Intersects(NPC.Hitbox) && player.statLife > 0 && !player.creativeGodMode)
                    player.KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), player.statLifeMax2, 0);
                if (Target is NPC)
                    continue;
                if (Target == null)
                {
                    if (player.Distance(NPC.Center) < FollowDistance && Collision.CanHitLine(player.Center, 1, 1, NPC.Center, 1, 1))
                        Target = player;
                    continue;
                }
                if (player.Distance(NPC.Center) < Target.Distance(NPC.Center) && player.Distance(NPC.Center) < FollowDistance && Collision.CanHitLine(player.Center, 1, 1, NPC.Center, 1, 1))
                    Target = player;
            }
            if (Target != null)
            {
                if (Target.Distance(NPC.Center) > (Target.Size.Length() / 2f) && Collision.CanHitLine(Target.Center, 1, 1, NPC.Center, 1, 1))
                    NPC.velocity = NPC.SafeDirectionTo(Target.Center) * 2f;
                else if (Target.Hitbox.Intersects(NPC.Hitbox))
                    NPC.velocity = Vector2.Zero;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<TheButterflyEffect>(), 1, 1, 1);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[Type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Timer < 300)
            {
                Dust d = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.Stone, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10), 0, Color.Black);
                d.noGravity = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Timer < 300)
                return false;
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/FunnyButterflyTrail").Value;
            NPC.spriteDirection = NPC.direction;
            SpriteEffects effect = (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle rect = new(0, NPC.frame.Y, texture.Width, texture.Height / 3);
            if (DrawCooldown > 10)
            {
                oldDir[0] = NPC.spriteDirection;
                oldFrames[0] = NPC.frame.Y;
                oldPos[0] = NPC.position;
                oldRot[0] = NPC.rotation;
                oldColors[0] = Main.DiscoColor;
            }
            for (int i = MaxOld - 1; i > 0; i--)
            {
                Rectangle rect2 = new(0, oldFrames[i], texture.Width, texture.Height / 3);
                SpriteEffects effect2 = (oldDir[i] > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(texture2, oldPos[i] - screenPos + new Vector2(0f, NPC.gfxOffY) + Vector2.UnitY * texture.Height / 3, rect2, oldColors[i] * 0.65f * (1 - (float)i / (float)MaxOld), oldRot[i], texture.Size() / 2, NPC.scale, effect2, 0f);
            }
            spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2(0f, NPC.gfxOffY) + Vector2.UnitY * texture.Height / 3, rect, Color.DarkGray * 0.75f, NPC.rotation, texture.Size() / 2, NPC.scale, effect, 0f);
            if (DrawCooldown > 10)
            {
                for (int i = MaxOld - 1; i > 0; i--)
                {
                    oldDir[i] = oldDir[i - 1];
                    oldFrames[i] = oldFrames[i - 1];
                    oldPos[i] = oldPos[i - 1];
                    oldRot[i] = oldRot[i - 1];
                    oldColors[i] = oldColors[i - 1];
                }
                DrawCooldown = 0;
            }
            DrawCooldown++;
            return false;
        }
        public override bool CheckActive() => false;
        public override bool CheckDead() => false;
        public override bool NeedSaving() => true;
    }
}
