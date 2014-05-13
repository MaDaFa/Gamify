package ar.com.mdf.gamify.observers;

import ar.com.mdf.gamify.interfaces.IObserver;
import ar.com.mdf.gamify.objects.notification.PlayerConnectedNotificationObject;

public class PlayerConnectedObserver implements IObserver<PlayerConnectedNotificationObject> {

	public void notify(PlayerConnectedNotificationObject notificationObject) {
		System.out.println(notificationObject.getMessage());
	}

	
}
