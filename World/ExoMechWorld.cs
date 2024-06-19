using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
using CalamityMod.UI.DraedonSummoning;

namespace CalRemix
{
    public class ExoMechWorld : ModSystem
    {
        public static bool ExoMayhem = false;
        public static bool ExoQuartetActive => NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>()) || NPC.AnyNPCs(NPCType<AresBody>()) || NPC.AnyNPCs(NPCType<ThanatosHead>());
        public static bool AnyExoMechActive => ExoQuartetActive || NPC.AnyNPCs(NPCType<Hypnos>()) || NPC.AnyNPCs(NPCType<Losbaf>());
        public static bool AnyExosOrDraedonActive => AnyExoMechActive || NPC.AnyNPCs(NPCType<Draedon>()) || NPC.AnyNPCs(NPCType<RemixDraedon>());
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (SubworldSystem.Current == GetInstance<ExosphereSubworld>())
                backgroundColor = new Color(22, 22, 22);
        }
        public override void PreUpdateWorld()
        {
            CalamityWorld.DraedonSummonCountdown = -1;
            if (ExoMayhem && !AnyExoMechActive)
                ExoMayhem = false;
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

    }
}