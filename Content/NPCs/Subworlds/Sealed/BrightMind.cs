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
using CalamityMod.NPCs.Perforator;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    [AutoloadHead]
    public class BrightMind : QuestNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMind") with { PitchVariance = 0.75f };

        public static SoundStyle hitSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMindHit") with { PitchVariance = 0.75f };

        public static SoundStyle deathSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMindDeath");

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 80;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = hitSound;
            NPC.DeathSound = deathSound;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BadlandsBiome>().Type };
        }
        public override void AI()
        {
            base.AI();
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            Timer++;
            if (Timer > 120 && NPC.dontTakeDamage && State == 0)
            {
                NPC.dontTakeDamage = false;
            }
            if (NPCDialogueUI.NotFinishedTalking(NPC) && Timer % 7 == 0)
            {
                SoundEngine.PlaySound(talkSound, NPC.Center);
            }
            if ((JustFinishedTalking || Main.netMode != NetmodeID.SinglePlayer) && ItemQuestSystem.brainLevel == 3 && Main.player[NPC.target].Distance(NPC.Center) < 600 && NPC.life == NPC.lifeMax)
            {
                State = 1;
                Timer = 0;
            }
            if (State == 1)
            {
                int wait = 30;
                int impact = 20;
                int knockoff = 30;
                if (Timer < wait)
                {
                    NPC.dontTakeDamage = true;
                }
                else if (Timer == wait)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 1000, (int)NPC.Center.Y, ModContent.NPCType<MonorianWarrior>());
                }
                else if (Timer < wait + impact)
                {

                }
                else if (Timer == wait + impact)
                {
                    NPC.velocity = new Vector2(-50, -26);
                    SoundEngine.PlaySound(PerforatorHive.DeathSound with { Pitch = 0.5f, Volume = 2 }, NPC.Center);
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
                    NPC.noTileCollide = true;
                }
                else if (Timer > wait + impact && Timer < wait + impact + knockoff)
                {
                    NPC.velocity = new Vector2(-40, -26);
                    NPC.rotation -= 0.2f;
                    NPC.noTileCollide = true;
                }
                else if (Timer > wait + impact + knockoff)
                {
                    NPC.active = false;
                }
            }
            else
            {
                NPC.dontTakeDamage = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float speed = 40f;
            Vector2 scale = Vector2.One + new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * speed), MathF.Sin(Main.GlobalTimeWrappedHourly * speed)) * 0.1f;
            if (!NPCDialogueUI.NotFinishedTalking(NPC))
                scale = Vector2.One;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + texture.Height / 2), null, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<RustedShardProjectile>() && !NPC.dontTakeDamage)
                return true;
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            NPCDialogueUI.StartDialogue(NPC.whoAmI, "Hurt" + Main.rand.Next(1, 11));
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<TanMatter>());
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CanBeTalkedTo => State == 0;
    }
}
