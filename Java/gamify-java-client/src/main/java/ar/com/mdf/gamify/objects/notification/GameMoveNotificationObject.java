package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameMoveNotificationObject extends NotificationObject {
	
	@SerializedName("SessionId")
	private String sessionId;
	
	@SerializedName("PlayerName")
	private String playerName;
	
	@SerializedName("Number")
	private String number;

	public String getSessionId() {
		return sessionId;
	}

	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}

	public String getPlayerName() {
		return playerName;
	}

	public void setPlayerName(String playerName) {
		this.playerName = playerName;
	}

	public String getNumber() {
		return number;
	}

	public void setNumber(String number) {
		this.number = number;
	}

	
}
