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
using MonoMod.Cil;
using Mono.Cecil.Cil;
using CalamityMod;
using System.Collections.Immutable;

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
        public override void Load()
        {
            /*
            IL.CalamityMod.NPCs.ExoMechs.Ares.AresBody.AI += AresHide;
            IL.CalamityMod.NPCs.ExoMechs.Artemis.Artemis.AI += ArtemisHide;
            IL.CalamityMod.NPCs.ExoMechs.Apollo.Apollo.AI += ApolloHide;
            IL.CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosHead.AI += ThanatosHide;
             */
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
            */
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
        private static void AresHide(ILContext il)
        {
            var c = new ILCursor(il);
            for (int e = 0; e < 5; e++)
            {
                if (c.TryGotoNext(i => i.MatchLdcR4(2f), i => i.MatchCall<AresBody>("set_SecondaryAIState")))
                {
                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() => !ExoMayhem ? 2f : 0f);
                }
            }
        }
        private static void ArtemisHide(ILContext il)
        {
            var c = new ILCursor(il);
            for (int e = 0; e < 5; e++)
            {
                if (c.TryGotoNext(i => i.MatchLdcR4(2f), i => i.MatchCall<Artemis>("set_SecondaryAIState")))
                {
                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() => !ExoMayhem ? 2f : 0f);
                }
            }
        }
        private static void ApolloHide(ILContext il)
        {
            var c = new ILCursor(il);
            for (int e = 0; e < 5; e++)
            {
                if (c.TryGotoNext(i => i.MatchLdcR4(2f), i => i.MatchCall<Apollo>("set_SecondaryAIState")))
                {
                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() => !ExoMayhem ? 2f : 0f);
                }
            }
        }
        private static void ThanatosHide(ILContext il)
        {
            var c = new ILCursor(il);
            for (int e = 0; e < 5; e++)
            {
                if (c.TryGotoNext(i => i.MatchLdcR4(2f), i => i.MatchCall<ThanatosHead>("set_SecondaryAIState")))
                {
                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() => !ExoMayhem ? 2f : 0f);
                }
            }
        }
    }
}