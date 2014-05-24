package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.objects.notification.GameNotification;
import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.observers.IGameObserver;
import ar.com.mdf.gamify.parsers.GameParser;
import ar.com.mdf.gamify.parsers.IGameParser;

public class GameListener<U extends INotificationObject> implements IGameListener<U>{


	private NotificationType notificationType;
	private IGameObserver<U> observer;
	private IGameParser<U> notificationParser;
	private Class<U> notificationClazz;
	
	public GameListener(NotificationType nt, Class<U> notificationClazz) {
		this.notificationType = nt;
		this.notificationClazz = notificationClazz;
		this.notificationParser = new GameParser<U>();
	}

	public void suscribe(IGameObserver<U> observer) {
		this.setObserver(observer);
	}
	
	public void notify(GameNotification notification) {
		if(this.canHandleNotification(notification)) {
			this.handleNotification(notification);
		}
	}
	
	private boolean canHandleNotification(GameNotification notification){
		return this.notificationType.getType().equals(notification.getType());
	}
	
	private void handleNotification(GameNotification notification){
		String serializedNotificationObject = notification.getSerializedNotificationObject();		

		U notificationObject = (U)this.getNotificationParser().parseToObject(serializedNotificationObject, this.notificationClazz);
		
		this.getObserver().notify(notificationObject);
	}
	
	protected IGameParser<U> getNotificationParser() {
		return this.notificationParser;
	}
	
	protected IGameObserver<U> getObserver() {
		return this.observer;
	}
	
	protected void setObserver(IGameObserver<U> observer) {
		this.observer = observer;
	}
	
}
