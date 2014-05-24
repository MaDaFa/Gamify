package ar.com.mdf.gamify.enums;

import java.util.HashMap;
import java.util.Map;

public enum RequestType {

	   PlayerConnect (1),
       CreateGame (2),
       GameAccepted (3),
       GameRejected (4),
       GameMove (5),
       AbandonGame (6),
       GetConnectedPlayers (7),
       GetActiveGames (8),
       OpenGame (9),
       Message (10),
       TypingMessage (11),
       PlayerDisconnect (255);
	
	private Integer type;
	private static Map<Integer, RequestType> map = null;
	
	static {
		RequestType[] notificationType = RequestType.values();
		map = new HashMap<Integer, RequestType>();
		for (int i = 0; i < notificationType.length; i++) {
			map.put(notificationType[i].type, notificationType[i]);
		}
	}
	
	public static RequestType getNotificationTypeByType(int value){
		return map.get(value);
	}
	
	private RequestType(int type) {
		this.type = type;
	}

	public Integer getType() {
		return type;
	}
	
	
}
