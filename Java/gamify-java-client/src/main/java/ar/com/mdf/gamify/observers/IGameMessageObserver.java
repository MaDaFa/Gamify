package ar.com.mdf.gamify.observers;

import ar.com.mdf.gamify.objects.notification.GameNotification;

public interface IGameMessageObserver {

	public void notify(GameNotification notification);
}
