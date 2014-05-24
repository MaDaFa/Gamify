package ar.com.mdf.gamify.client;

import java.net.URI;
import java.util.List;
import java.util.Vector;

import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;

import ar.com.mdf.gamify.objects.notification.GameNotification;
import ar.com.mdf.gamify.objects.request.GameRequest;
import ar.com.mdf.gamify.observers.IGameMessageObserver;
import ar.com.mdf.gamify.parsers.GameParser;
import ar.com.mdf.gamify.parsers.IGameParser;

public class GamifyClient implements IGamifyClient // extends WebSocketClient
{
	
	private List<IGameMessageObserver> observersNotification;
	private WebSocketClient gameClient;
	private IGameParser<GameRequest> parserRequest;
	private IGameParser<GameNotification> parserNotification;
	static GamifyClient gamifyClient;
	
	public static GamifyClient getClient(URI url){
		if(gamifyClient == null){
			gamifyClient = new GamifyClient(url);
		}
		return gamifyClient;
	}
	
	
	public GamifyClient(URI serverURI) {
		this.parserRequest = new GameParser<GameRequest>();
		this.parserNotification = new GameParser<GameNotification>();
		this.gameClient = new WebSocketClient(serverURI) {
			
			@Override
			public void onClose(int arg0, String arg1, boolean arg2) {
				// TODO Auto-generated method stub
				
			}

			@Override
			public void onError(Exception arg0) {
				// TODO Auto-generated method stub
				System.out.println(arg0);
			}

			@Override
			public void onMessage(String response) {
				GameNotification gameNotification = parserNotification.parseToObject(response, GameNotification.class);
				for (IGameMessageObserver service : observersNotification) {
					service.notify(gameNotification);
				}
			}

			@Override
			public void onOpen(ServerHandshake arg0) {
				System.out.println("Open");
			}

		};
		this.gameClient.connect();
		this.observersNotification = new Vector<IGameMessageObserver>();
	}

	public void suscribe(IGameMessageObserver service) {
		this.observersNotification.add(service);
	}

	public void send(GameRequest gameNotification) {
		String message = this.parserRequest.parseToString(gameNotification);
		this.gameClient.send(message);
	}

	public void close() {
		this.gameClient.close();
	}
	

}
