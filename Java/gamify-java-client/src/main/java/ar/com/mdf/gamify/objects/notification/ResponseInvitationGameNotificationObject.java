package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class ResponseInvitationGameNotificationObject extends InvitationToGameNotificationObject {
	
	@SerializedName("Player2Name")
	private String player2Name;

	public String getPlayer2Name() {
		return player2Name;
	}

	public void setPlayer2Name(String player2Name) {
		this.player2Name = player2Name;
	}

}
