using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Story;
using TransitionRandomiser.UI;
using UnityEngine;

namespace TransitionRandomiser.Player_Events
{

    class PlayerPatcher
    {
        private static Boolean isDead = true;
        private static Boolean biomeChangeDone = true;

        private static Biome lastFrameBiome = BiomeHandler.SAFESHALLOWS;
        private static double stabilisationCounter = 0;

        private static double stabilisationTime = 5;

        private static String yourBiomeText = "";
        private static int yourBiomeTextCounter = 0;

        private static String baseDirectory, saveFilePath;

        private static Boolean precursorPortalUsed = false;
        private static Vector3 positionBeforePrecursorTeleporter;

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class Player_Update_Patch
        {

            [HarmonyPostfix]
            public static void Postfix()
            {
                CustomUI.Update();
                if (IngameMenu.main.gameObject.activeInHierarchy)
                {
                    CustomUI.SetSecondText("Waiting for unpause");
                    return;
                }

                CustomUI.SetBigText("");
                CustomUI.SetFirstText("Current biome: " + TransitionHandler.GetCurrentBiome().GetName());
                if (TransitionHandler.GetUndoLocation() != null)
                {
                    CustomUI.SetSecondText("Press U to undo the last teleport");
                }
                else
                {
                    CustomUI.SetSecondText("No undo available");
                }
                if (yourBiomeTextCounter <= 0)
                {
                    yourBiomeText = "";
                }
                yourBiomeTextCounter--;
                CustomUI.SetBiomeText(yourBiomeText);

                CustomPDA.AddPDAEntry(TransitionHandler.GetCurrentBiome(), false);

                try
                {
                    Biome newBiome = BiomeHandler.GetBiomeByGameID(Player.main.GetBiomeString().ToLower());
                    if (newBiome == null) return;

                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        Teleport(TransitionHandler.GetUndoLocation());
                        TransitionHandler.ClearUndoLocation();
                        return;
                    }

                    // Moonpool swimming fix
                    PrecursorMoonPoolTrigger moonpoolTrigger = GameObject.FindObjectOfType<PrecursorMoonPoolTrigger>();
                    if (TransitionHandler.GetCurrentBiome().GetName() != BiomeHandler.ALIENBASE.GetName())
                    {
                        if (moonpoolTrigger != null)
                        {
                            moonpoolTrigger.checkPlayer = null;
                        }
                        Player.main.precursorOutOfWater = false;
                    }
                    else if (Player.main.GetBiomeString().ToLower().Contains("precursor_gun"))
                    {
                        if (moonpoolTrigger != null)
                        {
                            moonpoolTrigger.checkPlayer = Player.main;
                        }
                    }

                    // Stabilisation Counter
                    if (Player.main.playerController.inputEnabled)
                    {
                        if (lastFrameBiome.GetName() == newBiome.GetName())
                        {
                            stabilisationCounter += Time.deltaTime;
                        }
                        else
                        {
                            if (newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName() && biomeChangeDone)
                            {
                                UnfreezeStats();
                            }
                            else
                            {
                                FreezeStats();
                            }
                            stabilisationCounter = 0;
                            lastFrameBiome = newBiome;
                        }
                        if (stabilisationCounter > stabilisationTime * 1.25 && precursorPortalUsed)
                        {
                            precursorPortalUsed = false;
                        }
                    }

                    // Biome change done
                    if (!biomeChangeDone && newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName() && Player.main.playerController.inputEnabled)
                    {
                        biomeChangeDone = true;
                        UnfreezeStats();
                        Player.main.teleportingLoopSound.Stop();
                        Player.main.OnPlayerPositionCheat();

                        if (newBiome.GetName() == BiomeHandler.ALIENBASE.GetName() && Player.main.GetBiomeString().ToLower().Contains("prison"))
                        {
                            PrecursorAquariumPlatformTrigger trigger = GameObject.FindObjectOfType<PrecursorAquariumPlatformTrigger>();
                            if (trigger)
                            {
                                trigger.OnTriggerEnter(Player.main.gameObject.GetComponent<Collider>());
                            }
                        }
                    }

                    // Death stuff
                    if (isDead && Player.main.playerController.inputEnabled && LargeWorldStreamer.main.IsWorldSettled())
                    {
                        TransitionHandler.SetCurrentBiome(newBiome);
                        UnfreezeStats();
                        Console.WriteLine("HANDLED DEATH");
                        isDead = false;
                    }
                    // Normal update stuff
                    else if (!isDead && Player.main.playerController.inputEnabled)
                    {
                        if (TransitionHandler.GetCurrentBiome().GetName() != newBiome.GetName() && biomeChangeDone)
                        {
                            if (stabilisationCounter > stabilisationTime)
                            {
                                Console.WriteLine("CHANGE FROM " + TransitionHandler.GetCurrentBiome().GetName() + " TO " + newBiome.GetName());

                                Vector3 lastPosition = precursorPortalUsed ? positionBeforePrecursorTeleporter : Player.main.lastPosition;
                                precursorPortalUsed = false;
                                TeleportLocation teleportLocation = TransitionHandler.getTeleportPositionForBiomeTransfer(newBiome, lastPosition);
                                stabilisationCounter = 0;

                                if (teleportLocation == null)
                                {
                                    Console.WriteLine("NOT VALID; IGNORING");
                                    TransitionHandler.SetCurrentBiome(newBiome);
                                    return;
                                }

                                Teleport(teleportLocation);
                            }
                            else
                            {
                                CustomUI.SetBigText("Teleporting in " + Math.Round(stabilisationTime - stabilisationCounter, 0));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to invoke action " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            public static void Teleport(TeleportLocation location)
            {
                if (location == null) return;

                Player.main.playerController.inputEnabled = false;
                Player.main.playerController.SetEnabled(false);

                if (Player.main.GetVehicle())
                {
                    Player.main.GetVehicle().TeleportVehicle(location.GetPosition().ToVector3(), Quaternion.Euler(location.GetRotation().ToVector3()));
                }
                else
                {
                    Player.main.transform.position = location.GetPosition().ToVector3();
                    Player.main.transform.rotation = Quaternion.Euler(location.GetRotation().ToVector3());
                }

                MainCameraControl.main.rotationX = 0;
                MainCameraControl.main.rotationY = 0;
                Player.main.WaitForTeleportation();
                Player.main.OnPlayerPositionCheat();
                Player.main.teleportingLoopSound.Play();
                biomeChangeDone = false;

                Console.WriteLine("TELEPORTING TO " + location.GetBiome().GetName());

                yourBiomeText = "New biome: " + location.GetBiome().GetName() + " (from " + location.GetOrigin().GetName() + ")";
                yourBiomeTextCounter = 600;

                TransitionHandler.SetCurrentBiome(location.GetBiome());
                location.SetKnownToPlayer(true);
                TransitionHandler.GetUndoLocation().SetKnownToPlayer(true);
                SaveTransitionsToFile();
                CustomPDA.CreatePDAEntries(baseDirectory);
                CustomPDA.AddPDAEntry(location.GetBiome(), true);
            }

            public static void FreezeStats()
            {
                Player.main.FreezeStats();
            }

            public static void UnfreezeStats()
            {
                while (Player.main.IsFrozenStats())
                {
                    Player.main.UnfreezeStats();
                }
            }

        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Awake")]
        internal class Player_Awake_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isDead = true;

                Console.WriteLine("PLAYER AWAKE");
                baseDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
                String slot = SaveLoadManager.main.GetCurrentSlot();
                saveFilePath = Path.Combine(baseDirectory, slot + "-data.dat");

                if (SaveLoadManager.main.GetGameInfo(slot) == null || !File.Exists(saveFilePath))
                {
                    // New save, regenerate
                    //Console.WriteLine("GENERATING " + filePath);
                    TransitionHandler.GenerateRandomTransitionMap();
                    SaveTransitionsToFile();
                }
                else
                {
                    // Load
                    //Console.WriteLine("LOADING " + filePath);
                    String base64str = File.ReadAllText(saveFilePath);
                    TransitionHandler.FromBase64String(base64str);
                }
                TransitionHandler.WriteTransitionLog();
                CustomPDA.CreatePDAEntries(baseDirectory);
            }
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("OnKill")]
        internal class Player_Death_Patch
        {

            [HarmonyPostfix]
            public static void Postfix()
            {
                isDead = true;
            }
        }

        [HarmonyPatch(typeof(PrecursorTeleporter))]
        [HarmonyPatch("SetWarpPosition")]
        internal class Teleporter_SetWarpPosition_Patch
        {
            [HarmonyPrefix]
            public static Boolean Prefix()
            {
                precursorPortalUsed = true;
                positionBeforePrecursorTeleporter = Player.main.lastPosition;
                return true;
            }
        }

        public static void SaveTransitionsToFile()
        {
            String base64str = TransitionHandler.ToBase64String();
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
            File.Create(saveFilePath).Close();
            File.WriteAllText(saveFilePath, base64str);
        }

    }

}
