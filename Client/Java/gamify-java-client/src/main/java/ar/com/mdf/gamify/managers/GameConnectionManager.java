package ar.com.mdf.gamify.managers;

import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.IClientFactory;
import ar.com.mdf.gamify.objects.notification.ConnectionStatePlayerNotificationObject;
import ar.com.mdf.gamify.objects.notification.SendConnectedPlayersNotificationObject;
import ar.com.mdf.gamify.objects.request.GetConnectedPlayersRequestObject;
import ar.com.mdf.gamify.objects.request.PlayerNameRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.services.GameService;
import ar.com.mdf.gamify.services.IGameService;

public class GameConnectionManager {
	private IGameService<PlayerNameRequestObject, ConnectionStatePlayerNotificationObject> playerConnectService;
	private IGameService<PlayerNameRequestObject, ConnectionStatePlayerNotificationObject> playerDisconnectService;
	private IGameService<GetConnectedPlayersRequestObject, SendConnectedPlayersNotificationObject> connectedPlayersService;
	private IGamifyClient client;
	
	public GameConnectionManager(IClientFactory clientFactory) throws NotConnectedException {
		try {
			this.client = clientFactory.getClient();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
		this.playerConnectService = new GameService<PlayerNameRequestObject, ConnectionStatePlayerNotificationObject>(RequestType.PlayerConnect, NotificationType.PlayerConnected, this.client, ConnectionStatePlayerNotificationObject.class);
		this.playerDisconnectService = new GameService<PlayerNameRequestObject, ConnectionStatePlayerNotificationObject>(RequestType.PlayerDisconnect, NotificationType.PlayerDisconnected, this.client, ConnectionStatePlayerNotificationObject.class);
		this.connectedPlayersService = new GameService<GetConnectedPlayersRequestObject, SendConnectedPlayersNotificationObject>(RequestType.GetConnectedPlayers, NotificationType.SendConnectedPlayers, this.client, SendConnectedPlayersNotificationObject.class);
	}
	
	public void suscribePlayerConnect(IGameObserver<ConnectionStatePlayerNotificationObject> observer){
		this.playerConnectService.suscribe(observer);
	}
	public void suscribePlayerDisconnect(IGameObserver<ConnectionStatePlayerNotificationObject> observer){
		this.playerDisconnectService.suscribe(observer);
	}
	public void suscribeConnectedPlayers(IGameObserver<SendConnectedPlayersNotificationObject> observer){
		this.connectedPlayersService.suscribe(observer);
	}

	public void connect(PlayerNameRequestObject connectRequest) {
		this.playerConnectService.send(connectRequest);
	}
	public void disconnect(PlayerNameRequestObject disconnectRequest) {
		this.playerDisconnectService.send(disconnectRequest);
	}
	public void rejectGame(GetConnectedPlayersRequestObject connectedPlayersRequest) {
		this.connectedPlayersService.send(connectedPlayersRequest);
	}
}
