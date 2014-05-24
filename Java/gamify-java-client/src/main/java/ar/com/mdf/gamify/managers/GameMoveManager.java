package ar.com.mdf.gamify.managers;

import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.IClientFactory;
import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.objects.request.IRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.services.GameListener;
import ar.com.mdf.gamify.services.GameService;
import ar.com.mdf.gamify.services.IGameListener;
import ar.com.mdf.gamify.services.IGameService;

public class GameMoveManager<TGameMoveRequest extends IRequestObject, TMoveResultNotification extends INotificationObject, TMoveNotification extends INotificationObject> {
	private IGameService<TGameMoveRequest, TMoveResultNotification> gameMoveService;
    private IGameListener<TMoveNotification> gameMoveListener;
    private IGamifyClient client;
    
    public GameMoveManager(IClientFactory clientFactory, Class<TMoveResultNotification> resultNotificationClass, Class<TMoveNotification> moveNotificationClass) throws NotConnectedException {
		try {
			this.client = clientFactory.getClient();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
		this.gameMoveService = new GameService<TGameMoveRequest, TMoveResultNotification>(RequestType.GameMove, NotificationType.GameMoveResult, this.client, resultNotificationClass);
		this.gameMoveListener = new GameListener<TMoveNotification>(NotificationType.GameMove, moveNotificationClass);
	}
    
    public void suscribeMoveResult(IGameObserver<TMoveResultNotification> observer){
		this.gameMoveService.suscribe(observer);
	}
    public void suscribeMove(IGameObserver<TMoveNotification> observer){
		this.gameMoveListener.suscribe(observer);
	}
    
	public void sendMove(TGameMoveRequest moveRequest) {
		this.gameMoveService.send(moveRequest);
	}
}
