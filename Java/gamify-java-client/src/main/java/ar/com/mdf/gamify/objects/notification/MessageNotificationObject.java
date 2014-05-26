package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class MessageNotificationObject extends NotificationObject {
	@SerializedName("FromPlayerName")
	private String fromPlayerName;

	public String getFromPlayerName() {
		return fromPlayerName;
	}

	public void setFromPlayerName(String fromPlayerName) {
		this.fromPlayerName = fromPlayerName;
	}
}
