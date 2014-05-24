package ar.com.mdf.gamify.exceptions;

import java.net.URISyntaxException;

public class NotConnectedException extends Exception {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	public NotConnectedException(URISyntaxException e) {
		super("Not connected" + e.getMessage());
	}

}
