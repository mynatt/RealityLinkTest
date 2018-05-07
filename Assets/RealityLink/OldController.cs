//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using Quobject.SocketIoClientDotNet.Client;
//using RealityLink;
//
//public class SocketIOController : MonoBehaviour {
//	public enum Status {
//		Connected,
//		Reconnecting,
//		Disconnected,
//	};
//
//	public string serverURL = "http://localhost:3000";
//
////	public InputField uiInput = null;
////	public Button uiSend = null;
//	public Text uiChatLog = null;
//	public Text uiID = null;
//	public Text uiDebug = null;
//
//	public GameObject indicator = null;
//	protected Spindicator indicatorSpin = null;
//
//	public Status connection = Status.Reconnecting;
//	protected Coroutine reconnectCoroutine = null;
//	protected int reconnectAttempts = 0;
//	public int reconnectAttemptsMax = 5;
//
//	protected Socket socket = null;
//	protected object chatLock = null;
//	protected string userID = null;
//	protected Queue<string> chatLog = new Queue<string> ();
//	protected bool chatChanged = false;
//	protected int chatLines = 5;
//
//	protected float messageRate = 0.0f;
//	protected int messageCount = 0;
//	protected float messageAcc = 0.0f;
//
//	public string username = "Unity Guest";
//	public string session = "Session";
//
//	public object mutexSEComponents = null;
//	internal Queue<ISEComponent> queueSEComponents = new Queue<ISEComponent> ();
//	public SECSpace space = null;
//	public SECObjectContainer objectContainer = null;
//	public SECTracker tracker = null;
//
//	void Destroy() {
//		DoClose ();
//	}
//
//	void OnApplicationQuit() {
//		DoClose();
//	}
//
//	// Use this for initialization
//	void Start () {
//		if (indicator) {
//			indicatorSpin = indicator.GetComponent<Spindicator>();
//		}
//		chatLock = new object ();
//		mutexSEComponents = new object ();
//
////		chatLock = new object();
////		Text text = uiChatLog.GetComponent<Text>();
//		// chatLines = text.() / (text.fontSize * text.lineSpacing);
//
//		uiChatLog.text = "Not connected.";
//		reconnectCoroutine = StartCoroutine(WaitConnect());
//
//		DoOpen();
//
////		uiSend.onClick.AddListener(() => {
////			SendChat(uiInput.text);
////		});
//	}
//
//	// Update is called once per frame
//	void Update () {
//		messageAcc += Time.deltaTime;
//		lock (chatLock) {
//			if (chatChanged) {
//				string str = "";
//				while (chatLog.Count > chatLines) {
//					chatLog.Dequeue();
//				}
//				foreach (var s in chatLog) {
//					str = str + "\n" + s;
//				}
//				uiChatLog.text = str;
//				chatChanged = false;
//			}
//			if (messageAcc >= 1) {
//				messageRate = Mathf.RoundToInt(messageCount / messageAcc);
//				messageAcc = 0;
//				messageCount = 0;
//
//				uiDebug.text = chatLog.Count + "M | " + messageRate.ToString() + " m/sec";
//			}
//		}
//
//		if (connection == Status.Connected) {
//			if (uiID.text == "") {
//				uiID.text = "ID: " + userID;
//			}
//			UpdateIndicator(1.0f);
//		} else if (connection == Status.Reconnecting) {
//			uiID.text = "";
//			UpdateIndicator(0.2f);
//			if (reconnectCoroutine == null) {
//				reconnectAttempts = 0;
//				reconnectCoroutine = StartCoroutine(Reconnect());
//			}
//		} else if (connection == Status.Disconnected) {
//			uiID.text = "";
//			UpdateIndicator(0.0f);
//		}
//		lock (mutexSEComponents) {
//			while (queueSEComponents.Count > 0) {
//				var component = queueSEComponents.Dequeue ();
//				component.SECUpdate ();
//			}
//		}
//	}
//
//	void OnGUI() {
////		if (uiInput.isFocused &&
////			uiInput.text != "" &&
////			Input.GetKey(KeyCode.Return)) {
////			SendChat(uiInput.text);
////		}
//	}
//
//	void ChatMessage(string message) {
//		lock (chatLock) {
//			// Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
//			chatLog.Enqueue(message);
//			messageCount++;
//			chatChanged = true;
//		}
//	}
//
//	void DoOpen() {
//		if (socket == null) {
//			// var options = new IO.Options();
//			// options.Reconnection = true;
//
//			socket = IO.Socket(serverURL); // , options);
//			socket.On("connect", () => {
//				connection = Status.Connected;
//				ChatMessage("<i>Connected.</i>");
//
//				DoJoin();
//			});
//			socket.On("disconnect", () => {
//				ChatMessage("<i>Disconnected.</i>");
//				connection = Status.Reconnecting;
//			});
//			socket.On("session joined", () => {
//				ChatMessage("<b>Joined " + session + "</b>");
//			});
//
//			socket.On("session ready", (data) => {
//				ChatMessage("<b>The server is ready.</b>");
//				socket.Emit("request space");
//				socket.Emit("request objects");
//				socket.Emit("request tracker");
//				// PacketChat chat = JsonUtility.FromJson<PacketChat>(data);
//				// PacketReady ready = JsonUtility.FromJson<PacketReady>(data);
//			});
//
//			socket.On("provide space", (data) => {
//				ChatMessage("Given space.");
//				string dataStr = data.ToString();
////				Debug.Log(space.SECEncode());
//				space.SECDecode(dataStr);
////				Debug.Log(space.SECEncode());
//				lock (mutexSEComponents) {
//					queueSEComponents.Enqueue(space);
//				}
//			});
//
//			socket.On ("provide objects", (data) => {
//				ChatMessage("Given objects.");
//
//				string dataStr = data.ToString();
//				objectContainer.SECDecode(dataStr);
//
//				lock (mutexSEComponents) {
//					queueSEComponents.Enqueue(objectContainer);
//				}
//			});
//
//			socket.On ("provide tracker", (data) => {
//				ChatMessage("Given tracker.");
//
//				string dataStr = data.ToString();
//				tracker.SECDecode(dataStr);
//
//				lock (mutexSEComponents) {
//					queueSEComponents.Enqueue(tracker);
//				}
//			});
//
//			socket.On ("tick", (data) => {
//				string dataStr = data.ToString();
//				objectContainer.SECDecodeDelta(dataStr);
//				tracker.SECDecode(dataStr);
//				lock (mutexSEComponents) {
//					queueSEComponents.Enqueue(objectContainer);
//					queueSEComponents.Enqueue(tracker);
//				}
////				ChatMessage("tick");
//			});
//
//			// socket.On("chat", (data) => {
//			// 	string str = data.ToString();
//			// 	ChatData chat = JsonUtility.FromJson<ChatData>(str);
//			// 	ChatMessage("<b>user " + chat.id + "</b>: " + chat.msg);
//			// });
//			// socket.On("id", (id) => {
//			// 	userID = id.ToString();
//			// });
//			// socket.On("join", (id) => {
//			// 	ChatMessage("<b>user " + id.ToString() + " has joined</b>");
//			// });
//			// socket.On("leave", (id) => {
//			// 	ChatMessage("<b>user " + id.ToString() + " has left</b>");
//			// });
//		}
//	}
//
//	void DoClose() {
//		if (socket != null) {
//			socket.Disconnect ();
//			socket = null;
//		}
//	}
//
//	void DoJoin() {
//		PacketJoin join = new PacketJoin(username, session);
//
//		string payload = join.encode();
//
//		ChatMessage ("Attempting join..."); //: " + payload);
//		socket.Emit("session join", payload);
//	}
//
//	void UpdateIndicator(float speed) {
//		if (indicatorSpin) {
//			indicatorSpin.setSpeed(speed);
//		}
//	}
//
//	void SendChat(string str) {
//		if (socket != null) {
//			socket.Emit ("chat", str);
//		}
////		uiInput.text = "";
////		uiInput.ActivateInputField();
//	}
//
//	IEnumerator Reconnect() {
//		if (connection == Status.Connected) {
//			reconnectCoroutine = null;
//			yield break;
//		}
//
//		if (++reconnectAttempts > reconnectAttemptsMax) {
//			ChatMessage("<b>Failed to reconnect.</b>");
//			connection = Status.Disconnected;
//			reconnectCoroutine = null;
//			yield break;
//		}
//		if (socket == null) {
//			ChatMessage("<b>Attempting connection.</b>");
//			DoOpen();
//		} else {
//			ChatMessage("<b>Attempting reconnection. (" + reconnectAttempts + ")</b>");
//			socket.Connect();
//		}
//
//		// ChatMessage("Waiting to reattempt.");
//		yield return new WaitForSeconds(5.0f);
//		reconnectCoroutine = StartCoroutine(Reconnect());
//	}
//
//	IEnumerator WaitConnect() {
//		yield return new WaitForSeconds(1.0f);
//		reconnectCoroutine = null;
//	}
//}
