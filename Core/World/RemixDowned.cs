using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using System.Collections.Generic;
using SubworldLibrary;
using System.Reflection;
using Stubble.Core.Classes;

namespace CalRemix.Core.World
{
    public class RemixDowned : ModSystem
    {
        public static bool DownedGens => downedOrigen && downedCarcinogen && downedPhytogen && downedHydrogen && downedOxygen && downedIonogen && downedPathogen;

        // Bosses

        public static bool downedCalamity;

        public static bool downedExcavator;

        public static bool downedAcidsighter;

        public static bool downedOrigen;

        public static bool downedCarcinogen;

        public static bool downedPhytogen;

        public static bool downedIonogen;

        public static bool downedHydrogen;

        public static bool downedOxygen;

        public static bool downedPathogen;

        public static bool downedPyrogen;

        public static bool downedDerellect;

        public static bool downedPolyphemalus;

        public static bool downedAurelionium;

        public static bool downedHypnos;

        public static bool downedSealedOne;

        public static bool downedNoxegg;

        public static bool downedNoxus;

        public static bool downedLivyatan;

        public static bool downedGastropod;

        public static bool downedVoid;

        public static bool downedDisil;

        public static bool downedOneguy;

        public static bool downedDraedon;

        public static bool downedCrevi;

        public static bool downedRajah;

        public static bool downedRajahsRevenge;

        public static bool downedAnomaly;

        //Minibosses

        public static bool downedEarthElemental;

        public static bool downedLifeSlime;

        public static bool downedClamitas;

        public static bool downedCyberDraedon;

        public static bool downedOnyxKinsman;

        public static bool downedYggdrasilEnt;

        public static bool downedPlagueEmperor;

        public static bool downedLaRuga;

        public static bool downedKingMinnowsPrime;

        public static bool downedDend;

        public static bool downedMaser;

        public static bool downedRed;

        public static bool downedCryonix;

        public static bool downedVernix;

        public static bool downedChaotrix;

        //Events

        public static bool downedGale;

        public void ResetBools()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                field.SetValue(null, false);
            }
        }

        public override void OnWorldLoad()
        {
            if (SubworldSystem.AnyActive())
                return;
            ResetBools();
        }
        public override void OnWorldUnload()
        {
            if (SubworldSystem.AnyActive())
                return;
            ResetBools();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                tag[key] = field.GetValue(null);
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                if (tag.TryGet<bool>(key, out var boolValue))
                {
                    field.SetValue(null, boolValue);
                }
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                writer.Write((bool)field.GetValue(null));
            }
        }
        public override void NetReceive(BinaryReader reader)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
               field.SetValue(null, reader.ReadBoolean());
            }
        }
    }
}