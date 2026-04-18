using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Placeables.Banners;
using Terraria.ModLoader.Utilities;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using System.Diagnostics.Eventing.Reader;
using Terraria.DataStructures;
using Steamworks;

namespace CalRemix.Content.NPCs
{
    public class WulfrumSwarmer : ModNPC
    {
        public static readonly SoundStyle RandomChirpSound = new("CalamityMod/Sounds/Custom/WulfrumDroidChirp", 4) { PitchVariance = 0.3f };
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.width = NPC.height = 28;
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.lifeMax = 17;
            NPC.knockBackResist = 0f;
            NPC.value = 120;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = CommonCalamitySounds.WulfrumNPCDeathSound;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<WulfrumSwarmerBanner>();
            NPC.noGravity = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.WulfrumSwarmer").Value)
            });
        }

        public int shotCooldown = 90;
        public bool Attacking = false;
        public bool ReachedPlayer = false;
        public Vector2 SeekingTarget = Vector2.Zero; 
        public Vector2 SeekingOffset = Vector2.Zero;
        public float Fastness = 6f;
        public float Clinginess = 0.05f;
        public bool CanAttack = true;
        public NPC Parent;
        public bool Flipping = false;
        public float FlippingSpin = 0.4f;

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void FindFrame(int frameheight)
        {
            float frameSpeed = 5f;
            NPC.frameCounter++;
            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;

                // Do the attack
                if (NPC.frame.Y == frameheight * 7)
                {
                    frameSpeed = 30f;
                    NPC.damage = 10;
                    SoundEngine.PlaySound(SoundID.Item1, NPC.Center);

                    //do a flip !!
                    if (Main.rand.Next(1) == 0)
                    {
                        NPC.velocity.Y -= 5;
                        Flipping = true;
                        FlippingSpin = Main.rand.Next(4, 6) * 0.26f;
                        Fastness = 12f;
                    }
                }
                //end of attack
                else if (NPC.frame.Y == frameheight * 8)
                {
                    frameSpeed = 6f;
                    NPC.damage = 0;
                    Fastness = 6f;
                    Clinginess = 0.05f;
                    Attacking = false;
                    ReachedPlayer = false;
                    NPC.noTileCollide = true;
                    NPC.velocity.Y -= 3;
                }

                // animation control
                if (ReachedPlayer)
                {
                    if (NPC.frame.Y < frameheight * 8) NPC.frame.Y += frameheight;
                }
                else
                {
                    if (NPC.frame.Y < frameheight * 3) NPC.frame.Y += frameheight;
                    else NPC.frame.Y = 0;
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent parentSource && parentSource.Entity is NPC parentNpc && parentNpc.active && parentNpc.ModNPC is WulfrumSwarmerCore)
            {
                Parent = parentNpc;
            }

        }
        public override void AI()
        {
            // die if the core disappears somehow
            if (Parent == null) NPC.StrikeInstantKill();
            else if (Parent.life == 0) NPC.StrikeInstantKill();

            //change direction & rotation based on movement
            NPC.TargetClosest(false);
            
            if (!Flipping)
            {
                NPC.rotation = MathHelper.SmoothStep(NPC.rotation, NPC.velocity.X * 0.1f, 0.3f);
                if (NPC.velocity.X < -1)
                {
                    NPC.spriteDirection = 1;
                }
                else if (NPC.velocity.X > 1)
                {
                    NPC.spriteDirection = 0;
                }
            }
            else
            {
                //make it always frontflip
                FlippingSpin *= 0.97f;
                if (NPC.spriteDirection == 1)
                {
                    NPC.rotation += FlippingSpin;
                }
                else
                {
                    NPC.rotation -= FlippingSpin;
                }

                //stop flipping!
                if (FlippingSpin < 0.01f || (NPC.rotation > MathHelper.ToRadians(10) && NPC.rotation > MathHelper.ToRadians(10) && FlippingSpin < 1.5f) || CanAttack == true)
                {
                    Flipping = false;
                }
            }
            Player player = Main.player[NPC.target];
            NPC.ai[0]++;
            if (!Attacking)
            {
                if (Parent != null)
                {
                    SeekingTarget = Parent.Center;
                }
                // begin to go towards the player
                if (Main.rand.Next(300) == 1 && player != null && CanAttack == true && NPC.Center.Distance(player.Center) < 800 && Collision.CanHit(NPC.Center, 1,1, player.Center, 1,1) == true)
                {
                    SoundEngine.PlaySound(RandomChirpSound with { Volume = RandomChirpSound.Volume * Main.rand.NextFloat(0.5f, 1f) }, NPC.Center);
                    SeekingOffset = Vector2.Zero;
                    Fastness = 10f;
                    Attacking = true;
                    CanAttack = false;
                }

                // choose a location close to the core
                else if (NPC.ai[0] % 10 == 0 && Main.rand.Next(3) == 1 || NPC.ai[0] == 1)
                {
                    SeekingOffset = new Vector2(Main.rand.Next(5, 16), Main.rand.Next(5, 16)).RotatedByRandom(MathHelper.TwoPi);
                }
                //become ready to attack again, and heal a little after reaching the core
                if (NPC.Center.Distance(SeekingTarget + SeekingOffset) <= 15 && CanAttack == false && Attacking == false)
                {
                    CanAttack = true;
                    NPC.noTileCollide = false;
                    if (NPC.life < NPC.lifeMax)
                    {
                        NPC.life += 5;
                        NPC.HealEffect(5, true);
                    }
                    
                    if (NPC.life >= NPC.lifeMax)
                    {
                        NPC.life = NPC.lifeMax;
                    } 
                }

            }
            else
            {
                NPC.ai[2]++;
                SeekingTarget = player.Center;
                // begin to slash the player.
                if (NPC.position.Distance(SeekingTarget) < 25 && !ReachedPlayer)
                {
                    ReachedPlayer = true;
                    NPC.frame.Y = 14 * 4;
                    Fastness = 2f;
                    Clinginess = 0.6f;
                }
                // give up if the player can't be reached, quickly returning to the parent
                else if (NPC.ai[2] >= 240)
                {
                    Attacking = false;
                    NPC.ai[2] = 0;
                    Fastness = 13f;
                    NPC.noTileCollide = true;
                }
            }

            //Move towards the seeking target
            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.DirectionTo(SeekingTarget + SeekingOffset) * Fastness, Clinginess);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 90);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.Center + new Vector2(0,-5), NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 0.3f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.Center + new Vector2(0, -5), NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 0.5f);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<WulfrumMetalScrap>(), 2, 1, 2);
        }
    }
}
