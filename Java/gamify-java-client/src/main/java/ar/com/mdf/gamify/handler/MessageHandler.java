package ar.com.mdf.gamify.handler;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.List;
import java.util.Vector;

import ar.com.mdf.gamify.client.ClientWebSocket;
import ar.com.mdf.gamify.enums.NotificationType;
import ar.com.mdf.gamify.exceptions.NotConnectedException;
import ar.com.mdf.gamify.interfaces.IMessageHandler;
import ar.com.mdf.gamify.interfaces.IMessageParserBase;
import ar.com.mdf.gamify.objects.notification.GameNotification;

import com.google.gson.Gson;

public class MessageHandler implements IMessageHandler{

	static public MessageHandler messageHandler;
	private ClientWebSocket client;
	private List<IMessageParserBase> parsers;
	
	public static MessageHandler getMessageHandler(String player) throws NotConnectedException{
		if(messageHandler == null){
			messageHandler = new MessageHandler(player);
			messageHandler.parsers = new Vector<IMessageParserBase>();
		}
		return messageHandler;
	}
	
	private MessageHandler(String player) throws NotConnectedException {
		try {
			this.client = new ClientWebSocket(new URI("ws://ec2-54-207-14-192.sa-east-1.compute.amazonaws.com/guessmynumber/api/guessmynumber?userName="+player));
			this.client.addHandler(this);
			this.client.connect();
		} catch (URISyntaxException e) {
			throw new NotConnectedException(e);
		}
	}
	
	public void notify(String response) {
		Gson gson = new Gson();
		GameNotification gn = gson.fromJson(response, GameNotification.class);
		for (IMessageParserBase parser : parsers) {
			if(parser.canParse(NotificationType.getNotificationTypeByType(gn.getType())))
				parser.notify(gn.getSerializedNotificationObject());
				break;
		}
	}
	
	public void addParser(IMessageParserBase parser){
		parsers.add(parser);
	}

	public <T> void sendMessage(T message){
		Gson gson = new Gson();
		this.client.send(gson.toJson(message));
	}
	

}
