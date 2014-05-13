package ar.com.mdf.gamify;

import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.observers.PlayerConnectedObserver;
import ar.com.mdf.gamify.services.ConnectedService;

public class ConnectTest {
	
	public static void main(String[] args) {
		
		ConnectedService cs = new ConnectedService();
		PlayerConnectedObserver pco = new PlayerConnectedObserver();
		
		cs.addObserver(pco);
		
		try {
			cs.connect("JugadorPrueba");
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
