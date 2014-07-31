package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameAbandonedNotificationObject extends ConnectionStatePlayerNotificationObject {
	@SerializedName("SessionId")
	private String sessionId;

	public String getSessionId() {
		return sessionId;
	}

	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}
	
}
