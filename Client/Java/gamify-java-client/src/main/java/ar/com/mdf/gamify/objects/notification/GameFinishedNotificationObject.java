package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameFinishedNotificationObject extends NotificationObject {
	
	@SerializedName("SessionId")
	private String sessionId;
	
	@SerializedName("WinnerPlayerName")
	private String winnerPlayerName;
	
	@SerializedName("LooserPlayerName")
	private String looserPlayerName;

	public String getSessionId() {
		return sessionId;
	}

	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}

	public String getWinnerPlayerName() {
		return winnerPlayerName;
	}

	public void setWinnerPlayerName(String winnerPlayerName) {
		this.winnerPlayerName = winnerPlayerName;
	}

	public String getLooserPlayerName() {
		return looserPlayerName;
	}

	public void setLooserPlayerName(String looserPlayerName) {
		this.looserPlayerName = looserPlayerName;
	}
	
	
}
