package ar.com.mdf.gamify.objects.notification;

import java.util.List;

import com.google.gson.annotations.SerializedName;

public class SendConnectedPlayersNotificationObject implements INotificationObject {
	@SerializedName("ConnectedPlayersCount")
	private int connectedPlayersCount;
	@SerializedName("ConnectedPlayerNames")
	private List<String> connectedPlayerNames;
	
	public int getConnectedPlayersCount() {
		return connectedPlayersCount;
	}
	public void setConnectedPlayersCount(int connectedPlayersCount) {
		this.connectedPlayersCount = connectedPlayersCount;
	}
	public List<String> getConnectedPlayerNames() {
		return connectedPlayerNames;
	}
	public void setConnectedPlayerNames(List<String> connectedPlayerNames) {
		this.connectedPlayerNames = connectedPlayerNames;
	}
}
