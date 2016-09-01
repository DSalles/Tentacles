using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using KinectLink;

public class KinectBodiesReceiver : MonoBehaviour
{
    const byte CMD_CAPTURE_FULL_FRAME = 0x01;
const byte CMD_START_SESSION = 0x02;
const byte CMD_STOP_SESSION = 0x03;
const byte CMD_SUBSCRIBE = 0x10;
const byte CMD_UNSUBSCRIBE = 0x11;
const byte CMD_SOUND = 0x20;

static string GetLocalIPAddress()
{
var host = Dns.GetHostEntry(Dns.GetHostName());
foreach (var ip in host.AddressList)
{
if (ip.AddressFamily == AddressFamily.InterNetwork)
{
return ip.ToString();
}
}
throw new System.Exception("Local IP Address Not Found!");
}

[System.Serializable]
public class KinectInfo
{
    public string name;
    public Transform location;
    public bool useReportedLocation = true;
    public BodyFilter filter;
    public byte id;


    [System.NonSerialized]
    public IPAddress address;
    [System.NonSerialized]
    public int port;
    [System.NonSerialized]
    public IPEndPoint endPoint;
    [System.NonSerialized]
    public bool active;
    [System.NonSerialized]
    public Vector3 position = Vector3.zero;
    [System.NonSerialized]
    public Quaternion rotation = Quaternion.identity;
    [System.NonSerialized]
    public Vector3 scale = Vector3.one;



    [System.NonSerialized]
    public float lastDataTime;

    [HideInInspector]
    public Matrix4x4 matrix;

    //public string ipString = "";

    public void UseReportedLocation(Transform parent = null)
    {
        if (location == null)
        {
            location = (new GameObject("Kinect[" + id + "]")).transform;
        }

        location.parent = parent;

        location.localPosition = position;
        location.localRotation = rotation;
        location.localScale = scale;

        UpdateMatrix();
    }

    public void UpdateMatrix()
    {
        matrix = location.localToWorldMatrix;
    }

}

public class BodyFrameData
{

    public static BodyFrameData Blank
    {
        get
        {
            return new BodyFrameData();
        }
    }

    public static BodyFrameData GetBlankWithID(byte id)
    {
        return new BodyFrameData(id);
    }

    public StreamState streamState;
    public BodyData[] bodyData;
    public byte kinectId;

    public BodyFrameData()
    {
        bodyData = new BodyData[0];
        kinectId = 0;
    }

    public BodyFrameData(byte id)
    {
        bodyData = new BodyData[0];
        kinectId = id;
    }

    public void Process(byte[] data)
    {
        MemoryStream stream = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(stream);

        //header
        byte header = reader.ReadByte();
        if (header != 0xFF)
        {
            //uh oh
            Debug.LogError("Header incorrect!");
        }

        //id
        kinectId = reader.ReadByte();

        //state
        streamState = (StreamState)reader.ReadByte();

        //skeletonCount
        int count = (int)reader.ReadByte();

        if (bodyData.Length != count)
        {
            System.Array.Resize<BodyData>(ref bodyData, count);
            //bodyData = new BodyData[count];
        }


        for (int i = 0; i < count; i++)
        {
            if (bodyData[i] == null)
                bodyData[i] = new BodyData(reader, kinectId);
            else
                bodyData[i].Deserialize(reader, kinectId);
        }
    }

}


public class BodyData
{
    public byte kinectId;
    public ulong trackingId;
    public Vector3 position;
    public TrackingState trackingState;
    public Vector3 spineMidPosition;
    public TrackingState spineMidState;
    public Vector3 spineShoulderPosition;
    public TrackingState spineShoulderState;
  

        public Vector3 headPosition;
    public TrackingState headState;


    public HandState handLeftState;
    public TrackingConfidence handLeftConfidence;
    public TrackingState handLeftTrackingState;
    public Vector3 handLeftPosition;
      
    public HandState handRightState;
    public TrackingConfidence handRightConfidence;
    public TrackingState handRightTrackingState;
    public Vector3 handRightPosition;
    public bool modifiedByMatrix;
    public bool valid;

    public BodyData(BinaryReader reader, byte kinectId)
    {
        this.kinectId = kinectId;
        trackingId = 0;
        position = Vector3.zero;
        trackingState = 0;
        spineMidPosition = Vector3.zero;
        spineMidState = 0;
        spineShoulderPosition = Vector3.zero;
        spineShoulderState = 0;

        headPosition = Vector3.zero;
        headState = 0;

        handLeftState = HandState.NotTracked;
        handLeftConfidence = TrackingConfidence.Low;
        handLeftTrackingState = TrackingState.NotTracked;
        handLeftPosition = Vector3.zero;
           
        handRightState = HandState.NotTracked;
        handRightConfidence = TrackingConfidence.Low;
        handRightTrackingState = TrackingState.NotTracked;
        handRightPosition = Vector3.zero;
        valid = true;

        Deserialize(reader, kinectId);
    }

    public void Deserialize(BinaryReader reader, byte kinectId)
    {
        this.kinectId = kinectId;
        trackingId = reader.ReadUInt64(); //WTF
        ReadJointData(reader, ref trackingState, ref position);
        ReadJointData(reader, ref spineMidState, ref spineMidPosition);
        ReadJointData(reader, ref spineShoulderState, ref spineShoulderPosition);

            ReadJointData(reader, ref headState, ref headPosition);

        handLeftState = (HandState)reader.ReadByte();
        handLeftConfidence = (TrackingConfidence)reader.ReadByte();
        ReadJointData(reader, ref handLeftTrackingState, ref handLeftPosition);

        handRightState = (HandState)reader.ReadByte();
        handRightConfidence = (TrackingConfidence)reader.ReadByte();
        ReadJointData(reader, ref handRightTrackingState, ref handRightPosition);
        modifiedByMatrix = false;
        valid = true;
    }

    void ReadJointData(BinaryReader reader, ref TrackingState state, ref Vector3 position)
    {
        state = (TrackingState)reader.ReadByte();
        position.x = reader.ReadSingle();
        position.y = reader.ReadSingle();
        position.z = reader.ReadSingle();
    }

    public string TrackingString
    {
        get
        {
            return trackingId.ToString() + "_" + kinectId;
        }
    }

    public override string ToString()
    {
        return string.Format("[BodyData] ID: {0}      {1},     {2},    {3}", trackingId, position.x, position.y, position.z);
    }
}

public class KinectData
{
    public byte id;
    public IPAddress ipAddress;
    public Vector3 position;
    public Vector3 rotation;
}

public delegate void BodyFrameDataDelegate(BodyFrameData frame);
public event BodyFrameDataDelegate OnBodyFrameDataReceived;

public static KinectBodiesReceiver Instance;
UdpClient client;
UdpClient discoveryClient;
byte[] discoveryPayload;
IPAddress ipAddress;
//public string ipAddressStr = "239.200.200.200";
public int port = 55555;
public int commandPort = 51000;
int discoveryPort = 50665;

public float blankFrameDelay = 0.25f;
//float nextBlankFrameTime = 0;

public float lastPacketTime;
public Dictionary<byte, StreamState> streamStateTable = new Dictionary<byte, StreamState>();
public Dictionary<byte, string> streamAddressTable = new Dictionary<byte, string>();
Dictionary<byte, float> streamBlankTable = new Dictionary<byte, float>();

//IPEndPoint multiCastEndPoint;
IPEndPoint sender;
BodyFrameData currentFrame;
Dictionary<byte, BodyFrameData> currentFrames = new Dictionary<byte, BodyFrameData>();
public BodyFrameData CurrentFrame
{
    get
    {
        return currentFrame;
    }
}

public BodyFrameData GetCurrentFrame(byte kinectId)
{
    return currentFrames[kinectId];
}

//TODO:  Update matrix to include multiple Kinect ID's with new SensorLocation pattern
//matrix transforms

public bool disregardUnknownKinects = true;
public List<KinectInfo> kinectInfo;
public Dictionary<byte, KinectInfo> kinectInfoTable = new Dictionary<byte, KinectInfo>();

//public Matrix4x4 bodyToWorldMatrix;
//public Transform sensorLocation;

//session serialization
public string sessionDirectory;
public string sessionFilename;
MemoryStream sessionStream;
BinaryWriter sessionWriter;
BinaryReader sessionReader;
float sessionStartTime;
public bool recordingSession;
public bool playingSession;

void Awake()
{
    Instance = this;

    sessionDirectory = PlayerPrefs.GetString("SessionPath", Application.dataPath);
    sessionFilename = PlayerPrefs.GetString("SessionFilename", "");
}

public void Start()
{
    currentFrame = new BodyFrameData();
    sender = new IPEndPoint(IPAddress.Any, 1);
    client = new UdpClient();
    client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

    discoveryClient = new UdpClient();
    discoveryClient.Client.Bind(new IPEndPoint(IPAddress.Any, discoveryPort));
    discoveryClient.EnableBroadcast = true;

    BinaryWriter writer = new BinaryWriter(new MemoryStream());
    string localIPStr = GetLocalIPAddress();
    writer.Write((byte)0x00);
    writer.Write(localIPStr);
    writer.Write(port);

    discoveryPayload = (writer.BaseStream as MemoryStream).ToArray();


    foreach (var ki in kinectInfo)
    {
        kinectInfoTable.Add(ki.id, ki);
    }
}

void OnDisable()
{
   // if (recordingSession)
   //     StopSession();
}

void Update()
{
    //UpdateMatrix();
    while (client.Available > 0)
    {
        byte[] arr = client.Receive(ref sender);

        switch (arr[0])
        {
            //process body packet
            case 0xFF:
                if (!playingSession)
                    HandleBodyPacket(arr);
                break;
            case 0xFD:
                //heartbeat
                if (!playingSession)
                    HandleHearbeatPacket(arr);
                break;
            default:
                continue;
        }
    }

    while (discoveryClient.Available > 0)
    {
        HandleDiscoveryPacket(discoveryClient.Receive(ref sender));

    }

    List<byte> blankIdUpdates = new List<byte>();

    foreach (var pair in streamBlankTable)
    {
        if (Time.time > pair.Value)
        {
            if (OnBodyFrameDataReceived != null)
            {
                OnBodyFrameDataReceived(BodyFrameData.GetBlankWithID(pair.Key));
            }

            blankIdUpdates.Add(pair.Key);
        }
    }

    foreach (var b in blankIdUpdates)
        streamBlankTable[b] = Time.time + blankFrameDelay;
}

static Vector3 ReadVector3(BinaryReader reader)
{
    float x = reader.ReadSingle();
    float y = reader.ReadSingle();
    float z = reader.ReadSingle();

    return new Vector3(x, y, z);
}

void HandleDiscoveryPacket(byte[] data)
{
    BinaryReader reader = new BinaryReader(new MemoryStream(data));

    //header
    reader.ReadByte();
    byte kinectId = reader.ReadByte();
    string ipStr = reader.ReadString();
    int commandPort = reader.ReadInt32();
    Vector3 pos = ReadVector3(reader);
    Quaternion rot = Quaternion.Euler(ReadVector3(reader));
    Vector3 scale = ReadVector3(reader);

    KinectInfo ki = null;

    if (kinectInfoTable.ContainsKey(kinectId))
    {
        ki = kinectInfoTable[kinectId];
        ki.UpdateMatrix();
    }
    else if (!disregardUnknownKinects)
    {
        ki = new KinectInfo();
        kinectInfo.Add(ki);
        kinectInfoTable.Add(kinectId, ki);
        ki.id = kinectId;
        ki.useReportedLocation = true;
        ki.UseReportedLocation(transform);
    }

    if (ki != null)
    {
        ki.id = kinectId;
        ki.address = IPAddress.Parse(ipStr);
        ki.port = commandPort;
        ki.endPoint = new IPEndPoint(ki.address, ki.port);
        ki.position = pos;
        ki.rotation = rot;
        ki.scale = scale;


        if (ki.useReportedLocation)
            ki.UseReportedLocation(ki.location == null ? transform : ki.location.parent);
        //ki.ipString = ki.address.ToString() + ":" + ki.port;

        //TODO: auto subscribe toggle?
        if (!ki.active)
            Subscribe(ki);
    }
}

void Subscribe(KinectInfo info)
{
    discoveryPayload[0] = CMD_SUBSCRIBE;
    discoveryClient.Send(discoveryPayload, discoveryPayload.Length, info.endPoint);
}

void Unsubscribe(KinectInfo info)
{
    discoveryPayload[0] = CMD_UNSUBSCRIBE;
    discoveryClient.Send(discoveryPayload, discoveryPayload.Length, info.endPoint);
}

void HandleHearbeatPacket(byte[] data)
{
    lastPacketTime = Time.time;

    MemoryStream stream = new MemoryStream(data);
    BinaryReader reader = new BinaryReader(stream);
    byte header = reader.ReadByte();
    //id
    byte kinectId = reader.ReadByte();
    //state
    var streamState = (StreamState)reader.ReadByte();

    //TODO: maybe timestamp it
    if (kinectInfoTable.ContainsKey(kinectId))
    {
        kinectInfoTable[kinectId].active = true;
        kinectInfoTable[kinectId].lastDataTime = Time.time;
    }



    streamStateTable[kinectId] = streamState;
    streamAddressTable[kinectId] = sender.Address.ToString();
}

void HandleBodyPacket(byte[] data)
{
    lastPacketTime = Time.time;
       
        byte kinectId = data[1];


    if (currentFrames.ContainsKey(kinectId))
    {
        currentFrame = GetCurrentFrame(kinectId);
    }
    else
    {
        currentFrame = new BodyFrameData(kinectId);
        currentFrames.Add(kinectId, currentFrame);
    }

    //deprecated 
    currentFrame.Process(data);
    streamStateTable[currentFrame.kinectId] = currentFrame.streamState;
    streamAddressTable[currentFrame.kinectId] = sender.Address.ToString();

    //TODO: maybe timestamp it
    if (kinectInfoTable.ContainsKey(currentFrame.kinectId))
    {
        var ki = kinectInfoTable[currentFrame.kinectId];
        ki.active = true;
        ki.lastDataTime = Time.time;

        if (ki.filter != null)
            ki.filter.Validate(ki, currentFrame.bodyData);
    }




    if (OnBodyFrameDataReceived != null)
        OnBodyFrameDataReceived(currentFrame);

    if (recordingSession)
    {
        //sessionWriter.Write(Time.realtimeSinceStartup - sessionStartTime);
        //sessionWriter.Write(data.Length);
        //sessionWriter.Write(data);
    }

    streamBlankTable[currentFrame.kinectId] = Time.time + blankFrameDelay;
    //nextBlankFrameTime = Time.time + blankFrameDelay;
}

public void PlayIdentification(byte kinectId)
{
    PlaySound(((int)kinectId).ToString(), kinectId);
}

public void PlaySound(string sound, byte kinectId = 0xFF)
{
    MemoryStream stream = new MemoryStream();
    BinaryWriter writer = new BinaryWriter(stream);

    writer.Write(CMD_SOUND);
    writer.Write(sound);

    byte[] data = stream.ToArray();

    if (kinectId != 0xFF)
    {
        //TODO: cache IP :(
        IPAddress targetAddress = IPAddress.Parse(streamAddressTable[kinectId]);
        client.Send(data, data.Length, new IPEndPoint(targetAddress, commandPort));
    }
    else
    {
        foreach (var str in streamAddressTable.Values)
        {
            IPAddress targetAddress = IPAddress.Parse(str);
            client.Send(data, data.Length, new IPEndPoint(targetAddress, commandPort));
        }
    }
}


//TODO: enable targeting arbitrary KinectStream server
//public void CaptureFullFrame(string session = "", string name = "", byte kinectId = 0xFF)
//{
//    MemoryStream stream = new MemoryStream();
//    BinaryWriter writer = new BinaryWriter(stream);

//    //TODO: const or enum these op codes
//    writer.Write(CMD_CAPTURE_FULL_FRAME);
//    writer.Write(session);
//    writer.Write(name);

//    byte[] data = stream.ToArray();
      
//    IPAddress targetAddress = kinectId == 0xFF ? sender.Address : IPAddress.Parse(streamAddressTable[kinectId]);
//    client.Send(data, data.Length, new IPEndPoint(targetAddress, commandPort));
//}

//public void StartSession(string session, string name = "", float maxDuration = 0, byte kinectId = 0xFF)
//{
//    MemoryStream stream = new MemoryStream();
//    BinaryWriter writer = new BinaryWriter(stream);

//    //TODO: const or enum these op codes
//    writer.Write(CMD_START_SESSION);
//    writer.Write(session);
//    writer.Write(name);
//    writer.Write(maxDuration);

//    byte[] data = stream.ToArray();
//    IPAddress targetAddress = kinectId == 0xFF ? sender.Address : IPAddress.Parse(streamAddressTable[kinectId]);
//    client.Send(data, data.Length, new IPEndPoint(targetAddress, commandPort));

//    recordingSession = true;
//}

//public void StopSession(byte kinectId = 0xFF)
//{
//    MemoryStream stream = new MemoryStream();
//    BinaryWriter writer = new BinaryWriter(stream);

//    //TODO: const or enum these op codes
//    writer.Write(CMD_STOP_SESSION);

//    byte[] data = stream.ToArray();
//    IPAddress targetAddress = kinectId == 0xFF ? sender.Address : IPAddress.Parse(streamAddressTable[kinectId]);
//    client.Send(data, data.Length, new IPEndPoint(targetAddress, commandPort));

//    recordingSession = false;
//}

//[System.Obsolete]
//public void StartRecording(string filename = "")
//{

//    if (!Directory.Exists(sessionDirectory))
//    {
//        if (sessionDirectory == "" || sessionDirectory == null)
//        {
//            Debug.LogError("Session Directory cannot be null");
//            return;
//        }
//        if (!Directory.Exists(sessionDirectory))
//            Directory.CreateDirectory(sessionDirectory);
//    }


//    if (playingSession)
//    {
//        Debug.LogError("Cannot start recording while playing a session!");
//        return;
//    }


//    if (filename == "")
//    {
//        var dt = System.DateTime.Now;
//        sessionFilename = string.Format("{0}-{1}-{2}_{3}-{4}.bsd", dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute);
//    }
//    else
//    {
//        sessionFilename = filename;
//    }

//    Debug.Log("Start Recording: " + Path.Combine(sessionDirectory, sessionFilename));
//    PlayerPrefs.SetString("SessionPath", sessionDirectory);
//    PlayerPrefs.SetString("SessionFilename", sessionFilename);

//    recordingSession = true;
//    sessionStream = new MemoryStream();
//    sessionWriter = new BinaryWriter(sessionStream);
//    sessionWriter.Write((float)0f);
//    sessionStartTime = Time.realtimeSinceStartup;
//}

//[System.Obsolete]
//public void StopRecording()
//{
//    Debug.Log("Stop Recording: " + Path.Combine(sessionDirectory, sessionFilename));
//    sessionStream.Position = 0;
//    //write duration
//    sessionWriter.Write(Time.realtimeSinceStartup - sessionStartTime);

//    recordingSession = false;
//    string path = Path.Combine(sessionDirectory, sessionFilename);

//    File.WriteAllBytes(path, sessionStream.ToArray());
//    sessionWriter.Flush();
//    sessionWriter.Close();
//    sessionStream.Flush();
//    sessionStream.Dispose();
//    sessionStream = null;
//}

//public void StartPlayback(string filename = "")
//{
//    if (recordingSession)
//    {
//        Debug.LogError("Cannot start playback while recording!");
//        return;
//    }

//    if (filename != "")
//        sessionFilename = filename;

//    string path = Path.Combine(sessionDirectory, sessionFilename);
//    sessionStream = new MemoryStream(File.ReadAllBytes(path));
//    sessionReader = new BinaryReader(sessionStream);
//    sessionStartTime = Time.realtimeSinceStartup;

//    StartCoroutine("PlaybackRoutine");
//}

//public void StopPlayback()
//{
//    StopCoroutine("PlaybackRoutine");
//    sessionReader.Close();
//    sessionStream.Close();
//    sessionStream.Flush();
//    sessionReader = null;
//    sessionStream = null;
//    playingSession = false;
//}

//public float playbackSessionDuration;
//public float playbackSessionTime;
//IEnumerator PlaybackRoutine()
//{
//    playingSession = true;

//    PlayerPrefs.SetString("SessionPath", sessionDirectory);
//    PlayerPrefs.SetString("SessionFilename", sessionFilename);

//    long endPos = sessionStream.Length;
//    float nextEntryTime = 0;

//    float duration = sessionReader.ReadSingle();
//    playbackSessionDuration = duration;
//    float sessionTime = 0;
//    while (sessionStream.Position < endPos)
//    {
//        //read next entry
//        nextEntryTime = sessionReader.ReadSingle();
//        int dataLen = sessionReader.ReadInt32();
//        byte[] data = sessionReader.ReadBytes(dataLen);

//        //wait until frame was delivered
//        while (sessionTime < nextEntryTime)
//        {
//            sessionTime = Time.realtimeSinceStartup - sessionStartTime;
//            playbackSessionTime = sessionTime;
//            yield return null;
//        }


//        HandleBodyPacket(data);
//        yield return null;
//    }

//    playingSession = false;

//}

public static string GetFormattedTimestamp()
{
    var dt = System.DateTime.Now;
    return dt.ToString("yyyy-MM-dd_HH-mm-ss");
}
}
