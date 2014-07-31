package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.observers.IGameObserver;

public interface IGameListener<U extends INotificationObject> {

	public void suscribe(IGameObserver<U> observer);
}
