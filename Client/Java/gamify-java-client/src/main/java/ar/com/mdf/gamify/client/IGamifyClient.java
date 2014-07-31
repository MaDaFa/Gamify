package ar.com.mdf.gamify.client;

import ar.com.mdf.gamify.objects.request.GameRequest;
import ar.com.mdf.gamify.observers.IGameMessageObserver;

public interface IGamifyClient {

	public void send(GameRequest gameNotification);
	public void suscribe(IGameMessageObserver gameNotification);
	public void close();
}
