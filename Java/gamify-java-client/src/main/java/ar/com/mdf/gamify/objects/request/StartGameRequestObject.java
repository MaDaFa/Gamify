package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class StartGameRequestObject extends PlayerNameRequestObject {
	
	
	@SerializedName("InvitedPlayerName")
	private String invitedPlayerName;
	@SerializedName("AdditionalInformation")
	private String additionalInformation;
	
	public String getInvitedPlayerName() {
		return invitedPlayerName;
	}

	public void setInvitedPlayerName(String invitedPlayerName) {
		this.invitedPlayerName = invitedPlayerName;
	}

	public String getAdditionalInformation() {
		return additionalInformation;
	}

	public void setAdditionalInformation(String additionalInformation) {
		this.additionalInformation = additionalInformation;
	}

	
}
