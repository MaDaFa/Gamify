package ar.com.mdf.gamify.factory;

import java.net.URISyntaxException;

import ar.com.mdf.gamify.client.IGamifyClient;

public interface IClientFactory {
	public IGamifyClient getClient() throws URISyntaxException;
}
