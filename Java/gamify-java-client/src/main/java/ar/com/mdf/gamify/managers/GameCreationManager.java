package ar.com.mdf.gamify.managers;


import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.IClientFactory;
import ar.com.mdf.gamify.objects.notification.InvitationToGameNotificationObject;
import ar.com.mdf.gamify.objects.notification.ResponseInvitationGameNotificationObject;
import ar.com.mdf.gamify.objects.request.AcceptGameRequestObject;
import ar.com.mdf.gamify.objects.request.SessionRequestObject;
import ar.com.mdf.gamify.objects.request.StartGameRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.services.GameService;
import ar.com.mdf.gamify.services.IGameService;

public class GameCreationManager {
	
	private IGameService<StartGameRequestObject, InvitationToGameNotificationObject> createGameService;
	private IGameService<AcceptGameRequestObject, ResponseInvitationGameNotificationObject> acceptGameService;
	private IGameService<SessionRequestObject, ResponseInvitationGameNotificationObject> rejectGameService;
	private IGamifyClient client;
	
	public GameCreationManager(IClientFactory clientFactory) throws NotConnectedException {
		try {
			this.client = clientFactory.getClient();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
		this.createGameService = new GameService<StartGameRequestObject, InvitationToGameNotificationObject>(RequestType.CreateGame, NotificationType.GameInvite, this.client, InvitationToGameNotificationObject.class);
		this.acceptGameService = new GameService<AcceptGameRequestObject, ResponseInvitationGameNotificationObject>(RequestType.GameAccepted, NotificationType.GameCreated, this.client,ResponseInvitationGameNotificationObject.class);
		this.rejectGameService = new GameService<SessionRequestObject, ResponseInvitationGameNotificationObject>(RequestType.GameRejected, NotificationType.GameRejected, this.client, ResponseInvitationGameNotificationObject.class);
	}
	
	public void suscribeCreateGame(IGameObserver<InvitationToGameNotificationObject> observer){
		this.createGameService.suscribe(observer);
	}
	public void suscribeAcceptGame(IGameObserver<ResponseInvitationGameNotificationObject> observer){
		this.acceptGameService.suscribe(observer);
	}
	public void suscribeRejectGame(IGameObserver<ResponseInvitationGameNotificationObject> observer){
		this.rejectGameService.suscribe(observer);
	}

	public void createGame(StartGameRequestObject inviteRequest) {
		createGameService.send(inviteRequest);
	}
	public void acceptGame(AcceptGameRequestObject acceptGameRequest) {
		acceptGameService.send(acceptGameRequest);
	}
	public void rejectGame(SessionRequestObject rejectGameRequest) {
		rejectGameService.send(rejectGameRequest);
	}

}
