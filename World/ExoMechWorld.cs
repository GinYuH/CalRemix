using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using CalRemix.Subworlds;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs;
using CalRemix.NPCs;
using CalRemix.NPCs.Bosses.Hypnos;
using CalRemix.NPCs.Bosses.Losbaf;
using System.IO;
using CalamityMod.World;
using CalamityMod.Particles;
using System.Collections.Immutable;
using CalRemix.Items.Tools;

namespace CalRemix
{
    public class ExoMechWorld : ModSystem
    {
        public static bool ExoMayhem = false;
        public static bool AllExoQuartetActive => (NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>());
        public static bool AllExoMechActive => AllExoQuartetActive && NPC.AnyNPCs(NPCType<Hypnos>()) && NPC.AnyNPCs(NPCType<Losbaf>());
        public static bool AnyDraedonActive => NPC.AnyNPCs(NPCType<Draedon>()) || NPC.AnyNPCs(NPCType<HypnosDraedon>());
        public static bool AnyExoQuartetActive => NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>()) || NPC.AnyNPCs(NPCType<AresBody>()) || NPC.AnyNPCs(NPCType<ThanatosHead>());
        public static bool AnyExoMechActive => AnyExoQuartetActive || NPC.AnyNPCs(NPCType<Hypnos>()) || NPC.AnyNPCs(NPCType<Losbaf>());
        public static bool AnyExosOrDraedonActive => AnyExoMechActive || AnyDraedonActive;
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (SubworldSystem.Current == GetInstance<ExosphereSubworld>())
                backgroundColor = new Color(22, 22, 22);
        }
        public override void PreUpdateWorld()
        {
            /*
            if (CalamityWorld.DraedonSummonCountdown > 200)
            {
                if (!Main.item.ToImmutableList().Exists((Item i) => i.type == ItemType<Caduceus>()))
                {
                    Particle pulse = new PulseRing(CalamityWorld.DraedonSummonPosition, new Vector2(0, 0), Color.Cyan, 1f, 2f, 60);
                    GeneralParticleHandler.SpawnParticle(pulse);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Item.NewItem(Entity.GetSource_None(), CalamityWorld.DraedonSummonPosition, ItemType<Caduceus>());
                }
                CalamityWorld.DraedonSummonCountdown = -1;
            }
            if (ExoMayhem && !AnyExoMechActive)
                ExoMayhem = false;
             */
        }
        public override void OnWorldLoad()
        {
            ExoMayhem = false;
        }
        public override void OnWorldUnload()
        {
            ExoMayhem = false;
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(ExoMayhem);
        }
        public override void NetReceive(BinaryReader reader)
        {
            ExoMayhem = reader.ReadBoolean();
        }

        /*
        if (Main.npc.ToImmutableList().Exists((NPC npc) => npc.type == NPCType<Artemis>()))
        {
            NPC npc = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>());
            Main.NewText("ai0: " + npc.Calamity().newAI[0] + ", ai1: " + npc.Calamity().newAI[1] + ", ai2: " + npc.Calamity().newAI[2] + ", ai3: " + npc.Calamity().newAI[3]);
        }
         */
        /*
        if (Main.npc.ToImmutableList().Exists((NPC npc) => npc.type == NPCType<Artemis>()))
        {
            NPC npc = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>());
            Main.NewText("ai0: " + npc.Calamity().newAI[0] + ", ai1: " + npc.Calamity().newAI[1] + ", ai2: " + npc.Calamity().newAI[2] + ", ai3: " + npc.Calamity().newAI[3]);
            if (npc.Calamity().newAI[1] == 2)
                npc.Calamity().newAI[1] = 0;
        }
        if (Main.npc.ToImmutableList().Exists((NPC npc) => npc.type == NPCType<Apollo>()))
        {
            NPC npc = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>());
            if (npc.Calamity().newAI[1] == 2)
                npc.Calamity().newAI[1] = 0;
        }
        if (Main.npc.ToImmutableList().Exists((NPC npc) => npc.type == NPCType<AresBody>()))
        {
            NPC npc = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>());
            if (npc.Calamity().newAI[1] == 2)
                npc.Calamity().newAI[1] = 0;
        }
        if (Main.npc.ToImmutableList().Exists((NPC npc) => npc.type == NPCType<ThanatosHead>()))
        {
            NPC npc = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>());
            if (npc.Calamity().newAI[1] == 2)
                npc.Calamity().newAI[1] = 0;
        }
         */
    }
}