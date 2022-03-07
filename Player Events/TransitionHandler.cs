using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TransitionRandomiser.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TransitionRandomiser.Player_Events
{
    internal class TransitionHandler
    {

        private static Biome CurrentBiome = BiomeHandler.LIFEPOD;
        private static Biome PreviousBiome = CurrentBiome;
        private static Boolean Processing = false;
        private static Dictionary<String, TeleportPosition> TransitionDictionary = new Dictionary<String, TeleportPosition>()
        {
        };
        internal static void SetCurrentBiome(Biome Biome, Boolean dontUpdatePreviousBiome = false)
        {
            if (CurrentBiome.GetName() != Biome.GetName() && !dontUpdatePreviousBiome)
            {
                PreviousBiome = CurrentBiome;
                CustomUI.SetSecondText("Previous biome: " + PreviousBiome.GetName());
            }
            CurrentBiome = Biome;
            CustomUI.SetFirstText("Current biome: " + CurrentBiome.GetName());
        }

        internal static Biome GetCurrentBiome()
        {
            return CurrentBiome;
        }

        internal static Boolean GetProcessing()
        {
            return Processing;
        }

        internal static void SetProcessing(Boolean state)
        {
            Processing = state;
        }

        internal static TeleportPosition getTeleportPositionForBiomeTransfer(Biome newBiome)
        {
            String biomeTransferString = CurrentBiome.GetName() + newBiome.GetName();
            if (TransitionDictionary.ContainsKey(biomeTransferString))
            {
                return TransitionDictionary[biomeTransferString];
            }
            else
            {
                var biomePosition = GetRandomBiomePositions();
                TransitionDictionary.Add(biomeTransferString, biomePosition);
                return biomePosition;
            }
        }

        private static TeleportPosition GetRandomBiomePositions()
        {
            var transitionLocations = TransitionDictionary.Values.ToArray();
            var biomeTeleportData = BiomeConsoleCommand.main.data.locations;
            var availableLocations = biomeTeleportData.Except(transitionLocations).ToArray();
            var totalCount = availableLocations.Length;
            var newPositionNum = Random.Range(0, totalCount);

            return availableLocations[newPositionNum];
        }
    }
}
