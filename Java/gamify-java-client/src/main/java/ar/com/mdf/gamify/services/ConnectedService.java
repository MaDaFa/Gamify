package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.handler.MessageHandler;
import ar.com.mdf.gamify.interfaces.IObserver;
import ar.com.mdf.gamify.objects.notification.PlayerConnectedNotificationObject;
import ar.com.mdf.gamify.parsers.ConnectParser;

public class ConnectedService {
	
	private ConnectParser parser;
	
	public ConnectedService() {
		this.parser = new ConnectParser();
	}
	
	//TODO: Mauro mejora esto.
	public void connect(String player) throws NotConnectedException{
		MessageHandler.getMessageHandler(player);
		MessageHandler.getMessageHandler(player).addParser(parser);
	}
	
	public void addObserver(IObserver<PlayerConnectedNotificationObject> observer){
		this.parser.addObserver(observer);
	}

}
