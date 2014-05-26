package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameObject {
	@SerializedName("Player1Name")
	private String player1Name;
	@SerializedName("Player2Name")
	private String player2Name;
	@SerializedName("SessionId")
	private String sessionId;
	
	public String getPlayer1Name() {
		return player1Name;
	}
	public void setPlayer1Name(String player1Name) {
		this.player1Name = player1Name;
	}
	public String getPlayer2Name() {
		return player2Name;
	}
	public void setPlayer2Name(String player2Name) {
		this.player2Name = player2Name;
	}
	public String getSessionId() {
		return sessionId;
	}
	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}
}
