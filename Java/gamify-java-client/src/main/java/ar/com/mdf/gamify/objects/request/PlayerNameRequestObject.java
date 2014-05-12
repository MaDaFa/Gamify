package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class PlayerNameRequestObject {

	@SerializedName("PlayerName")
	private String playerName;

	public String getPlayerName() {
		return playerName;
	}

	public void setPlayerName(String playerName) {
		this.playerName = playerName;
	}
	
}
