package ar.com.mdf.gamify;

import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.ClientFactory;
import ar.com.mdf.gamify.managers.GameCreationManager;
import ar.com.mdf.gamify.objects.request.Player;
import ar.com.mdf.gamify.observers.StartGameObserver;

public class AcceptGameTest {
	
public static void main(String[] args) {
		
		try {
			String gameServerUri = "ws://ec2-54-207-14-192.sa-east-1.compute.amazonaws.com/guessmynumber/play";
			Player player = new Player();
			player.setPlayerName("mauro");
			
			ClientFactory clientFactory = new ClientFactory(gameServerUri, player);
			GameCreationManager manager = new GameCreationManager(clientFactory);
	
			StartGameObserver sgo = new StartGameObserver();
			
			manager.suscribeCreateGame(sgo);
			
			
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
