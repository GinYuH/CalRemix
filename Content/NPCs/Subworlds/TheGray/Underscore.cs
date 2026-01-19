using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using CalRemix.Content.Items.Weapons;
using Terraria.Audio;
using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.DataStructures;
using CalRemix.Content.Items.Placeables.Subworlds.TheGray;

namespace CalRemix.Content.NPCs.Subworlds.TheGray
{
    public class Underscore : ModNPC
    {
        public static SoundStyle DeathSound = new SoundStyle("CalRemix/Assets/Sounds/UnderscoreDeath") with { PitchVariance = 1.4f };

        public static SoundStyle AttackSound = new SoundStyle("CalRemix/Assets/Sounds/Underscore", 3) with { PitchVariance = 1.4f, MaxInstances = 0 };

        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 66;
            NPC.height = 160;
            NPC.npcSlots = 2;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = null;
            NPC.DeathSound = DeathSound;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            foreach (Player p in Main.ActivePlayers)
            {
                if (NPC.ai[0] == 1 || (Main.rand.NextBool(120) && NPC.ai[0] > 20))
                {
                    SoundEngine.PlaySound(AttackSound);
                }
                if (p.Hitbox.Intersects(NPC.Hitbox) && p.HeldItem.type != ModContent.ItemType<Umbren>())
                {
                    p.Center = NPC.Center;
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 160)
                    {
                        p.statLife -= (int)(p.statLifeMax2 * 0.005f);
                        if (p.statLife <= 0)
                        {
                            p.KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), 1, 1);
                        }
                    }
                    p.Calamity().GeneralScreenShakePower = 30;
                }
                else
                {
                    NPC.ai[0] = 0;
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;

            spriteBatch.Draw(tex, NPC.Bottom - screenPos, null, Color.Black, NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);
            spriteBatch.Draw(glow, NPC.Bottom - screenPos, null, Color.White, NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);
            Vector2 eyePosition = NPC.Bottom - Vector2.UnitY * (140) - screenPos;
            if (NPC.HasPlayerTarget)
            {
                float dist = Main.player[NPC.target].Distance(NPC.Center);
                if (NPC.ai[0] == 0)
                {
                    eyePosition += (eyePosition + screenPos).DirectionTo(Main.player[NPC.target].Center) * MathHelper.Lerp(0, 8, Utils.GetLerpValue(0, 100, dist, true));
                }
            }
            spriteBatch.Draw(eye, eyePosition, null, Color.White, NPC.rotation, new Vector2(eye.Width / 2, eye.Height / 2), NPC.scale, 0, 0);
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (item.type == ModContent.ItemType<Umbren>())
                return true;
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<UmbrenSwing>())
                return true;
            return false;
        }

        public override void OnKill()
        {
            for (int i = 0; i < 50; i++)
                GeneralParticleHandler.SpawnParticle(new SquareParticle(Main.rand.NextVector2FromRectangle(NPC.Hitbox), Main.rand.NextVector2Circular(20, 20), false, 60, Main.rand.NextFloat(1f, 3f), Color.Red));
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<YellowMazeBrick>(), 1, 20, 40);
            npcLoot.Add(ModContent.ItemType<BlueMazeBrick>(), 1, 20, 40);
            npcLoot.Add(ModContent.ItemType<QuestionSoil>(), 1, 30, 60);
        }
    }
}
