package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class InvitationToGameNotificationObject extends NotificationObject {
	
	@SerializedName("Player1Name")
	private String player1Name;
	
	@SerializedName("SessionId")
	private String sessionId;
	
	@SerializedName("AdditionalInformation")
	private String additionalInformation;

	public String getPlayer1Name() {
		return player1Name;
	}

	public void setPlayer1Name(String player1Name) {
		this.player1Name = player1Name;
	}

	public String getSessionId() {
		return sessionId;
	}

	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}

	public String getAdditionalInformation() {
		return additionalInformation;
	}

	public void setAdditionalInformation(String additionalInformation) {
		this.additionalInformation = additionalInformation;
	}

}
