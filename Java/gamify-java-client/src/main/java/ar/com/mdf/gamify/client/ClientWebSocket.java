package ar.com.mdf.gamify.client;

import java.net.URI;

import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;

import ar.com.mdf.gamify.interfaces.IMessageHandler;

public class ClientWebSocket extends WebSocketClient{
	
	private IMessageHandler handler;

	public ClientWebSocket(URI serverURI) {
		super(serverURI);
		// TODO Auto-generated constructor stub
	}

	@Override
	public void onClose(int arg0, String arg1, boolean arg2) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onError(Exception arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onMessage(String response) {
		this.handler.notify(response);
	}

	@Override
	public void onOpen(ServerHandshake arg0) {
		System.out.println("Me conecte");
	}
	
	public void addHandler(IMessageHandler handler){
		this.handler = handler;
	}
	

}
