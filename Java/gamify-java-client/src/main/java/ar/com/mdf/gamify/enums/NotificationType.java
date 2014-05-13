package ar.com.mdf.gamify.enums;

import java.util.HashMap;
import java.util.Map;

public enum NotificationType {

	PlayerConnected (1),
	GameInvite (2),
	GameCreated (3),
	GameRejected (4),
	GameMove (5),
	GameMoveResult (6),
	GameAbandoned (7),
	SendConnectedPlayers (8),
	SendActiveGames (9),
	SendGameInformation (10),
	GameFinished (11),
	Message (12),
	TypingMessage (13),
	PlayerDisconnected (14),
	Error (255);
	
	private Integer type;
	private static Map<Integer, NotificationType> map = null;
	
	static {
		NotificationType[] estadosAfiliacion = NotificationType.values();
		map = new HashMap<Integer, NotificationType>();
		for (int i = 0; i < estadosAfiliacion.length; i++) {
			map.put(estadosAfiliacion[i].type, estadosAfiliacion[i]);
		}
	}
	
	public static NotificationType getNotificationTypeByType(int value){
		return map.get(value);
	}
	
	private NotificationType(int type) {
		this.type = type;
	}
}
