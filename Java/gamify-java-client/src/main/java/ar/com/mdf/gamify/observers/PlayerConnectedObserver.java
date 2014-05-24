package ar.com.mdf.gamify.observers;

import ar.com.mdf.gamify.objects.notification.ConnectionStatePlayerNotificationObject;

public class PlayerConnectedObserver implements IGameObserver<ConnectionStatePlayerNotificationObject> {

	public void notify(ConnectionStatePlayerNotificationObject notificationObject) {
		System.out.println(notificationObject.getMessage());
	}

	
}
