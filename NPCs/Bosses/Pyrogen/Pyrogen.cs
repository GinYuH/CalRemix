using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using ReLogic.Content;
using CalamityMod;
using CalRemix.Items;
using System.Linq;
using CalRemix.UI;
using System.Collections;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using CalRemix.Retheme;
using CalRemix.UI.ElementalSystem;
using CalamityMod.World;
using Terraria.DataStructures;

namespace CalRemix.NPCs.Bosses.Pyrogen
{
    [AutoloadBossHead]
    public class Pyrogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public enum PhaseType
        {
            Idle = 0
        }

        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/CryogenHit", 3);
        public static readonly SoundStyle TransitionSound = new("CalamityMod/Sounds/NPCHit/CryogenPhaseTransitionCrack");
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/CryogenDeath");
        public override string Texture => "CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase1";

        public static Asset<Texture2D> Phase2Texture;
        public static Asset<Texture2D> Phase3Texture;
        public static Asset<Texture2D> Phase4Texture;
        public static Asset<Texture2D> Phase5Texture;
        public static Asset<Texture2D> GlowTexture;

        public static int cryoIconIndex;
        public static int pyroIconIndex;

        internal static void LoadHeadIcons()
        {
            string pyroIconPath = "CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase1_Head_Boss";
            string cryoIconPath = "CalamityMod/NPCs/Cryogen/Cryogen_Phase1_Head_Boss";

            CalRemix.Instance.AddBossHeadTexture(pyroIconPath, -1);
            pyroIconIndex = ModContent.GetModBossHeadSlot(pyroIconPath);

            CalRemix.Instance.AddBossHeadTexture(cryoIconPath, -1);
            cryoIconIndex = ModContent.GetModBossHeadSlot(cryoIconPath);

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrogen");
            if (!Main.dedServ)
            {
                Phase2Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase2", AssetRequestMode.AsyncLoad);
                Phase3Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase3", AssetRequestMode.AsyncLoad);
                Phase4Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase4", AssetRequestMode.AsyncLoad);
                Phase5Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase5", AssetRequestMode.AsyncLoad);
                GlowTexture = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 200;
            NPC.width = 86;
            NPC.height = 88;
            NPC.defense = 60;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(422600, 475700, 1550000); ;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(3, 5, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToCold = true;
            Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/InfernalSeal");
        }


        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                int bitSprite = 0;
                switch (i)
                {
                    case 0:
                    case 4:
                    case 8:
                    case 12:
                        bitSprite = 0;
                        break;
                    case 1:
                    case 5:
                    case 9:
                    case 13:
                        bitSprite = 1;
                        break;
                    case 2:
                    case 6:
                    case 10:
                    case 14:
                        bitSprite = 2;
                        break;
                    case 3:
                    case 7:
                    case 11:
                    case 15:
                        bitSprite = 3;
                        break;
                }
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PyrogenShield>(), ai0: NPC.whoAmI, ai1: i, ai2: bitSprite);  
            }
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (Target == null || Target.dead)
            {
                NPC.velocity.Y -= 1;
                return;
            }
            switch (Phase)
            {
                case (int)PhaseType.Idle:
                    {
                        int tpDistX = 1000;
                        int tpDistY = 500;
                        NPC.ai[1]++;
                        player = Main.player[NPC.target];
                        Vector2 pyrogenCenter = new Vector2(NPC.Center.X, NPC.Center.Y);
                        float playerXDist = player.Center.X - pyrogenCenter.X;
                        float playerYDist = player.Center.Y - pyrogenCenter.Y;
                        float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                        float pyrogenSpeed = CalamityWorld.revenge ? 8f : 7f;
                        pyrogenSpeed += 4f;

                        playerDistance = pyrogenSpeed / playerDistance;
                        playerXDist *= playerDistance;
                        playerYDist *= playerDistance;

                        float inertia = 25f;

                        NPC.velocity.X = (NPC.velocity.X * inertia + playerXDist) / (inertia + 1f);
                        NPC.velocity.Y = (NPC.velocity.Y * inertia + playerYDist) / (inertia + 1f);
                        NPC.rotation = NPC.velocity.X * 0.1f;

                        if (NPC.ai[1] == 180)
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        if (NPC.ai[1] > 180)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Dirt);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        if (NPC.ai[1] > 240)
                        {
                            DustExplosion();
                            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                            DustExplosion();
                            NPC.ai[1] = 0;
                        }
                    }
                    break;
            }
            
            base.AI();
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4),
                NPC.frame, Color.Orange, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effects, 0);
            }
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
        new FlavorTextBestiaryInfoElement("Having absorbed the energy of the fallen goddess, this elemental construct's seal is supreme amongst its kin. Fate is often cruel to the kind, and mistakes repeated are the most bitter form of punishment.")
            });
        }

        public override void ModifyTypeName(ref string typeName)
        {
            if (Main.zenithWorld)
            {
                typeName = CalamityUtils.GetTextValue("NPCs.Cryogen");
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
