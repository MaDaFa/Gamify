package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public abstract class NotificationObject {
	
	@SerializedName("Message")
	private String message;

	public String getMessage() {
		return message;
	}

	public void setMessage(String message) {
		this.message = message;
	}
	
	

}
