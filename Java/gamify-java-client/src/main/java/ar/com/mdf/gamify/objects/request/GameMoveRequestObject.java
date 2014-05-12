package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class GameMoveRequestObject extends SessionRequestObject{

	@SerializedName("Number")
	private String number;

	public String getNumber() {
		return number;
	}

	public void setNumber(String number) {
		this.number = number;
	}

}
