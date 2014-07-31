package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class MessageRequestObject implements IRequestObject {
	@SerializedName("Message")
	private String message;
	@SerializedName("FromPlayerName")
	private String fromPlayerName;
	
	public String getMessage() {
		return message;
	}
	public void setMessage(String message) {
		this.message = message;
	}
	public String getFromPlayerName() {
		return fromPlayerName;
	}
	public void setFromPlayerName(String fromPlayerName) {
		this.fromPlayerName = fromPlayerName;
	}
}
