package ar.com.mdf.gamify.objects.notification;

import java.util.List;

import com.google.gson.annotations.SerializedName;

public class SendActiveGamesNotificationObject extends ConnectionStatePlayerNotificationObject {
	@SerializedName("ActiveGames")
	private List<GameObject> activeGames;
	@SerializedName("activeGamesCount")
	private int activeGamesCount;
	
	public List<GameObject> getActiveGames() {
		return activeGames;
	}
	public void setActiveGames(List<GameObject> activeGames) {
		this.activeGames = activeGames;
	}
	public int getActiveGamesCount() {
		return activeGamesCount;
	}
	public void setActiveGamesCount(int activeGamesCount) {
		this.activeGamesCount = activeGamesCount;
	}
}
