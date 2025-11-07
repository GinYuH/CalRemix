using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using System.Collections.Generic;
using SubworldLibrary;

namespace CalRemix.Core.World
{
    public class RemixDowned : ModSystem
    {
        public static bool DownedGens => downedOrigen && downedCarcinogen && downedPhytogen && downedHydrogen && downedOxygen && downedIonogen && downedPathogen;

        public static bool downedCalamity
        {
            get => bossDowns["Calamity"];
            set => bossDowns["Calamity"] = value;
        }
        public static bool downedExcavator
        {
            get => bossDowns["Excavator"];
            set => bossDowns["Excavator"] = value;
        }
        public static bool downedAcidsighter
        {
            get => bossDowns["Acidsighter"];
            set => bossDowns["Acidsighter"] = value;
        }
        public static bool downedOrigen
        {
            get => bossDowns["Origen"];
            set => bossDowns["Origen"] = value;
        }
        public static bool downedCarcinogen
        {
            get => bossDowns["Carcinogen"];
            set => bossDowns["Carcinogen"] = value;
        }
        public static bool downedPhytogen
        {
            get => bossDowns["Phytogen"];
            set => bossDowns["Phytogen"] = value;
        }
        public static bool downedIonogen
        {
            get => bossDowns["Ionogen"];
            set => bossDowns["Ionogen"] = value;
        }
        public static bool downedHydrogen
        {
            get => bossDowns["Hydrogen"];
            set => bossDowns["Hydrogen"] = value;
        }
        public static bool downedOxygen
        {
            get => bossDowns["Oxygen"];
            set => bossDowns["Oxygen"] = value;
        }
        public static bool downedPathogen
        {
            get => bossDowns["Pathogen"];
            set => bossDowns["Pathogen"] = value;
        }
        public static bool downedPyrogen
        {
            get => bossDowns["Pyrogen"];
            set => bossDowns["Pyrogen"] = value;
        }
        public static bool downedDerellect
        {
            get => bossDowns["Derellect"];
            set => bossDowns["Derellect"] = value;
        }
        public static bool downedPolyphemalus
        {
            get => bossDowns["Polyphemalus"];
            set => bossDowns["Polyphemalus"] = value;
        }
        public static bool downedAurelionium
        {
            get => bossDowns["Aurelionium"];
            set => bossDowns["Aurelionium"] = value;
        }
        public static bool downedHypnos
        {
            get => bossDowns["Hypnos"];
            set => bossDowns["Hypnos"] = value;
        }
        public static bool downedSealedOne
        {
            get => bossDowns["SealedOne"];
            set => bossDowns["SealedOne"] = value;
        }
        public static bool downedNoxegg
        {
            get => bossDowns["Noxegg"];
            set => bossDowns["Noxegg"] = value;
        }
        public static bool downedNoxus
        {
            get => bossDowns["Noxus"];
            set => bossDowns["Noxus"] = value;
        }
        public static bool downedLivyatan
        {
            get => bossDowns["Livyatan"];
            set => bossDowns["Livyatan"] = value;
        }
        public static bool downedGastropod
        {
            get => bossDowns["Gastropod"];
            set => bossDowns["Gastropod"] = value;
        }
        public static bool downedVoid
        {
            get => bossDowns["Void"];
            set => bossDowns["Void"] = value;
        }
        public static bool downedDisil
        {
            get => bossDowns["Disilphia"];
            set => bossDowns["Disilphia"] = value;
        }
        public static bool downedOneguy
        {
            get => bossDowns["Oneguy"];
            set => bossDowns["Oneguy"] = value;
        }
        public static bool downedDraedon
        {
            get => bossDowns["Draedon"];
            set => bossDowns["Draedon"] = value;
        }
        public static bool downedCrevi
        {
            get => bossDowns["Crevi"];
            set => bossDowns["Crevi"] = value;
        }

        public static bool downedEarthElemental
        {
            get => bossDowns["EarthElemental"];
            set => bossDowns["EarthElemental"] = value;
        }
        public static bool downedLifeSlime
        {
            get => bossDowns["LifeSlime"];
            set => bossDowns["LifeSlime"] = value;
        }
        public static bool downedClamitas
        {
            get => bossDowns["Clamitas"];
            set => bossDowns["Clamitas"] = value;
        }
        public static bool downedCyberDraedon
        {
            get => bossDowns["CyberDraedon"];
            set => bossDowns["CyberDraedon"] = value;
        }
        public static bool downedOnyxKinsman
        {
            get => bossDowns["OnyxKinsman"];
            set => bossDowns["OnyxKinsman"] = value;
        }
        public static bool downedYggdrasilEnt
        {
            get => bossDowns["YggdrasilEnt"];
            set => bossDowns["YggdrasilEnt"] = value;
        }
        public static bool downedPlagueEmperor
        {
            get => bossDowns["PlagueEmperor"];
            set => bossDowns["PlagueEmperor"] = value;
        }
        public static bool downedLaRuga
        {
            get => bossDowns["LaRuga"];
            set => bossDowns["LaRuga"] = value;
        }
        public static bool downedKingMinnowsPrime
        {
            get => bossDowns["KingMinnowsPrime"];
            set => bossDowns["KingMinnowsPrime"] = value;
        }
        public static bool downedDend
        {
            get => bossDowns["Dend"];
            set => bossDowns["Dend"] = value;
        }
        public static bool downedMaser
        {
            get => bossDowns["Maser"];
            set => bossDowns["Maser"] = value;
        }
        public static bool downedRed
        {
            get => bossDowns["Red"];
            set => bossDowns["Red"] = value;
        }

        public static bool downedGale
        {
            get => bossDowns["Gale"];
            set => bossDowns["Gale"] = value;
        }

        public static Dictionary<string, bool> bossDowns = new()
        {
            { "Calamity", false },
            { "Excavator", false },
            { "Acidsighter", false },
            { "Origen", false },
            { "Carcinogen", false },
            { "Phytogen", false },
            { "Ionogen", false },
            { "Hydrogen", false },
            { "Oxygen", false },
            { "Pathogen", false },
            { "Pyrogen", false },
            { "Derellect", false },
            { "Polyphemalus", false },
            { "Aurelionium", false },
            { "Hypnos", false },
            { "SealedOne", false },
            { "Noxegg", false },
            { "Noxus", false },
            { "EarthElemental", false },
            { "LifeSlime", false },
            { "Clamitas", false },
            { "CyberDraedon", false },
            { "OnyxKinsman", false },
            { "YggdrasilEnt", false },
            { "PlagueEmperor", false },
            { "LaRuga", false },
            { "KingMinnowsPrime", false },
            { "Dend", false },
            { "Maser", false },
            { "Red", false },
            { "Gale", false },
            { "Livyatan", false },
            { "Gastropod", false },
            { "Void", false },
            { "Disilphia", false },
            { "Draedon", false },
            { "Oneguy", false },
            { "Crevi", false },
        };

        public static void ResetBools()
        {
            foreach (string key in bossDowns.Keys)
            {
                bossDowns[key] = false;
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
            foreach (string key in bossDowns.Keys)
            {
                tag["downed" + key] = bossDowns[key];
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            foreach (string key in bossDowns.Keys)
            {
                bossDowns[key] = tag.Get<bool>("downed" + key);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            foreach (string key in bossDowns.Keys)
            {
                writer.Write(bossDowns[key]);
            }
        }
        public override void NetReceive(BinaryReader reader)
        {
            foreach (string key in bossDowns.Keys)
            {
                bossDowns[key] = reader.ReadBoolean();
            }
        }
    }
}