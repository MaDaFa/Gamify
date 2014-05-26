package ar.com.mdf.gamify.exceptions;

public class NotClienInitializedExcepcion extends Exception {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	public NotClienInitializedExcepcion() {
		super("The server has not been previously initialized.");
	}
}
