package ar.com.mdf.gamify.managers;

import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.IClientFactory;
import ar.com.mdf.gamify.objects.notification.GameAbandonedNotificationObject;
import ar.com.mdf.gamify.objects.notification.GameFinishedNotificationObject;
import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.objects.notification.SendActiveGamesNotificationObject;
import ar.com.mdf.gamify.objects.request.PlayerNameRequestObject;
import ar.com.mdf.gamify.objects.request.SessionRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.services.GameListener;
import ar.com.mdf.gamify.services.GameService;
import ar.com.mdf.gamify.services.IGameListener;
import ar.com.mdf.gamify.services.IGameService;

public class GameProgressManager<TGameInformationNotification extends INotificationObject> {
	private IGameService<SessionRequestObject, TGameInformationNotification> openGameService;
	private IGameService<PlayerNameRequestObject, SendActiveGamesNotificationObject> activeGamesService;
	private IGameService<SessionRequestObject, GameAbandonedNotificationObject> abandonGameService;
	private IGameListener<GameFinishedNotificationObject> gameFinishedListener;
	private IGamifyClient client;
	
	public GameProgressManager(IClientFactory clientFactory, Class<TGameInformationNotification> gameInformationNotificationClass) throws NotConnectedException {
		try {
			this.client = clientFactory.getClient();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
		this.openGameService = new GameService<SessionRequestObject, TGameInformationNotification>(RequestType.OpenGame, NotificationType.SendGameInformation, this.client, gameInformationNotificationClass);
		this.activeGamesService = new GameService<PlayerNameRequestObject, SendActiveGamesNotificationObject>(RequestType.GetActiveGames, NotificationType.SendActiveGames, this.client, SendActiveGamesNotificationObject.class);
		this.abandonGameService = new GameService<SessionRequestObject, GameAbandonedNotificationObject>(RequestType.AbandonGame, NotificationType.GameAbandoned, this.client, GameAbandonedNotificationObject.class);
		this.gameFinishedListener = new GameListener<GameFinishedNotificationObject>(NotificationType.GameFinished, GameFinishedNotificationObject.class);
	}
	
	public void suscribeOpenGame(IGameObserver<TGameInformationNotification> observer){
		this.openGameService.suscribe(observer);
	}
	public void suscribeActiveGames(IGameObserver<SendActiveGamesNotificationObject> observer){
		this.activeGamesService.suscribe(observer);
	}
	public void suscribeAbandonGame(IGameObserver<GameAbandonedNotificationObject> observer){
		this.abandonGameService.suscribe(observer);
	}
	public void suscribeGameFinished(IGameObserver<GameFinishedNotificationObject> observer){
		this.gameFinishedListener.suscribe(observer);
	}
	
	public void openGame(SessionRequestObject openGameRequest) {
		this.openGameService.send(openGameRequest);
	}
	public void getActiveGames(PlayerNameRequestObject getActiveGamesRequest) {
		this.activeGamesService.send(getActiveGamesRequest);
	}
	public void abandonGame(SessionRequestObject abandonGameRequest) {
		this.abandonGameService.send(abandonGameRequest);
	}
}
