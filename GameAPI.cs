/*
 * Made by https://github.com/official-notfishvr
 * https://github.com/official-notfishvr/MIMESIS-Mod-Menu
*/
using Mimic;
using Mimic.Actors;
using Mimic.Audio;
using Mimic.InputSystem;
using ReluProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GameAPI
{
    public static Hub GetHub()
    {
        return Hub.s;
    }

    public static VWorld GetVWorld()
    {
        Hub hub = GetHub();
        if (hub == null) return null;

        return ModHelper.GetFieldValue<VWorld>(hub, "vworld");
    }

    public static VRoomManager GetVRoomManager()
    {
        VWorld vworld = GetVWorld();
        if (vworld == null) return null;

        return ModHelper.GetFieldValue<VRoomManager>(vworld, "VRoomManager");
    }

    public static IVroom GetRoom(long roomID)
    {
        VRoomManager roomManager = GetVRoomManager();
        if (roomManager == null) return null;

        object roomDict = ModHelper.GetFieldValue(roomManager, "_roomDict");
        if (roomDict is System.Collections.IDictionary dict)
        {
            return dict[roomID] as IVroom;
        }
        return null;
    }

    public static IVroom[] GetAllRooms()
    {
        VRoomManager roomManager = GetVRoomManager();
        if (roomManager == null) return new IVroom[0];

        object roomDict = ModHelper.GetFieldValue(roomManager, "_roomDict");
        if (roomDict is System.Collections.IDictionary dict)
        {
            IVroom[] rooms = new IVroom[dict.Count];
            int i = 0;
            foreach (var room in dict.Values)
            {
                rooms[i++] = room as IVroom;
            }
            return rooms;
        }
        return new IVroom[0];
    }

    public static Hub.PersistentData GetPersistentData()
    {
        Hub hub = GetHub();
        if (hub == null) return null;

        return ModHelper.GetFieldValue<Hub.PersistentData>(hub, "pdata");
    }

    public static ProtoActor GetLocalPlayer()
    {
        return FindActorWhere(a => a.AmIAvatar());
    }

    public static ProtoActor[] GetAllPlayers()
    {
        return FindActorsWhere(a => a != null);
    }

    public static ProtoActor[] GetOtherPlayers()
    {
        return FindActorsWhere(a => !a.AmIAvatar());
    }

    public static ProtoActor GetPlayerByName(string name)
    {
        return FindActorWhere(a => a.gameObject.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public static ProtoActor GetPlayerByID(uint actorID)
    {
        return FindActorWhere(a => a.ActorID == actorID);
    }

    public static LootingLevelObject[] GetAllLoot()
    {
        return FindObjectsOfType<LootingLevelObject>().Where(l => l != null && l.gameObject.activeInHierarchy).ToArray();
    }

    public static LootingLevelObject[] GetLootNearby(float maxDistance, Vector3? searchCenter = null)
    {
        Vector3 center = searchCenter ?? GetLocalPlayer()?.transform.position ?? Vector3.zero;
        return FindObjectsOfType<LootingLevelObject>()
            .Where(l => l != null && l.gameObject.activeInHierarchy &&
                   Vector3.Distance(l.transform.position, center) <= maxDistance)
            .ToArray();
    }

    public static LootingLevelObject[] GetLootByName(string name)
    {
        return FindObjectsOfType<LootingLevelObject>()
            .Where(l => l != null && l.gameObject.name.Contains(name))
            .ToArray();
    }

    public static StatManager GetLocalStatManager()
    {
        ProtoActor player = GetLocalPlayer();
        return player?.GetComponent<StatManager>();
    }

    public static StatManager GetStatManager(ProtoActor actor)
    {
        return actor?.GetComponent<StatManager>();
    }

    public static MovementController GetLocalMovementController()
    {
        ProtoActor player = GetLocalPlayer();
        return player?.GetComponent<MovementController>();
    }

    public static MovementController GetMovementController(ProtoActor actor)
    {
        return actor?.GetComponent<MovementController>();
    }

    public static object GetLocalInventory()
    {
        ProtoActor player = GetLocalPlayer();
        return player != null ? ModHelper.GetFieldValue(player, "inventory") : null;
    }

    public static object GetInventory(ProtoActor actor)
    {
        return actor != null ? ModHelper.GetFieldValue(actor, "inventory") : null;
    }

    public static List<InventoryItem> GetInventoryItems(ProtoActor actor)
    {
        if (actor == null) return new List<InventoryItem>();

        object inventory = GetInventory(actor);
        return inventory != null ? ModHelper.GetFieldValue<List<InventoryItem>>(inventory, "SlotItems")
            ?? new List<InventoryItem>() : new List<InventoryItem>();
    }

    public static ProtoActor FindActorWhere(Func<ProtoActor, bool> predicate)
    {
        try
        {
            ProtoActor[] allActors = FindObjectsOfType<ProtoActor>();
            return allActors.FirstOrDefault(predicate);
        }
        catch
        {
            return null;
        }
    }

    public static ProtoActor[] FindActorsWhere(Func<ProtoActor, bool> predicate)
    {
        try
        {
            ProtoActor[] allActors = FindObjectsOfType<ProtoActor>();
            return allActors.Where(predicate).ToArray();
        }
        catch
        {
            return new ProtoActor[0];
        }
    }

    public static T[] FindObjectsOfType<T>() where T : Component
    {
        return UnityEngine.Object.FindObjectsOfType<T>();
    }

    public static T FindObjectOfType<T>() where T : Component
    {
        return UnityEngine.Object.FindObjectOfType<T>();
    }

    public static bool IsPlayerValid(ProtoActor actor)
    {
        return actor != null && actor.gameObject.activeInHierarchy;
    }

    public static bool HasLocalPlayer()
    {
        return GetLocalPlayer() != null;
    }

    public static DataManager GetDataManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<DataManager>(hub, "dataman") : null;
    }

    public static TimeUtil GetTimeUtil()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<TimeUtil>(hub, "timeutil") : null;
    }

    public static NavManager GetNavManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<NavManager>(hub, "navman") : null;
    }

    public static DynamicDataManager GetDynamicDataManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<DynamicDataManager>(hub, "dynamicDataMan") : null;
    }

    public static UIManager GetUIManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<UIManager>(hub, "uiman") : null;
    }

    public static CameraManager GetCameraManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<CameraManager>(hub, "cameraman") : null;
    }

    public static AudioManager GetAudioManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<AudioManager>(hub, "audioman") : null;
    }

    public static InputManager GetInputManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<InputManager>(hub, "inputman") : null;
    }

    public static NetworkManagerV2 GetNetworkManager()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<NetworkManagerV2>(hub, "netman2") : null;
    }

    public static APIRequestHandler GetAPIHandler()
    {
        Hub hub = GetHub();
        return hub != null ? ModHelper.GetFieldValue<APIRequestHandler>(hub, "apihandler") : null;
    }

    public static VPlayer GetVPlayerInRoom(IVroom room, int actorID)
    {
        if (room == null) return null;
        return ModHelper.GetFieldValue<Dictionary<int, VPlayer>>(room, "_vPlayerDict")
            ?.Values
            .FirstOrDefault(p => p.ObjectID == actorID);
    }

    public static List<VPlayer> GetAllVPlayersInRoom(IVroom room)
    {
        if (room == null) return new List<VPlayer>();
        var dict = ModHelper.GetFieldValue<Dictionary<int, VPlayer>>(room, "_vPlayerDict");
        return dict?.Values.ToList() ?? new List<VPlayer>();
    }

    public static List<VActor> GetAllVActorsInRoom(IVroom room)
    {
        if (room == null) return new List<VActor>();
        var dict = ModHelper.GetFieldValue<Dictionary<int, VActor>>(room, "_vActorDict");
        return dict?.Values.ToList() ?? new List<VActor>();
    }

    public static VActor GetVActorInRoom(IVroom room, int actorID)
    {
        if (room == null) return null;
        var dict = ModHelper.GetFieldValue<Dictionary<int, VActor>>(room, "_vActorDict");
        return dict != null && dict.ContainsKey(actorID) ? dict[actorID] : null;
    }

    public static int GetRoomPlayerCount(IVroom room)
    {
        if (room == null) return 0;
        var dict = ModHelper.GetFieldValue<Dictionary<int, VPlayer>>(room, "_vPlayerDict");
        return dict?.Count ?? 0;
    }

    public static int GetRoomActorCount(IVroom room)
    {
        if (room == null) return 0;
        var dict = ModHelper.GetFieldValue<Dictionary<int, VActor>>(room, "_vActorDict");
        return dict?.Count ?? 0;
    }

    public static long GetRoomID(IVroom room)
    {
        if (room == null) return 0;
        return ModHelper.GetFieldValue<long>(room, "RoomID");
    }

    public static int GetRoomMasterID(IVroom room)
    {
        if (room == null) return 0;
        return ModHelper.GetFieldValue<int>(room, "MasterID");
    }

    public static bool IsRoomPlayable(IVroom room)
    {
        if (room == null) return false;
        return ModHelper.InvokeMethod(room, "IsPlayable") is bool result && result;
    }

    public static int GetCurrentGameDay(IVroom room)
    {
        if (room == null) return 0;
        return ModHelper.GetFieldValue<int>(room, "_currentDay");
    }

    public static int GetCurrentSessionCycle(IVroom room)
    {
        if (room == null) return 0;
        return ModHelper.GetFieldValue<int>(room, "_currentSessionCount");
    }

    public static Dictionary<int, ILevelObjectInfo> GetRoomLevelObjects(IVroom room)
    {
        if (room == null) return new Dictionary<int, ILevelObjectInfo>();
        return ModHelper.GetFieldValue<Dictionary<int, ILevelObjectInfo>>(room, "_levelObjects")
            ?? new Dictionary<int, ILevelObjectInfo>();
    }

    public static List<VPlayer> FindPlayersInRoomByName(IVroom room, string name)
    {
        var players = GetAllVPlayersInRoom(room);
        return players.Where(p => p != null).ToList();
    }

    public static List<VPlayer> FindAlivePlayersInRoom(IVroom room)
    {
        var players = GetAllVPlayersInRoom(room);
        return players.Where(p => p != null && p.IsAliveStatus()).ToList();
    }

    public static List<VActor> FindMonstersInRoom(IVroom room)
    {
        var actors = GetAllVActorsInRoom(room);
        return actors.Where(a => a != null && a is VMonster).Cast<VActor>().ToList();
    }

    public static List<VActor> FindAliveMonstersInRoom(IVroom room)
    {
        var monsters = FindMonstersInRoom(room);
        return monsters.Where(m => m != null && m.IsAliveStatus()).ToList();
    }

    public static List<VActor> FindLootingObjectsInRoom(IVroom room)
    {
        var actors = GetAllVActorsInRoom(room);
        return actors.Where(a => a != null && a is VLootingObject).Cast<VActor>().ToList();
    }
}

public static class ModHelper
{
    private const BindingFlags DefaultFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

    public static object GetFieldValue(object target, string fieldName)
    {
        if (target == null) return null;

        FieldInfo field = target.GetType().GetField(fieldName, DefaultFlags);
        return field?.GetValue(target);
    }

    public static object GetFieldValue(Type type, string fieldName)
    {
        FieldInfo field = type.GetField(fieldName, DefaultFlags);
        return field?.GetValue(null);
    }

    public static T GetFieldValue<T>(object target, string fieldName)
    {
        object value = GetFieldValue(target, fieldName);
        return value == null ? default(T) : (T)value;
    }

    public static void SetFieldValue(object target, string fieldName, object value)
    {
        if (target == null) return;

        FieldInfo field = target.GetType().GetField(fieldName, DefaultFlags);
        if (field != null)
        {
            field.SetValue(target, value);
        }
    }

    public static object InvokeMethod(object target, string methodName, params object[] parameters)
    {
        if (target == null) return null;

        MethodInfo method = target.GetType().GetMethod(methodName, DefaultFlags);
        if (method == null) return null;

        return method.Invoke(target, parameters.Length > 0 ? parameters : null);
    }
}