
//				socket.On ("reconnect", () => {
//					Log("reconnected");
//					status = TransportStatus.Connected;
//				});
//				socket.On ("connect_error", () => {
//					Log ("connection error");
//					status = TransportStatus.Disconnected;
//				});
//				socket.On ("connect_timeout", () => {
//					Log ("connection timeout");
//					status = TransportStatus.Disconnected;
//				});
//				socket.On ("reconnect_error", () => {
//					Log ("reconnection error");
//				});
//				socket.On ("reconnect_failed", () => {
//					Log ("reconnection failed");
//					status = TransportStatus.Disconnected;
//				});
//				socket.On ("reconnect_attempt", () => {
//					Log ("reconnection attempt");
//					status = TransportStatus.Connecting;
//				});
//				socket.On ("reconnecting", () => {
//					Log ("reconnection attempt");
//					status = TransportStatus.Connecting;
//				});