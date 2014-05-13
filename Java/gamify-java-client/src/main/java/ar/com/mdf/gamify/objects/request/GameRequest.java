package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class GameRequest {
	
	@SerializedName("Type")
	private Integer type;
	@SerializedName("SerializedRequestObject")
	private String serializedRequestObject;
	
	public Integer getType() {
		return type;
	}
	public void setType(Integer type) {
		this.type = type;
	}
	public String getSerializedRequestObject() {
		return serializedRequestObject;
	}
	public void setSerializedRequestObject(String serializedRequestObject) {
		this.serializedRequestObject = serializedRequestObject;
	}
		

}
