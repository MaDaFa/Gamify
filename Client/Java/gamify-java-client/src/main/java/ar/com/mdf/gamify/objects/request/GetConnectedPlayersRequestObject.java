package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class GetConnectedPlayersRequestObject implements IRequestObject {
	@SerializedName("PageSize")
	private int pageSize;
	@SerializedName("PlayerName")
	private String playerName;
	
	public int getPageSize() {
		return pageSize;
	}

	public void setPageSize(int pageSize) {
		this.pageSize = pageSize;
	}

	public String getPlayerName() {
		return playerName;
	}

	public void setPlayerName(String playerName) {
		this.playerName = playerName;
	}
}
