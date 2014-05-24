package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.objects.notification.INotificationObject;
import ar.com.mdf.gamify.objects.request.IRequestObject;
import ar.com.mdf.gamify.observers.IGameObserver;

public interface IGameService<T extends IRequestObject, U extends INotificationObject > extends IGameSender<T>, IGameListener<U>{
	
		public void suscribe(IGameObserver<U> observer);
		public void send(T request);
		
}
