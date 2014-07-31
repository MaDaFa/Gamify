package ar.com.mdf.gamify.factory;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.HashMap;

import ar.com.mdf.gamify.client.GamifyClient;
import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.objects.request.Player;

public class ClientFactory implements IClientFactory {
	
	private static HashMap<String, IGamifyClient> clients = new HashMap<String, IGamifyClient>();
	
	private String gameServerUri;
	private Player player;
	
	public ClientFactory(String gameServerUri, Player player) {
		this.gameServerUri = gameServerUri;
		this.player = player;
	}
	
	public IGamifyClient getClient() throws URISyntaxException {
		String playerName = this.player.getPlayerName();
		if(!clients.containsKey(playerName)){
			String completeGameServerUri = this.gameServerUri + "?userName=" + playerName;
			URI url = new URI(completeGameServerUri);
			IGamifyClient client = new GamifyClient(url);
			
			clients.put(playerName, client);
		}
		
		return clients.get(playerName);
	}

}
