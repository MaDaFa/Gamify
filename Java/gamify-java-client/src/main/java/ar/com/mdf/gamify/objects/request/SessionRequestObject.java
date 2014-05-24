package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class SessionRequestObject extends PlayerNameRequestObject {

	@SerializedName("SessionId")
	private String sessionId;

	public String getSessionId() {
		return sessionId;
	}

	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}
	
	
}
