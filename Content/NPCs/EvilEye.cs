using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;

namespace CalRemix.Content.NPCs
{
    public class EvilEye : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public static readonly SoundStyle EvilEyeVoice = new("CalRemix/Assets/Sounds/EvilEyeVoice")
        {
            MaxInstances = 0
        };
        public static readonly SoundStyle EvilEyeAlarm1 = new("CalRemix/Assets/Sounds/EvilEyeAlarm1")
        {
            MaxInstances = 0
        };
        public static readonly SoundStyle EvilEyeAlarm2 = new("CalRemix/Assets/Sounds/EvilEyeAlarm2")
        {
            MaxInstances = 0
        };
        private int maxTime = 600;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Eye");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 2;
            NPC.width = 36;
            NPC.height = 21;
            NPC.defense = 0;
            NPC.lifeMax = 220;
            NPC.value = 20;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1 with { Pitch = -1, Volume = 2 };
            NPC.rarity = 1;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - MathHelper.Pi;
            NPC.velocity += NPC.DirectionTo(Main.player[NPC.target].Center) * 0.1f;

            if (Timer == 180)
            {
                SoundEngine.PlaySound(EvilEyeVoice with { Volume = 1 }, NPC.position);
            }
            if (Timer > 180 && Timer < maxTime)
            {
                if (Timer % 50 == 0)
                {
                    SoundEngine.PlaySound(EvilEyeAlarm1, NPC.position);
                }
                if (Timer % 90 == 0)
                {
                    SoundEngine.PlaySound(EvilEyeAlarm2, NPC.position);
                }

                int dust = Dust.NewDust(new Vector2(NPC.position.X - NPC.width, NPC.position.Y), NPC.width * 2, NPC.height * 2, DustID.BlueFairy);
                Main.dust[dust].velocity = Main.dust[dust].position.DirectionTo(NPC.Center);
            }

            if (Timer == maxTime)
            {
                int fireball = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 7, ProjectileID.CursedFlameFriendly, 20, 1);
                Main.projectile[fireball].friendly = false;
                Main.projectile[fireball].hostile = true;
            }
            else if (Timer > maxTime)
            {
                Timer = -1;
            }

            Timer++;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D line = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/LaserWallTelegraphBeam").Value;
            Texture2D circle = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleOpenCircle").Value;

            if (Timer > 180)
            {
                // line
                Vector2 vector = new Vector2(2400f / (float)line.Width, 2);
                Vector2 origin = line.Size() * new Vector2(0f, 0.5f);
                Vector2 scale = vector * new Vector2(1f, 1.6f);
                Main.EntitySpriteDraw(line, NPC.Center - Main.screenPosition, null, Color.Fuchsia, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation(), origin, scale, SpriteEffects.None);
            }
            if (Timer > maxTime - 60)
            {
                // ring
                int TimerButAwesome = ((int)Timer - (maxTime - 60) - 60);
                Vector2 scale = new Vector2(TimerButAwesome, TimerButAwesome);
                Main.EntitySpriteDraw(circle, NPC.Center - Main.screenPosition, null, Color.Fuchsia, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation(), circle.Size() / 2, scale, SpriteEffects.None);
            }
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.OverworldNight.Chance * 0.02f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Nazar, 1);
        }
    }
}
