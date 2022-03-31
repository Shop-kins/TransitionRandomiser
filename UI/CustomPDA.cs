using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TransitionRandomiser.Player_Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UWE;

namespace TransitionRandomiser.UI
{
    class CustomPDA
    {

        private static uGUI_EncyclopediaTab tab = null;

        public static void CreatePDAEntries(String directory)
        {

            Language.main.strings.Remove("EncyPath_TransitionData");
            Language.main.strings.Add("EncyPath_TransitionData", "Transition Data");

            if (tab == null)
            {
                try
                {
                    Boolean open = Player.main.GetPDA().isOpen;
                    if (!open)
                        Player.main.GetPDA().Open();
                    tab = Player.main.GetPDA().ui.GetTab(PDATab.Encyclopedia) as uGUI_EncyclopediaTab;
                    if (!open)
                        Player.main.GetPDA().Close();
                    UWE.Utils.lockCursor = true;
                }
                catch (Exception e) { }
            }

            foreach (Biome biome in BiomeHandler.BIOMES)
            {
                String id = "transitionData" + biome.GetName().Replace(" ", "");

                String contentString = "";

                List<TeleportLocation> locations = new List<TeleportLocation>();
                foreach (KeyValuePair<Transition, TeleportLocation[]> pair in TransitionHandler.GetTransitionsFromBiome(biome))
                {
                    pair.Value.ForEach((l) => locations.Add(l));
                }
                foreach (TeleportLocation location in locations.OrderBy((l) => l.getMarker()))
                {
                    TeleportLocation reverseLocation = TransitionHandler.getTeleportPositionForBiomeTransfer(location.GetOrigin(), location.GetPosition().ToVector3(), location.GetBiome(), false);
                    if (location.isKnownToPlayer())
                    {
                        contentString += location.getMarker() + ": " + location.GetBiome().GetName() + " (from " + location.GetOrigin().GetName() + ")\n";
                    }
                    else
                    {
                        contentString += location.getMarker() + ": ?\n";
                    }
                }

                Language.main.strings.Remove("Ency_" + id);
                Language.main.strings.Remove("EncyDesc_" + id);
                Language.main.strings.Add("Ency_" + id, biome.GetName());
                Language.main.strings.Add("EncyDesc_" + id, contentString);

                if (!PDAEncyclopedia.mapping.ContainsKey(id))
                {
                    PDAEncyclopedia.EntryData data = new PDAEncyclopedia.EntryData();
                    data.key = id;
                    data.path = "TransitionData";
                    data.nodes = new String[] { "TransitionData" };
                    data.image = LoadPNG(Path.Combine(directory, "maps", biome.GetName().ToLower().Replace(" ", "") + ".png"));

                    PDAEncyclopedia.mapping.Add(id, data);
                }
                else if (tab && tab.activeEntry && tab.activeEntry.key == id)
                {
                    tab.DisplayEntry(id);
                }
            }
        }

        public static void AddPDAEntry(Biome biome, Boolean verbose)
        {
            String id = "transitionData" + biome.GetName().Replace(" ", "");
            if (!PDAEncyclopedia.entries.ContainsKey(id))
            {
                PDAEncyclopedia.Add(id, new PDAEncyclopedia.Entry(), verbose);
            }
        }

        private static Texture2D LoadPNG(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                tex.LoadImage(fileData);
            }
            return tex;
        }

    }
}
