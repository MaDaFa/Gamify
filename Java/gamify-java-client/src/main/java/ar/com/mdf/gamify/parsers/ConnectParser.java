package ar.com.mdf.gamify.parsers;

import java.util.List;
import java.util.Vector;

import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.interfaces.IMessageParser;
import ar.com.mdf.gamify.interfaces.IObserver;
import ar.com.mdf.gamify.objects.notification.PlayerConnectedNotificationObject;

import com.google.gson.Gson;

public class ConnectParser implements IMessageParser<PlayerConnectedNotificationObject>{
	
	List<IObserver<PlayerConnectedNotificationObject>> observers;

	public boolean canParse(NotificationType type) {
		return NotificationType.PlayerConnected.equals(type);
	}

	public void notify(String message) {
		Gson gson = new Gson();
		PlayerConnectedNotificationObject pcn = gson.fromJson(message, PlayerConnectedNotificationObject.class);
		for (IObserver<PlayerConnectedNotificationObject> observer : this.observers) {
			observer.notify(pcn);
		}
	}

	public void addObserver(IObserver<PlayerConnectedNotificationObject> observer) {
		if(this.observers == null)
			this.observers = new Vector<IObserver<PlayerConnectedNotificationObject>>();
		this.observers.add(observer);
	}

	public boolean canParse(Class<?> type) {
		
		return PlayerConnectedNotificationObject.class.equals(type);
	}

}
