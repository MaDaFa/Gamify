package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class ErrorNotificationObject extends NotificationObject {

	@SerializedName("ErrorCode")
	private String errorCode;

	public String getErrorCode() {
		return errorCode;
	}

	public void setErrorCode(String errorCode) {
		this.errorCode = errorCode;
	}

	
}
