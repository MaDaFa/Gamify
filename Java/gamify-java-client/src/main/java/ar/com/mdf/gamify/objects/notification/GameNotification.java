package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameNotification {
	
	@SerializedName("Type")
	private Integer type;
	@SerializedName("SerializedNotificationObject")
	private String serializedNotificationObject;
	
	public Integer getType() {
		return type;
	}
	public void setType(Integer type) {
		this.type = type;
	}
	public String getSerializedNotificationObject() {
		return serializedNotificationObject;
	}
	public void setSerializedNotificationObject(String serializedNotificationObject) {
		this.serializedNotificationObject = serializedNotificationObject;
	}	

}
