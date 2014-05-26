package ar.com.mdf.gamify;

import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.ClientFactory;
import ar.com.mdf.gamify.managers.GameCreationManager;
import ar.com.mdf.gamify.objects.request.Player;
import ar.com.mdf.gamify.objects.request.StartGameRequestObject;
import ar.com.mdf.gamify.observers.StartGameObserver;

public class StartGameTest {
	
public static void main(String[] args) {
		
		try {
			String gameServerUri = "ws://ec2-54-207-14-192.sa-east-1.compute.amazonaws.com/guessmynumber/play";
			Player player = new Player();
			player.setPlayerName("LALALA");
			
			ClientFactory clientFactory = new ClientFactory(gameServerUri, player);
			GameCreationManager manager = new GameCreationManager(clientFactory);
			
			String invitedPlayerName = "mauro";
			
			StartGameRequestObject inviteRequest = new StartGameRequestObject();
			inviteRequest.setPlayerName(player.getPlayerName());
			inviteRequest.setInvitedPlayerName(invitedPlayerName);
			inviteRequest.setAdditionalInformation("5826");
			
			manager.createGame(inviteRequest);
			
			
			Thread.sleep(100000);
		} catch (NotConnectedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} 
		
	}

}
