package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.objects.notification.GameNotification;
import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.objects.request.GameRequest;
import ar.com.mdf.gamify.objects.request.IRequestObject;
import ar.com.mdf.gamify.observers.IGameMessageObserver;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.parsers.GameParser;
import ar.com.mdf.gamify.parsers.IGameParser;

public class GameService <T extends IRequestObject, U extends INotificationObject > extends GameListener<U> implements IGameService<T, U>, IGameMessageObserver {
	
	private RequestType requestType;
	private IGamifyClient gameClient;
	private IGameParser<T> requestParser;
	
	public GameService(RequestType rt, NotificationType nt, IGamifyClient client, Class<U> notificationClazz) {
		super(nt, notificationClazz);
		this.requestType = rt;
		this.gameClient = client;
		this.requestParser = new GameParser<T>();
		this.gameClient.suscribe(this);
	}

	protected IGameParser<T> getRequestParser() {
		return requestParser;
	}
	
	
	public void send(T request) {
		GameRequest gr = new GameRequest();
		gr.setType(this.requestType.getType());
		String serializedReuqest = this.getRequestParser().parseToString(request);
		gr.setSerializedRequestObject(serializedReuqest);
		this.gameClient.send(gr);
	}
	
		
	public void disConnect() {
		this.gameClient.close();
	}

	
}
