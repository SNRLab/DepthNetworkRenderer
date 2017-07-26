using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ImageServer : MonoBehaviour {
    public int ListenPort = 5454;
    public string DataDirectory;
    public RawImage ImageOverlay;

    private IPEndPoint listenEndpoint;

    private UdpClient client;

    private readonly object imageLock = new object();
    private byte[] receivedImage;
    private Vector3 position;
    private Vector3 orientation;


    private bool saveImage;
    private Texture2D currentImage;
    private string dateString;

    private Vector3 zeroPosition = new Vector3(0, 0, 0);

    // Use this for initialization
    public void Start() {
        client = new UdpClient(ListenPort);
        listenEndpoint = new IPEndPoint(IPAddress.Any, ListenPort);
        currentImage = new Texture2D(4, 4);

        print("Listening for data...");
        client.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar) {
        IPEndPoint e = listenEndpoint;
        byte[] buffer = client.EndReceive(ar, ref e);
        var bufferStream = new MemoryStream(buffer);
        var reader = new BinaryReader(bufferStream);

        var length = reader.ReadUInt32();
        var time = reader.ReadDouble();
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var az = reader.ReadSingle();
        var el = reader.ReadSingle();
        var ro = reader.ReadSingle();
        lock (imageLock) {
            receivedImage = reader.ReadBytes((int) (buffer.Length - bufferStream.Position));
            if (receivedImage.Length != length) {
                Debug.LogWarningFormat(
                    "Image length did not match length declared in message: expected={0}, actual={1}", length,
                    receivedImage.Length);
                receivedImage = null;
            }
            position.x = x;
            position.y = y;
            position.z = z;
            orientation.x = az;
            orientation.y = el;
            orientation.z = ro;
        }


//        print(string.Format("{0}, ({1}, {2}. {3}), ({4}, {5}. {6})", time, x, y, z, az, el, ro));

        client.BeginReceive(ReceiveCallback, null);
    }

    // Update is called once per frame
    private void Update() {
        byte[] receivedImage = null;
        lock (imageLock) {
            if (this.receivedImage != null) {
                receivedImage = this.receivedImage;
                this.receivedImage = null;
            }
        }
        if (receivedImage != null) {
            currentImage.LoadImage(receivedImage);

            if (Input.GetKeyDown("z")) {
                zeroPosition = position;
            }

            gameObject.transform.localPosition = position - zeroPosition;
            gameObject.transform.localEulerAngles = orientation;

            ImageOverlay.texture = currentImage;
            ImageOverlay.SetNativeSize();

            if (Input.GetKeyDown("k")) {
                saveImage = true;
                dateString = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff");
                File.WriteAllBytes(Path.Combine(DataDirectory, "rgb_" + dateString + ".jpg"), receivedImage);
            }
        }

//        print(string.Format("image size: ({0}, {1})", currentImage.width, currentImage.height));
    }

    private void LateUpdate() {
        if (saveImage) {
            var renderFile = Path.Combine(DataDirectory, "render_" + dateString + ".png");
            ScreenCapture.CaptureScreenshot(renderFile);
            print("Saved images.");
            saveImage = false;
        }
    }
}