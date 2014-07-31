package ar.com.mdf.gamify.managers;

import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.enums.RequestType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.factory.IClientFactory;
import ar.com.mdf.gamify.objects.notification.ErrorNotificationObject;
import ar.com.mdf.gamify.objects.notification.MessageNotificationObject;
import ar.com.mdf.gamify.objects.request.MessageRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.services.GameListener;
import ar.com.mdf.gamify.services.GameService;
import ar.com.mdf.gamify.services.IGameListener;
import ar.com.mdf.gamify.services.IGameService;

public class MessageManager {
	private IGameService<MessageRequestObject, MessageNotificationObject> messageService;
	private IGameService<MessageRequestObject, MessageNotificationObject> typingMessageService;
	private IGameListener<ErrorNotificationObject> errorListener;
	private IGamifyClient client;
	
	public MessageManager(IClientFactory clientFactory) throws NotConnectedException {
		try {
			this.client = clientFactory.getClient();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
		this.messageService = new GameService<MessageRequestObject, MessageNotificationObject>(RequestType.Message, NotificationType.Message, this.client, MessageNotificationObject.class);
		this.typingMessageService = new GameService<MessageRequestObject, MessageNotificationObject>(RequestType.TypingMessage, NotificationType.TypingMessage, this.client, MessageNotificationObject.class);
		this.errorListener = new GameListener<ErrorNotificationObject>(NotificationType.Error, ErrorNotificationObject.class);
	}
	
	public void suscribeMessage(IGameObserver<MessageNotificationObject> observer){
		this.messageService.suscribe(observer);
	}
	public void suscribeTypingMessage(IGameObserver<MessageNotificationObject> observer){
		this.typingMessageService.suscribe(observer);
	}
	public void suscribeError(IGameObserver<ErrorNotificationObject> observer){
		this.errorListener.suscribe(observer);
	}
	
	public void sendMessage(MessageRequestObject openGameRequest) {
		this.messageService.send(openGameRequest);
	}
	public void sendTypingMessage(MessageRequestObject openGameRequest) {
		this.typingMessageService.send(openGameRequest);
	}
}
