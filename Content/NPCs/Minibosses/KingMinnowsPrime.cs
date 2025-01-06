using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.UI;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class KingMinnowsPrime : ModNPC
    {
        int beginning = 0;
        int AISwitchTimer = 0;
        int TIME1 = 0;
        int SOUND = 0;
        int weaktime = 0;
        bool weaksound = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;

            if (Main.dedServ)
                return;
            HelperMessage.New("KingMinnows",
                "A king minnows prime! This fish is known for yelling out “die!” Which is a subtle reference to the fact that it will kill you! This reference is best observed on the Death mode difficulty.",
                "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            Mod calamityMod = CalRemix.CalMod;
            NPC.height = 50;
            NPC.width = 50;
            NPC.damage = 70;
            NPC.defense = 70;
            NPC.lifeMax = 15000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/MinnowsDeathScream");
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;




        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedKingMinnowsPrime = true;
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon, new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)});
        }



        public override void FindFrame(int frameHeight)
        {
            if (!NPC.wet && !NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreKill()
        {
            Player player = Main.player[NPC.target];
            SpawnProjectile(player, 0, 0, 0, ModContent.ProjectileType<MinnowsDeathAnimation>());
            return true;
        }


        public override void AI()

        {
            Particle inward = new PulseRing(NPC.Center, new Vector2(0, 0), Color.Orange, 3f, 0.1f, 10);
            Particle outward = new PulseRing(NPC.Center, new Vector2(0, 0), Color.Orange, 0.1f, 3f, 10);

            Particle DashBloom = new StrongBloom(NPC.Center, new Vector2(0, 0), Color.Cyan, 1.5f, 10);
            NPC.spriteDirection = 1;
            NPC.rotation = NPC.AngleTo(Main.player[NPC.target].Center);
            Lighting.AddLight(NPC.Center, new Vector3(0, 5, 10));
            Player player = Main.player[NPC.target];

            beginning++;
            if (beginning == 10)
            {
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/thypunishmentisdeath"), NPC.Center);
            }

            if (beginning > 300)
            {
                AISwitchTimer++;
                NPC.dontTakeDamage = false;
            }


            if (NPC.life < NPC.lifeMax / 2)
            {
                NPC.damage = 80;
                NPC.defense = 80;
                weaksound = true;
            }
            if (weaksound == true)
            {
                weaktime++;
            }
            if (weaktime == 5)
            {
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/weak"), NPC.Center);
            }
            if (NPC.ai[0] == 0 && beginning > 300)
            {
                NPC.aiStyle = 56;
                TIME1++;
                if (TIME1 == 10 && SOUND == 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/judgement"), NPC.Center);
                }
                if (TIME1 == 10 && SOUND == 1)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/thyendisnow"), NPC.Center);
                }

                if (TIME1 > 0 && TIME1 < 100)
                {
                    NPC.velocity *= 0;
                }

                if (TIME1 > 100 && TIME1 < 107)
                {
                    GeneralParticleHandler.SpawnParticle(DashBloom);
                    NPC.velocity *= 3f;
                }
                if (TIME1 > 107)
                {
                    NPC.velocity *= 0f;

                }
                if (TIME1 == 110)
                {
                    GeneralParticleHandler.SpawnParticle(DashBloom);
                    SpawnProjectile(player, 0, 50, 0, ModContent.ProjectileType<MinnowsSmallShockwave>());
                    SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                    NPC.velocity *= 0f;
                    TIME1 = 50;
                }
            }
            if (AISwitchTimer == 300)
            {
                TIME1 = 0;
                NPC.ai[0] = 1;
            }
            if (NPC.ai[0] == 1)
            {

                if (TIME1 > 0)
                {
                    NPC.velocity *= 0;
                }
                TIME1++;
                if (TIME1 == 60 && SOUND == 0)
                {
                    GeneralParticleHandler.SpawnParticle(inward);
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/preparethyself"), NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item132, NPC.Center);

                }
                if (TIME1 == 10 && SOUND == 1)
                {
                    GeneralParticleHandler.SpawnParticle(inward);
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/useless"), NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item132, NPC.Center);
                }
                if (TIME1 == 120)
                {
                    GeneralParticleHandler.SpawnParticle(outward);
                    SoundEngine.PlaySound(SoundID.Item45, NPC.Center);
                    SpawnProjectile(player, 50, 50, 0, ProjectileID.CultistBossFireBall);
                }
            }
            if (AISwitchTimer == 500)
            {

                NPC.ai[0] = 2;
                TIME1 = 0;
            }
            if (NPC.ai[0] == 2)
            {
                NPC.aiStyle = 0;
                TIME1++;
                if (TIME1 == 30 && SOUND == 0)
                {
                    Vector2 CRUSH = player.Center + new Vector2(0, -500);
                    NPC.Center = CRUSH;
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/crush"), NPC.Center);
                }
                if (TIME1 == 30 && SOUND == 1)
                {
                    Vector2 CRUSH = player.Center + new Vector2(0, -500);
                    NPC.Center = CRUSH;
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/MinnowsSounds/die"), NPC.Center);
                }
                if (TIME1 > 60 && TIME1 < 67)
                {
                    GeneralParticleHandler.SpawnParticle(DashBloom);
                    NPC.velocity = new Vector2(0, 100);
                }
                if (TIME1 == 67)
                {
                    GeneralParticleHandler.SpawnParticle(DashBloom);
                    NPC.velocity *= 0;
                    SoundEngine.PlaySound(SoundID.Item74, NPC.Center);
                    SpawnProjectile(player, 0, 60, 0, ModContent.ProjectileType<MinnowsLargeShockwave>());
                }
            }
            if (AISwitchTimer == 650)
            {
                SOUND++;
                NPC.ai[0] = 0;
                AISwitchTimer = 0;
                TIME1 = 0;
            }
            if (SOUND == 2)
            {
                SOUND = 0;
            }



        }
        void SpawnProjectile(Terraria.Player player, float projectileSpeed, int damage, int knockBack, int type)

        {




            Vector2 velocity = Vector2.Normalize(player.MountedCenter - NPC.Center) * projectileSpeed;

            int a = Projectile.NewProjectile(spawnSource: (null), NPC.Center, velocity, type, damage, knockBack, Main.myPlayer);
            Main.projectile[a].tileCollide = false;
            Main.projectile[a].velocity *= 1.5f;










        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            Mod CalamityMod = CalRemix.CalMod;
            Player player = spawnInfo.Player;

            if ((bool)CalamityMod.Call("GetBossDowned", "yharon"))
            {


                if (!(bool)CalamityMod.Call("GetInZone", player.whoAmI, "abyss"))
                {


                    return Terraria.ModLoader.Utilities.SpawnCondition.Cavern.Chance * 0.0118f;
                }
            }

            return 0f;



        }
    }
}






