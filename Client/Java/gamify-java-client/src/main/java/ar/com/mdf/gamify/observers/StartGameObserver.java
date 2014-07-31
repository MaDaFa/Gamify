package ar.com.mdf.gamify.observers;

import ar.com.mdf.gamify.objects.notification.InvitationToGameNotificationObject;

public class StartGameObserver implements IGameObserver<InvitationToGameNotificationObject> {

	public void notify(InvitationToGameNotificationObject notificationObject) {
		System.out.println(notificationObject.getMessage());
	}

	
}
