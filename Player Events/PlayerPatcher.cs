using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace TransitionRandomiser.Player_Events
{

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    internal class Player_Update_Patch
    {

        private static Boolean loadedIn = false;
        private static int Cooldown = 120;

        [HarmonyPostfix]
        public static void Postfix()
        {
            try
            {
                var main = Player.main;
                Biome newBiome = BiomeHandler.GetBiomeByGameID(main.GetBiomeString().ToLower());
                if (!loadedIn && newBiome.GetName() == "lifepod") //TODO DETECT GAME START BETTER
                {
                    Console.WriteLine("Loaded In");
                    loadedIn = true;
                }
                if (loadedIn && newBiome != null)
                {
                    if (TransitionHandler.GetProcessing()) //TODO DETECT TELEPORT COMPLETE BETTER
                    {
                        if(Cooldown < 1)
                        {
                            Cooldown = 120;
                            TransitionHandler.SetCurrentBiome(newBiome);
                            TransitionHandler.SetProcessing(false);
                        } else
                        {
                            Cooldown -= 1;
                        }
                    }
                    else
                    {
                        {
                            Biome currentBiome = TransitionHandler.GetCurrentBiome();
                            if (currentBiome.GetName() != newBiome.GetName() && newBiome.GetName() != "lifepod" && currentBiome.GetName() != "lifepod" && !TransitionHandler.GetProcessing())
                            {
                                TransitionHandler.SetProcessing(true);
                                Console.WriteLine("Detected Biome CHANGE FROM " + currentBiome.GetName() + " TO " + newBiome.GetName());
                                try
                                {
                                    Console.WriteLine("PROCESSING Biome CHANGE!");
                                    var teleportPosition = TransitionHandler.getTeleportPositionForBiomeTransfer(newBiome);
                                    Player.main.SetPosition(teleportPosition.position);
                                    Player.main.OnPlayerPositionCheat();
                                }
                                catch { 
                                
                                }
                            }
                        }
                        TransitionHandler.SetCurrentBiome(newBiome);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to invoke action " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
    

}
